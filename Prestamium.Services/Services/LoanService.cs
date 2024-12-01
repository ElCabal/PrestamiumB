using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Prestamium.Dto.Request;
using Prestamium.Dto.Response;
using Prestamium.Entities;
using Prestamium.Persistence;
using Prestamium.Repositories.Interfaces;
using Prestamium.Services.Interfaces;

namespace Prestamium.Services.Services
{
    public class LoanService : ILoanService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoanRepository _loanRepository;
        private readonly IBoxRepository _boxRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IInstallmentRepository _installmentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<LoanService> _logger;

        public LoanService(
            ApplicationDbContext context,
            ILoanRepository loanRepository,
            IBoxRepository boxRepository,
            IClientRepository clientRepository,
            IInstallmentRepository installmentRepository,
            IMapper mapper,
            ILogger<LoanService> logger)
        {
            _context = context;
            _loanRepository = loanRepository;
            _boxRepository = boxRepository;
            _clientRepository = clientRepository;
            _installmentRepository = installmentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponseGeneric<int>> CreateAsync(LoanRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                if (request.Amount <= 0)
                {
                    response.ErrorMessage = "El monto del préstamo debe ser mayor a cero";
                    return response;
                }

                if (request.InterestRate <= 0)
                {
                    response.ErrorMessage = "La tasa de interés debe ser mayor a cero";
                    return response;
                }

                if (!new[] { "monthly", "biweekly" }.Contains(request.Frequency.ToLower()))
                {
                    response.ErrorMessage = "La frecuencia debe ser 'monthly' o 'biweekly'";
                    return response;
                }

                // Validar que exista el cliente
                var client = await _clientRepository.GetByIdAsync(request.ClientId);
                if (client == null)
                {
                    response.ErrorMessage = "Cliente no encontrado";
                    return response;
                }

                // Validar que exista la caja y tenga fondos suficientes
                var box = await _boxRepository.GetByIdAsync(request.BoxId);
                if (box == null)
                {
                    response.ErrorMessage = "Caja no encontrada";
                    return response;
                }

                if (box.CurrentBalance < request.Amount)
                {
                    response.ErrorMessage = "Saldo insuficiente en la caja";
                    return response;
                }

                var loan = _mapper.Map<Loan>(request);

                // Calcular montos según frecuencia
                CalculateLoanAmounts(loan);

                // Crear el préstamo y sus cuotas en una transacción
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // Registrar el préstamo
                    response.Data = await _loanRepository.CreateAsync(loan);

                    if (response.Data > 0)
                    {
                        // Generar las cuotas
                        await GenerateInstallments(loan);

                        // Actualizar saldo de la caja
                        box.CurrentBalance -= loan.Amount;
                        await _boxRepository.UpdateAsync();

                        // Registrar la transacción en la caja
                        var boxTransaction = new BoxTransaction
                        {
                            BoxId = request.BoxId,
                            Amount = request.Amount,
                            Type = "expense",
                            Description = $"Desembolso de préstamo #{response.Data} - Cliente: {client.FirstName} {client.LastName}",
                            TransactionDate = DateTime.Now,
                            PreviousBalance = box.CurrentBalance + request.Amount,
                            NewBalance = box.CurrentBalance
                        };

                        await transaction.CommitAsync();
                        response.Success = true;
                    }
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al registrar el préstamo";
                _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        private void CalculateLoanAmounts(Loan loan)
        {
            // Calcular interés total
            decimal monthlyInterest = loan.Amount * (loan.InterestRate / 100);

            if (loan.Frequency.ToLower() == "monthly")
            {
                loan.TotalInterestReceivable = monthlyInterest * loan.Fees;
                loan.EndDate = loan.StartDate.AddMonths(loan.Fees);
            }
            else if (loan.Frequency.ToLower() == "biweekly")
            {
                // Para quincenal, calculamos el interés mensual y lo multiplicamos por los meses
                decimal months = loan.Fees / 2.0m;
                loan.TotalInterestReceivable = monthlyInterest * months;
                loan.EndDate = loan.StartDate.AddDays(loan.Fees * 15);
            }

            loan.TotalAmountDue = loan.Amount + loan.TotalInterestReceivable;
            loan.PaymentAmount = loan.TotalAmountDue / loan.Fees;
            loan.RemainingBalance = loan.TotalAmountDue;
        }

        private async Task GenerateInstallments(Loan loan)
        {
            for (int i = 1; i <= loan.Fees; i++)
            {
                var installment = new Installment
                {
                    LoanId = loan.Id,
                    Amount = loan.PaymentAmount,
                    DueDate = loan.Frequency.ToLower() == "monthly"
                        ? loan.StartDate.AddMonths(i)
                        : loan.StartDate.AddDays(i * 15),
                    IsPaid = false,
                    PaidAmount = 0
                };

                await _installmentRepository.CreateAsync(installment);
            }
        }

        public async Task<BaseResponseGeneric<LoanResponseDto>> GetByIdAsync(int id)
        {
            var response = new BaseResponseGeneric<LoanResponseDto>();
            try
            {
                var loan = await _loanRepository.GetLoanWithDetailsAsync(id);
                if (loan != null)
                {
                    response.Data = _mapper.Map<LoanResponseDto>(loan);
                    response.Success = true;
                }
                else
                {
                    response.ErrorMessage = "Préstamo no encontrado";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al obtener el préstamo";
                _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponseGeneric<ICollection<LoanResponseDto>>> GetByClientIdAsync(int clientId)
        {
            var response = new BaseResponseGeneric<ICollection<LoanResponseDto>>();
            try
            {
                var loans = await _loanRepository.GetLoansByClientAsync(clientId);
                response.Data = _mapper.Map<ICollection<LoanResponseDto>>(loans);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al obtener los préstamos del cliente";
                _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponseGeneric<ICollection<LoanResponseDto>>> GetAllAsync()
        {
            var response = new BaseResponseGeneric<ICollection<LoanResponseDto>>();
            try
            {
                var loans = await _loanRepository.GetAllAsync();
                response.Data = _mapper.Map<ICollection<LoanResponseDto>>(loans);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al obtener los préstamos";
                _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponseGeneric<LoanDetailResponseDto>> GetDetailAsync(int id)
        {
            var response = new BaseResponseGeneric<LoanDetailResponseDto>();
            try
            {
                var loan = await _loanRepository.GetLoanWithDetailsAsync(id);
                if (loan != null)
                {
                    response.Data = _mapper.Map<LoanDetailResponseDto>(loan);
                    response.Success = true;
                }
                else
                {
                    response.ErrorMessage = "Préstamo no encontrado";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al obtener el detalle del préstamo";
                _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponseGeneric<decimal>> CalculateLateFeesAsync(int installmentId, DateTime paymentDate)
        {
            var response = new BaseResponseGeneric<decimal>();
            try
            {
                var installment = await _installmentRepository.GetByIdAsync(installmentId);
                if (installment == null)
                {
                    response.ErrorMessage = "Cuota no encontrada";
                    return response;
                }

                if (paymentDate <= installment.DueDate)
                {
                    response.Data = 0;
                    response.Success = true;
                    return response;
                }

                var loan = await _loanRepository.GetByIdAsync(installment.LoanId);
                if (loan == null)
                {
                    response.ErrorMessage = "Préstamo no encontrado";
                    return response;
                }

                // Calcular días de retraso
                var daysLate = (paymentDate - installment.DueDate).Days;

                // Calcular interés diario: (Interés mensual / 30)
                var monthlyInterest = loan.Amount * (loan.InterestRate / 100);
                var dailyInterest = monthlyInterest / 30;

                // Calcular penalización
                response.Data = dailyInterest * daysLate;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al calcular la mora";
                _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponseGeneric<bool>> RegisterInstallmentPaymentAsync(int installmentId, decimal amount, int boxId)
        {
            var response = new BaseResponseGeneric<bool>();
            try
            {
                var installment = await _installmentRepository.GetByIdAsync(installmentId);
                if (installment == null)
                {
                    response.ErrorMessage = "Cuota no encontrada";
                    return response;
                }

                var loan = await _loanRepository.GetByIdAsync(installment.LoanId);
                if (loan == null)
                {
                    response.ErrorMessage = "Préstamo no encontrado";
                    return response;
                }

                var box = await _boxRepository.GetByIdAsync(boxId);
                if (box == null)
                {
                    response.ErrorMessage = "Caja no encontrada";
                    return response;
                }

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // Registrar el pago de la cuota
                    installment.PaidAmount = amount;
                    installment.PaymentDate = DateTime.Now;
                    installment.IsPaid = amount >= installment.Amount;
                    await _installmentRepository.UpdateAsync();

                    // Actualizar el saldo restante del préstamo
                    loan.RemainingBalance -= amount;
                    await _loanRepository.UpdateAsync();

                    // Registrar la entrada en la caja
                    var boxTransaction = new BoxTransaction
                    {
                        BoxId = boxId,
                        Amount = amount,
                        Type = "income",
                        Description = $"Pago de cuota #{installment.Id} - Préstamo #{loan.Id}",
                        TransactionDate = DateTime.Now,
                        PreviousBalance = box.CurrentBalance,
                        NewBalance = box.CurrentBalance + amount
                    };

                    box.CurrentBalance += amount;
                    await _boxRepository.UpdateAsync();

                    await transaction.CommitAsync();
                    response.Success = true;
                    response.Data = true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al registrar el pago";
                _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }
    }
}
