using TimeTracker.DTO;

namespace TimeTracker.BLL.Services
{
    public interface ITrackedAppService
    {
        Task<List<TrackedAppDto>> GetAllAsync();
        Task<bool> AssignCategoryAsync(int appId, int? categoryId);
    }
}