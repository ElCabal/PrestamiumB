namespace Prestamium.Entities
{
    public class Box : BaseEntity
    {
        public string Name { get; set; } = default!;
        public decimal InitialAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();

    }
}
