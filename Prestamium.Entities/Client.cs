namespace Prestamium.Entities
{
    public class Client : BaseEntity
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string DocumentNumber { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string? Address { get; set; }
        public string? Email { get; set; }

        // Navegación
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}
