namespace Prestamium.Dto.Request
{
    public class LoanRequestDto
    {
        public decimal Amount { get; set; } // Monto a prestar
        public int Fees { get; set; } // Cuotas
        public decimal InterestRate { get; set; } // Tasa de interés
        public DateTime StartDate { get; set; } // Fecha de inicio
        public int ClientId { get; set; } // ID del cliente
        public string Frequency { get; set; } = default!; // Frecuencia del pago (Diario, Semanal, Mensual)
        public int BoxId { get; set; } // ID de la caja
    }
}
