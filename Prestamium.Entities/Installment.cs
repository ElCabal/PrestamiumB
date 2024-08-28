namespace Prestamium.Entities
{
    public class Installment : BaseEntity
    {
        public int LoanId { get; set; }
        public Loan Loan { get; set; } = default!;
        public DateTime DueDate { get; set; } // Fecha de vencimiento
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
