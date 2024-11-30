namespace Prestamium.Dto.Request
{
    public class ClientRequestDto
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string DocumentNumber { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string? Address { get; set; }
        public string? Email { get; set; }
    }
}
