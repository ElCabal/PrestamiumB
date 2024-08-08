namespace Prestamium.Dto.Response
{
    public class LoanResponseDto
    {
        public class LoanDto
        {
            public int Id { get; set; }
            public decimal Amount { get; set; }
            public decimal TotalAmountDue { get; set; }
            public int Fees { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public decimal InterestRate { get; set; }
            public string Frequency { get; set; } = default!;// Se ha renombrado para ser más legible
            public string ClientName { get; set; } = default!; // Suponiendo que quieres mostrar el nombre del cliente
            public string LoanStatus { get; set; } = default!; // Suponiendo que quieres mostrar el estado del préstamo
            public decimal TotalPaid { get; set; } // Ejemplo de propiedad calculada
            public ICollection<PaymentDto> Payments { get; set; } = new List<PaymentDto>();
        }

    }
}
