using Microsoft.EntityFrameworkCore;
using TimeTracker.DAL.Entities;

namespace TimeTracker.DAL.Repositories
{
    public class ActivitySessionRepository : IActivitySessionRepository
    {
        private readonly AppDbContext _context;

        public ActivitySessionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ActivitySession?> GetLastAsync()
        {
            return await _context.ActivitySessions
                .OrderByDescending(s => s.EndTime)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ActivitySession>> GetInRangeAsync(DateTime from, DateTime to)
        {
            return await _context.ActivitySessions
                .Include(s => s.TrackedApp)
                    .ThenInclude(a => a.Category)
                .Where(s => s.EndTime > from && s.StartTime < to)
                .ToListAsync();
        }

        // ✏️ 1
        public async Task AddAsync(ActivitySession session)
        {
            _context.ActivitySessions.Add(session);
            await _context.SaveChangesAsync();
        }

        // ✏️ 2
        public async Task UpdateAsync(ActivitySession session)
        {
            _context.ActivitySessions.Update(session);

            await _context.SaveChangesAsync();
        }
    }
}