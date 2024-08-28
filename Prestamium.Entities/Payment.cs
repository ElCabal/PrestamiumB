namespace Prestamium.Entities
{
    public class Payment : BaseEntity
    {
        public int InstallmentId { get; set; }
        public Installment Installment { get; set; } = default!;
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
    }
}
