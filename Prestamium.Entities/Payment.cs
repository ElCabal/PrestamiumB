namespace Prestamium.Entities
{
    public class Payment : BaseEntity
    {
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public int LoanId { get; set; }
    }
}
