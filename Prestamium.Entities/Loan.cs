namespace Prestamium.Entities
{
    public class Loan : BaseEntity
    {
        public decimal Amount { get; set; } // Monto a prestar
        public decimal InterestRate { get; set; } // Tasa de interés
        public int Fees { get; set; } // Cuotas
        public string Frecuency { get; set; } = default!; // Diario, Semanal, Mensual
        public decimal TotalAmountDue { get; set; } // Monto total a cobrar
        public decimal TotalInterestReceivable { get; set; } // Total interés a cobrar
        public decimal PaymentAmount { get; set; } // Monto del pago
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal RemainingBalance { get; set; } // Saldo Restante
        public int ClientId { get; set; }
        public Client Client { get; set; } = default!;
        public string LoanStatusId { get; set; } = default!; // Ejemplo: "Activo", "Pagado", "Vencido", "Cancelado"
        public LoanStatus LoanStatus { get; set; } = default!;
        public int BoxId { get; set; }
        public Box Box { get; set; } = default!;
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}