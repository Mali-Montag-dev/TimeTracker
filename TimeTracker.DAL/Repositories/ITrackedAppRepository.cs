using TimeTracker.DAL.Entities;

namespace TimeTracker.DAL.Repositories
{
    public interface ITrackedAppRepository
    {
        Task<List<TrackedApp>> GetAllAsync();
        Task<TrackedApp?> GetByIdAsync(int id);
        Task<TrackedApp?> GetByProcessNameAsync(string processName);
        Task AddAsync(TrackedApp app);
        Task UpdateAsync(TrackedApp app);
    }
}