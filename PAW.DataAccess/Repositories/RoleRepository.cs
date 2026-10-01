using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;
public interface IRoleRepository : IRepositoryBase<Role>
{
    Task<bool> UpsertAsync(Role entity, bool isUpdating);
    Task<bool> CreateAsync(Role entity);
    Task<bool> DeleteAsync(Role entity);
    Task<IEnumerable<Role>> ReadAsync();
    Task<Role> FindAsync(int id);
    Task<bool> UpdateAsync(Role entity);
    Task<bool> UpdateManyAsync(IEnumerable<Role> entities);
    Task<bool> ExistsAsync(Role entity);
}

public class RoleRepository : RepositoryBase<Role>, IRoleRepository
{
}
