using System.Net.Http;
using System.Net.Http.Json;
using TimeTracker.DTO;

namespace TimeTracker.Desktop.Services
{
    public class ApiClient
    {
        private readonly HttpClient _http = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5173/")
        };

        public async Task ReportActivityAsync(ActivityReportDto report)
        {
            var response = await _http.PostAsJsonAsync("api/activity/report", report);
            response.EnsureSuccessStatusCode();
        }
    }
}