namespace Prestamium.Entities
{
    public class Loan : BaseEntity
    {
        public decimal Amount { get; set; } // Monto prestado
        public decimal InterestRate { get; set; } // Tasa de interés
        public int Fees { get; set; } // Número de cuotas
        public string Frequency { get; set; } = default!; // "monthly" o "biweekly"
        public decimal TotalAmountDue { get; set; } // Monto total a pagar
        public decimal TotalInterestReceivable { get; set; } // Total interés a cobrar
        public decimal PaymentAmount { get; set; } // Monto de cada cuota
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal RemainingBalance { get; set; } // Saldo pendiente

        // Relaciones
        public int ClientId { get; set; }
        public Client Client { get; set; } = default!;
        public int BoxId { get; set; }
        public Box Box { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public User User { get; set; } = default!;
        public ICollection<Installment> Installments { get; set; } = new List<Installment>();
    }

}