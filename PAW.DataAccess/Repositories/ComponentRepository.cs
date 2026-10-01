using Microsoft.EntityFrameworkCore;
using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface IComponentRepository : IRepositoryBase<Component>
{
    Task<bool> UpsertAsync(Component entity, bool isUpdating);
    Task<bool> CreateAsync(Component entity);
    Task<bool> DeleteAsync(Component entity);
    Task<IEnumerable<Component>> ReadAsync();
    Task<Component> FindAsync(int id);
    Task<bool> UpdateAsync(Component entity);
    Task<bool> UpdateManyAsync(IEnumerable<Component> entities);
    Task<bool> ExistsAsync(Component entity);
}

public class ComponentRepository : RepositoryBase<Component>, IComponentRepository
{
    // Component.Id is decimal, so the base FindAsync(int) cannot be used (EF expects a decimal key)
    public new async Task<Component> FindAsync(int id)
    {
        return await DbContext.Components.FirstOrDefaultAsync(c => c.Id == id);
    }
}
