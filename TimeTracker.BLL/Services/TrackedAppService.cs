using TimeTracker.DAL.Entities;
using TimeTracker.DAL.Repositories;
using TimeTracker.DTO;

namespace TimeTracker.BLL.Services
{
    public class TrackedAppService : ITrackedAppService
    {
        private readonly ITrackedAppRepository _appRepository;
        private readonly ICategoryRepository _categoryRepository;

        public TrackedAppService(
            ITrackedAppRepository appRepository,
            ICategoryRepository categoryRepository)
        {
            _appRepository = appRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<TrackedAppDto>> GetAllAsync()
        {
            var apps = await _appRepository.GetAllAsync();
            return apps.Select(ToDto).ToList();
        }

        public async Task<bool> AssignCategoryAsync(int appId, int? categoryId)
        {
            var app = await _appRepository.GetByIdAsync(appId);
            if (app == null)
                return false;

            Category? category = null;
            if (categoryId != null)
            {
                category = await _categoryRepository.GetByIdAsync(categoryId.Value);
                if (category == null)
                    throw new ArgumentException("הקטגוריה שנבחרה לא קיימת");
            }

            app.Category = category;
            app.CategoryId = categoryId;
            await _appRepository.UpdateAsync(app);
            return true;
        }

        private static TrackedAppDto ToDto(TrackedApp app)
        {
            return new TrackedAppDto
            {
                Id = app.Id,
                ProcessName = app.ProcessName,
                DisplayName = app.DisplayName,
                CategoryId = app.CategoryId,
                CategoryName = app.Category?.Name
            };
        }
    }
}