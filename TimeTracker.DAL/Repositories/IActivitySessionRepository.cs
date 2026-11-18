using TimeTracker.DAL.Entities;

namespace TimeTracker.DAL.Repositories
{
    public interface IActivitySessionRepository
    {
        Task<ActivitySession?> GetLastAsync();
        Task<List<ActivitySession>> GetInRangeAsync(DateTime from, DateTime to);
        Task AddAsync(ActivitySession session);
        Task UpdateAsync(ActivitySession session);
    }
}