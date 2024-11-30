namespace Prestamium.Entities
{
    public class BoxTransaction : BaseEntity
    {
        public int BoxId { get; set; }
        public Box Box { get; set; } = default!;
        public decimal Amount { get; set; }
        public string Description { get; set; } = default!;
        public string Type { get; set; } = default!; // "income" o "expense"
        public DateTime TransactionDate { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal NewBalance { get; set; }
    }
}
