namespace Prestamium.Dto.Request
{
    public class LoanRequestDto
    {
        public decimal Amount { get; set; }
        public decimal InterestRate { get; set; }
        public int Fees { get; set; }
        public string Frequency { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public int ClientId { get; set; }
        public int BoxId { get; set; }
    }
}
