namespace Prestamium.Dto.Request
{
    public class BoxTransactionRequestDto
    {
        public int BoxId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = default!;
        public string Type { get; set; } = default!;
    }
}
