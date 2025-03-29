namespace Prestamium.Dto.Request
{
    public class LoanFilterRequestDto
    {
        // Paginación
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        
        // Filtros
        public int? ClientId { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public DateTime? StartDateFrom { get; set; }
        public DateTime? StartDateTo { get; set; }
        public string? Frequency { get; set; }
        public bool? IsPaid { get; set; } // Préstamo completamente pagado (RemainingBalance = 0)
    }
}
