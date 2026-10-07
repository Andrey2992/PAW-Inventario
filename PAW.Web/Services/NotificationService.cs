using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationDTO>> GetNotificationsAsync();
    Task<NotificationDTO?> GetNotificationByIdAsync(int id);
    Task<bool> SaveAsync(NotificationDTO dto);
    Task<bool> DeleteAsync(int id);
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
        var items = await JsonProvider.DeserializeAsync<IEnumerable<NotificationDTO>>(response);
        return items;
    }

    public async Task<NotificationDTO?> GetNotificationByIdAsync(int id)
    {
        // Igual que RoleService: el id va dentro de la URL completa y mandamos id:null.
        var url = $"{SetPathUrl(_path)}/{id}";
        try
        {
            var response = await _restProvider.GetAsync(url, id: null);
            return await JsonProvider.DeserializeAsync<NotificationDTO>(response);
        }
        catch (ApplicationException)
        {
            // El API responde 404 cuando el registro no existe.
            return null;
        }
    }

    public async Task<bool> SaveAsync(NotificationDTO dto)
    {
        var content = JsonProvider.Serialize(new[] { dto }); // la API espera una lista
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return response.Trim().Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var url = $"{SetPathUrl(_path)}/{id}";
        var response = await _restProvider.DeleteAsync(url, id: string.Empty);
        return response.Trim().Equals("true", StringComparison.OrdinalIgnoreCase);
    }
}
