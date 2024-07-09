namespace Prestamium.Entities
{
    public class Loan
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public decimal Amount { get; set; }
        public decimal RateInterest { get; set; }
        public decimal DeadlineMonths { get; set; }
        public string PaymentMethod { get; set; } = default!;
        public string PaymentStatus { get; set; } = default!;
        public bool Status { get; set; }

    }
}
