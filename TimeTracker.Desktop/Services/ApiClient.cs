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

        // ===== מעקב =====

        public async Task ReportActivityAsync(ActivityReportDto report)
        {
            var response = await _http.PostAsJsonAsync("api/activity/report", report);
            response.EnsureSuccessStatusCode();
        }

        // ===== דוחות =====

        public async Task<DailyReportDto> GetDailyReportAsync(DateTime date)
        {
            var report = await _http.GetFromJsonAsync<DailyReportDto>(
                $"api/reports/daily?date={date:yyyy-MM-dd}");
            return report ?? new DailyReportDto();
        }

        // ===== קטגוריות =====

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            var list = await _http.GetFromJsonAsync<List<CategoryDto>>("api/categories");
            return list ?? new List<CategoryDto>();
        }

        public async Task CreateCategoryAsync(CategoryDto category)
        {
            var response = await _http.PostAsJsonAsync("api/categories", category);
            await EnsureSuccessAsync(response);
        }

        public async Task UpdateCategoryAsync(CategoryDto category)
        {
            var response = await _http.PutAsJsonAsync($"api/categories/{category.Id}", category);
            await EnsureSuccessAsync(response);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/categories/{id}");
            await EnsureSuccessAsync(response);
        }
        // ===== תוכנות =====

        public async Task<List<TrackedAppDto>> GetAppsAsync()
        {
            var list = await _http.GetFromJsonAsync<List<TrackedAppDto>>("api/apps");
            return list ?? new List<TrackedAppDto>();
        }

        public async Task AssignCategoryAsync(int appId, int? categoryId)
        {
            var dto = new AssignCategoryDto { CategoryId = categoryId };
            var response = await _http.PutAsJsonAsync($"api/apps/{appId}/category", dto);
            await EnsureSuccessAsync(response);
        }
        // ===== עזר =====

        private static async Task EnsureSuccessAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            var message = await response.Content.ReadAsStringAsync();
            throw new ApiException(string.IsNullOrWhiteSpace(message)
                ? $"שגיאה {(int)response.StatusCode}"
                : message);
        }
    }

    public class ApiException : Exception
    {
        public ApiException(string message) : base(message)
        {
        }
    }
}