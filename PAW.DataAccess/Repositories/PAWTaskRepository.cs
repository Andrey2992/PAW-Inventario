using PAW.Models;
using PAW.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PawTask = PAW.Models.Task;

namespace PAW.DataAccess.Repositories;

public interface IPawTaskRepository : IRepositoryBase<PawTask>
{
    Task<bool> UpsertAsync(PawTask entity, bool isUpdating);
    Task<bool> CreateAsync(PawTask entity);
    Task<bool> DeleteAsync(PawTask entity);
    Task<IEnumerable<PawTask>> ReadAsync();
    Task<PawTask> FindAsync(int id);
    Task<bool> UpdateAsync(PawTask entity);
    Task<bool> UpdateManyAsync(IEnumerable<PawTask> entities);
    Task<bool> ExistsAsync(PawTask entity);
}

public class PawTaskRepository : RepositoryBase<PawTask>, IPawTaskRepository
{
}

