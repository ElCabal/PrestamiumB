namespace Prestamium.Dto.Response
{
    public class BoxResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal InitialBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public bool IsActive { get; set; }
    }

    public class BoxDetailResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal InitialBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public ICollection<BoxTransactionResponseDto> Transactions { get; set; } = new List<BoxTransactionResponseDto>();
        public ICollection<LoanSimpleResponseDto> Loans { get; set; } = new List<LoanSimpleResponseDto>();
    }
}
