using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Prestamium.Persistence;
using Prestamium.Repositories.Interfaces;

public class BoxRepository : BaseRepository<Box>, IBoxRepository
{
    public BoxRepository(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor)
        : base(context, httpContextAccessor)
    {

    }

    public async Task<Box?> GetBoxWithTransactionsAsync(int id)
    {
        var userId = GetCurrentUserId();
        return await context.Set<Box>()
            .Include(b => b.Transactions)
            .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
    }

    public async Task<Box?> GetBoxWithDetailsAsync(int id)
    {
        var userId = GetCurrentUserId();
        return await context.Set<Box>()
            .Include(b => b.Transactions)
            .Include(b => b.Loans)
            .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
    }
}