namespace Prestamium.Dto.Response
{
    public class LoanResponseDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal InterestRate { get; set; }
        public int Fees { get; set; }
        public string Frequency { get; set; } = default!;
        public decimal TotalAmountDue { get; set; }
        public decimal TotalInterestReceivable { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal RemainingBalance { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; } = default!;
        public int BoxId { get; set; }
        public ICollection<InstallmentResponseDto> Installments { get; set; } = new List<InstallmentResponseDto>();
    }

    public class LoanSimpleResponseDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string ClientName { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public decimal RemainingBalance { get; set; }
    }

    public class LoanDetailResponseDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal InterestRate { get; set; }
        public int Fees { get; set; }
        public string Frequency { get; set; } = default!;
        public decimal TotalAmountDue { get; set; }
        public decimal TotalInterestReceivable { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal RemainingBalance { get; set; }

        public string ClientName { get; set; } = default!;
        public string BoxName { get; set; } = default!;
        public ICollection<InstallmentResponseDto> Installments { get; set; } = new List<InstallmentResponseDto>();
    }
}
