using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Prestamium.Dto.Response;
using Prestamium.Entities;
using Prestamium.Persistence;
using Prestamium.Repositories.Interfaces;
using Prestamium.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prestamium.Services.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoanRepository _loanRepository;
        private readonly IBoxRepository _boxRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IInstallmentRepository _installmentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(
            ApplicationDbContext context,
            ILoanRepository loanRepository,
            IBoxRepository boxRepository,
            IClientRepository clientRepository,
            IInstallmentRepository installmentRepository,
            IMapper mapper,
            ILogger<DashboardService> logger)
        {
            _context = context;
            _loanRepository = loanRepository;
            _boxRepository = boxRepository;
            _clientRepository = clientRepository;
            _installmentRepository = installmentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponseGeneric<DashboardSummaryDto>> GetDashboardSummaryAsync()
        {
            var response = new BaseResponseGeneric<DashboardSummaryDto>();
            try
            {
                // Crear el objeto de respuesta
                var dashboardSummary = new DashboardSummaryDto();

                // Obtener todos los préstamos
                var loans = await _loanRepository.GetAllAsync();
                var loanDtos = _mapper.Map<List<LoanResponseDto>>(loans);

                // Calcular totales de préstamos
                dashboardSummary.TotalPrestado = loans.Sum(l => l.Amount);
                dashboardSummary.SaldoPendiente = loans.Sum(l => l.RemainingBalance);

                // Obtener préstamos recientes (últimos 5)
                dashboardSummary.PrestamosRecientes = loanDtos
                    .OrderByDescending(l => l.StartDate)
                    .Take(5)
                    .ToList();

                // Obtener cajas
                var boxes = await _boxRepository.GetAllAsync();
                dashboardSummary.CajasActivas = boxes.Count;
                dashboardSummary.SaldoDisponible = boxes.Sum(b => b.CurrentBalance);

                // Obtener clientes
                var clients = await _clientRepository.GetAllAsync();
                dashboardSummary.ClientesRegistrados = clients.Count;

                // Calcular clientes con préstamos activos (saldo pendiente > 0)
                var clientesConPrestamosActivos = loans
                    .Where(l => l.RemainingBalance > 0)
                    .Select(l => l.ClientId)
                    .Distinct()
                    .Count();
                dashboardSummary.ClientesConPrestamos = clientesConPrestamosActivos;

                // Obtener cuotas próximas a vencer y vencidas
                var today = DateTime.Today;
                var allInstallments = await _installmentRepository.GetAllAsync();
                var installmentDtos = _mapper.Map<List<InstallmentResponseDto>>(allInstallments);

                // Filtrar cuotas próximas a vencer (en los próximos 7 días y no pagadas)
                dashboardSummary.CuotasProximasVencer = installmentDtos
                    .Where(i => i.DueDate >= today && i.DueDate <= today.AddDays(7) && !i.IsPaid)
                    .OrderBy(i => i.DueDate)
                    .Take(5)
                    .ToList();

                // Filtrar cuotas vencidas (antes de hoy y no pagadas)
                dashboardSummary.CuotasVencidas = installmentDtos
                    .Where(i => i.DueDate < today && !i.IsPaid)
                    .OrderByDescending(i => i.DueDate)
                    .Take(5)
                    .ToList();

                response.Data = dashboardSummary;
                response.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el resumen del dashboard");
                response.Success = false;
                response.ErrorMessage = "Error al obtener el resumen del dashboard";
            }

            return response;
        }
    }
}
