using TimeTracker.DAL.Entities;
using TimeTracker.DAL.Repositories;
using TimeTracker.DTO;

namespace TimeTracker.BLL.Services
{
    public class ActivityService : IActivityService
    {
        private static readonly TimeSpan MaxGap = TimeSpan.FromSeconds(30);

        private readonly ITrackedAppRepository _appRepository;
        private readonly IActivitySessionRepository _sessionRepository;

        public ActivityService(
            ITrackedAppRepository appRepository,
            IActivitySessionRepository sessionRepository)
        {
            _appRepository = appRepository;
            _sessionRepository = sessionRepository;
        }

        public async Task ReportActivityAsync(ActivityReportDto report)
        {
            // ולידציה
            var processName = report.ProcessName.Trim().ToLower();
            if (string.IsNullOrEmpty(processName))
                throw new ArgumentException("שם התהליך לא יכול להיות ריק");

            var now = DateTime.Now;

            // שלב 1: למצוא את התוכנה, או ליצור אותה אם היא חדשה
            var app = await _appRepository.GetByProcessNameAsync(processName);
            if (app == null)
            {
                app = new TrackedApp
                {
                    ProcessName = processName,
                    DisplayName = string.IsNullOrWhiteSpace(report.DisplayName)
                        ? processName
                        : report.DisplayName.Trim()
                };
                await _appRepository.AddAsync(app);
            }

            // שלב 2: ה-Session האחרון
            var last = await _sessionRepository.GetLastAsync();

            // שלב 3: אפשר להאריך?
            bool canExtend = last != null
                && last.TrackedAppId == app.Id
                && now - last.EndTime <= MaxGap;

            if (canExtend)
            {
                last!.EndTime = now;
                await _sessionRepository.UpdateAsync(last);
            }
            else
            {
                var session = new ActivitySession
                {
                    TrackedAppId = app.Id,
                    WindowTitle = report.WindowTitle,
                    StartTime = now,
                    EndTime = now
                };
                await _sessionRepository.AddAsync(session);
            }
        }
    }
}