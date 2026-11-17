using TimeTracker.DAL.Entities;
using TimeTracker.DAL.Repositories;
using TimeTracker.DTO;

namespace TimeTracker.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        // ===== שליפה =====

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();
            return categories.Select(ToDto).ToList();
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            return category == null ? null : ToDto(category);
        }

        // ===== יצירה =====

        public async Task<CategoryDto> CreateAsync(CategoryDto dto)
        {
            var name = dto.Name.Trim();

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("שם הקטגוריה לא יכול להיות ריק");

            if (await _repository.NameExistsAsync(name))
                throw new ArgumentException($"כבר קיימת קטגוריה בשם '{name}'");

            var category = new Category
            {
                Name = name,
                Color = dto.Color,
                IsProductive = dto.IsProductive
            };

            await _repository.AddAsync(category);
            return ToDto(category);
        }

        // ===== עדכון =====

        public async Task<bool> UpdateAsync(int id, CategoryDto dto)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return false;

            var name = dto.Name.Trim();

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("שם הקטגוריה לא יכול להיות ריק");

            if (name != category.Name && await _repository.NameExistsAsync(name))
                throw new ArgumentException($"כבר קיימת קטגוריה בשם '{name}'");

            category.Name = name;
            category.Color = dto.Color;
            category.IsProductive = dto.IsProductive;

            await _repository.UpdateAsync(category);
            return true;
        }

        // ===== מחיקה =====

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return false;

            await _repository.DeleteAsync(category);
            return true;
        }

        // ===== המרה =====

        private static CategoryDto ToDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Color = category.Color,
                IsProductive = category.IsProductive
            };
        }
    }
}