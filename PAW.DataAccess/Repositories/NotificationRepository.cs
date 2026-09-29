using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface INotificationRepository : IRepositoryBase<Notification>
{
    Task<bool> UpsertAsync(Notification entity, bool isUpdating);
    Task<bool> CreateAsync(Notification entity);
    Task<bool> DeleteAsync(Notification entity);
    Task<IEnumerable<Notification>> ReadAsync();
    Task<Notification> FindAsync(int id);
    Task<bool> UpdateAsync(Notification entity);
    Task<bool> UpdateManyAsync(IEnumerable<Notification> entities);
    Task<bool> ExistsAsync(Notification entity);
}

public class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
{
}
