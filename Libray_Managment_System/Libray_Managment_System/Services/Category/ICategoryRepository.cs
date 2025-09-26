using System.Threading.Tasks;

namespace Libray_Managment_System.Services.Category;

public interface ICategoryRepository
{
    Task AddAsync(Models.Category entity);
    Task<IEnumerable<Models.Category>> GetAllAsync();
    Task<Models.Category> GetByIdAsync(int id);
    Task UpdateAsync(Models.Category entity);
    Task DeleteAsync(Models.Category entity);
}