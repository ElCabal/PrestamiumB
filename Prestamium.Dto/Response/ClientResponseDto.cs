namespace Prestamium.Dto.Response
{
    public class ClientResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string DocumentNumber { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string? Address { get; set; }
        public string? Email { get; set; }
        public bool Status { get; set; }
    }
}
