using Prestamium.Entities;

public class Box : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal InitialBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    public ICollection<BoxTransaction> Transactions { get; set; } = new List<BoxTransaction>();
    public string UserId { get; set; } = default!;
    public User User { get; set; } = default!;
}