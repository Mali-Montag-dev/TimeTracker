using TimeTracker.DAL.Entities;

namespace TimeTracker.DAL.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<bool> NameExistsAsync(string name);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Category category);
    }
}