using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationDTO>> GetNotificationsAsync();
}

public class NotificationService : ServiceBase, INotificationService
{
    private const string _path = "Notification";
    private readonly IRestProvider _restProvider;

    public NotificationService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<NotificationDTO>> GetNotificationsAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var notifications = await JsonProvider.DeserializeAsync<IEnumerable<NotificationDTO>>(response);
        return notifications;
    }
}
