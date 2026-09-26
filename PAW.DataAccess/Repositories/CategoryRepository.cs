using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface ICategoryRepository : IRepositoryBase<Category>
{
    Task<bool> UpsertAsync(Category entity, bool isUpdating);
    Task<bool> CreateAsync(Category entity);
    Task<bool> DeleteAsync(Category entity);
    Task<IEnumerable<Category>> ReadAsync();
    Task<Category> FindAsync(int id);
    Task<bool> UpdateAsync(Category entity);
    Task<bool> UpdateManyAsync(IEnumerable<Category> entities);
    Task<bool> ExistsAsync(Category entity);
}

public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
{
}
