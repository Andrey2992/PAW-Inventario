using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface IProductRepository : IRepositoryBase<Product>
{
    Task<bool> UpsertAsync(Product entity, bool isUpdating);
    Task<bool> CreateAsync(Product entity);
    Task<bool> DeleteAsync(Product entity);
    Task<IEnumerable<Product>> ReadAsync();
    Task<Product> FindAsync(int id);
    Task<bool> UpdateAsync(Product entity);
    Task<bool> UpdateManyAsync(IEnumerable<Product> entities);
    Task<bool> ExistsAsync(Product entity);
}

public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
}

