using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface IUserRoleRepository : IRepositoryBase<UserRole>
{
    Task<bool> UpsertAsync(UserRole entity, bool isUpdating);
    Task<bool> CreateAsync(UserRole entity);
    Task<bool> DeleteAsync(UserRole entity);
    Task<IEnumerable<UserRole>> ReadAsync();
    Task<UserRole> FindAsync(int id);
    Task<bool> UpdateAsync(UserRole entity);
    Task<bool> UpdateManyAsync(IEnumerable<UserRole> entities);
    Task<bool> ExistsAsync(UserRole entity);
    Task<UserRole?> FindByIdAsync(int id);
}
public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
{
    public async Task<UserRole?> FindByIdAsync(int id)
    {
        return await DbContext.UserRoles.FirstOrDefaultAsync(ur => ur.Id == id);
    }
}

