using Prestamium.Dto.Response;
using System.Threading.Tasks;

namespace Prestamium.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<BaseResponseGeneric<DashboardSummaryDto>> GetDashboardSummaryAsync();
    }
}
