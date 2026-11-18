using Microsoft.EntityFrameworkCore;
using TimeTracker.DAL.Entities;

namespace TimeTracker.DAL.Repositories
{
    public class TrackedAppRepository : ITrackedAppRepository
    {
        private readonly AppDbContext _context;

        public TrackedAppRepository(AppDbContext context)
        {
            _context = context;
        }

        // ✅ כתבתי בשבילך, כדי שתראי את ה-Include
        public async Task<List<TrackedApp>> GetAllAsync()
        {
            return await _context.TrackedApps
                .Include(a => a.Category)
                .OrderBy(a => a.DisplayName)
                .ToListAsync();
        }

        // ✏️ 1
        public async Task<TrackedApp?> GetByIdAsync(int id)
        {
            return await _context.TrackedApps
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        // ✏️ 2
        public async Task<TrackedApp?> GetByProcessNameAsync(string processName)
        {
            return await _context.TrackedApps
                .FirstOrDefaultAsync(a => a.ProcessName == processName);
        }

        // ✏️ 3
        public async Task AddAsync(TrackedApp app)
        {
            _context.TrackedApps.Add(app);
            await _context.SaveChangesAsync();
        }

        // ✏️ 4
        public async Task UpdateAsync(TrackedApp app)
        {
            _context.TrackedApps.Update(app);

            await _context.SaveChangesAsync();
        }
    }
}