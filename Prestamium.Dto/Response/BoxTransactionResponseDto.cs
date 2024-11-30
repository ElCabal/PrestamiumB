namespace Prestamium.Dto.Response
{
    public class BoxTransactionResponseDto
    {
        public int Id { get; set; }
        public int BoxId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = default!;
        public string Type { get; set; } = default!;
        public DateTime TransactionDate { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal NewBalance { get; set; }
    }
}
