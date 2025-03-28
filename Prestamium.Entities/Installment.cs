﻿namespace Prestamium.Entities
{
    public class Installment : BaseEntity
    {
        public int LoanId { get; set; }
        public Loan Loan { get; set; } = default!;
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal PaidAmount { get; set; }
    }
}
