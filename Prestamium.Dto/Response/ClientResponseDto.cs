namespace Prestamium.Dto.Response
{
    public class ClientResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public int CellPhoneNumber { get; set; }
        public string Email { get; set; } = default!;
        public bool Status { get; set; } = true;
    }
}
