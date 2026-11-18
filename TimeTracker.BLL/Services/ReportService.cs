using TimeTracker.DAL.Entities;
using TimeTracker.DAL.Repositories;
using TimeTracker.DTO;

namespace TimeTracker.BLL.Services
{
    public class ReportService : IReportService
    {
        private const string UncategorizedName = "ללא קטגוריה";
        private const string UncategorizedColor = "#9E9E9E";

        private readonly IActivitySessionRepository _sessionRepository;

        public ReportService(IActivitySessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public async Task<DailyReportDto> GetDailyReportAsync(DateTime date)
        {
            var from = date.Date;          // היום בשעה 00:00
            var to = from.AddDays(1);      // מחר בשעה 00:00

            var sessions = await _sessionRepository.GetInRangeAsync(from, to);

            // שלב 1: כמה דקות כל Session תרם ליום הזה
            var items = sessions
                .Select(s => new
                {
                    Session = s,
                    Minutes = MinutesInRange(s, from, to)
                })
                .ToList();

            // שלב 2: קיבוץ לפי תוכנה
            var apps = items
                .GroupBy(x => x.Session.TrackedApp)
                .Select(g => new AppUsageDto
                {
                    AppId = g.Key.Id,
                    DisplayName = g.Key.DisplayName,
                    CategoryName = g.Key.Category?.Name,
                    Minutes = g.Sum(x => x.Minutes)
                })
                .OrderByDescending(a => a.Minutes)
                .ToList();

            // שלב 3: קיבוץ לפי קטגוריה
            var categories = items
                .GroupBy(x => x.Session.TrackedApp.Category)
                .Select(g => new CategoryUsageDto
                {
                    CategoryId = g.Key?.Id,
                    Name = g.Key?.Name ?? UncategorizedName,
                    Color = g.Key?.Color ?? UncategorizedColor,
                    IsProductive = g.Key?.IsProductive ?? false,
                    Minutes = g.Sum(x => x.Minutes)
                })
                .OrderByDescending(c => c.Minutes)
                .ToList();

            // שלב 4: הרכבת הדוח
            return new DailyReportDto
            {
                Date = from,
                TotalMinutes = apps.Sum(a => a.Minutes),
                ProductiveMinutes = categories.Where(c => c.IsProductive).Sum(c => c.Minutes),
                Apps = apps,
                Categories = categories
            };
        }

        // כמה דקות מתוך ה-Session נפלו בתוך הטווח
        private static double MinutesInRange(ActivitySession session, DateTime from, DateTime to)
        {
            var start = session.StartTime < from ? from : session.StartTime;
            var end = session.EndTime > to ? to : session.EndTime;
            return (end - start).TotalMinutes;
        }
    }
}