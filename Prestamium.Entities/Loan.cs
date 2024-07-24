namespace Prestamium.Entities
{
    public class Loan : BaseEntity
    {
        public decimal Amount { get; set; } // Monto a Prestar
        public decimal TotalAmountDue { get; set; } //Monto Total a Cobrar
        public int Fees { get; set; } //Cuotas
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal InterestRate { get; set; } //Tasa de Interes
        public string Frecuency { get; set; } = default!; // Diario, Semanal, Mensual
        public int ClientId { get; set; }
        public Client Client { get; set; } = default!;
        public string LoanStatusId { get; set; } = default!; // Ejemplo: "Activo", "Pagado", "Vencido", "Cancelado"
        public LoanStatus LoanStatus { get; set; } = default!;
        public int BoxId { get; set; }
        public Box Box { get; set; } = default!;
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}