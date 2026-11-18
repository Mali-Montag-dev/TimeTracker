using TimeTracker.DTO;

namespace TimeTracker.BLL.Services
{
    public interface IActivityService
    {
        Task ReportActivityAsync(ActivityReportDto report);
    }
}