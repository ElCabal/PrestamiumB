namespace Prestamium.Dto.Request
{
    public class BoxRequestDto
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal InitialBalance { get; set; }
    }
}
