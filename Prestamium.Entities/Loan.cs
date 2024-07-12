namespace Prestamium.Entities
{
    public class Loan : BaseEntity
    {
        public int ClientId { get; set; }
        public decimal Amount { get; set; }
        public decimal RateInterest { get; set; }
        public decimal DeadlineMonths { get; set; }
        public string PaymentMethod { get; set; } = default!;
        public string PaymentStatus { get; set; } = default!;

    }
}
