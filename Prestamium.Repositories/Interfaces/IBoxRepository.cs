namespace Prestamium.Repositories.Interfaces
{
    public interface IBoxRepository : IBaseRepository<Box>
    {
        Task<Box?> GetBoxWithTransactionsAsync(int id);
        Task<Box?> GetBoxWithDetailsAsync(int id);
    }
}
