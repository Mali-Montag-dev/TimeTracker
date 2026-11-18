using TimeTracker.DTO;

namespace TimeTracker.BLL.Services
{
    public interface IReportService
    {
        Task<DailyReportDto> GetDailyReportAsync(DateTime date);
    }
}