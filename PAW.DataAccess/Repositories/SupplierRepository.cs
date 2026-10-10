using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface ISupplierRepository : IRepositoryBase<Supplier>
{
    Task<bool> UpsertAsync(Supplier entity, bool isUpdating);
    Task<bool> CreateAsync(Supplier entity);
    Task<bool> DeleteAsync(Supplier entity);
    Task<IEnumerable<Supplier>> ReadAsync();
    Task<Supplier> FindAsync(int id);
    Task<bool> UpdateAsync(Supplier entity);
    Task<bool> UpdateManyAsync(IEnumerable<Supplier> entities);
    Task<bool> ExistsAsync(Supplier entity);
}

public class SupplierRepository : RepositoryBase<Supplier>, ISupplierRepository
{
}
