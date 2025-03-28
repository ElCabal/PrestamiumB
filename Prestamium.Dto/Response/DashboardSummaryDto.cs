using System;
using System.Collections.Generic;

namespace Prestamium.Dto.Response
{
    public class DashboardSummaryDto
    {
        public decimal TotalPrestado { get; set; }
        public decimal SaldoPendiente { get; set; }
        public decimal SaldoDisponible { get; set; }
        public int ClientesRegistrados { get; set; }
        public int ClientesConPrestamos { get; set; }
        public int CajasActivas { get; set; }
        public List<LoanResponseDto> PrestamosRecientes { get; set; } = new List<LoanResponseDto>();
        public List<InstallmentResponseDto> CuotasProximasVencer { get; set; } = new List<InstallmentResponseDto>();
        public List<InstallmentResponseDto> CuotasVencidas { get; set; } = new List<InstallmentResponseDto>();
    }
}
