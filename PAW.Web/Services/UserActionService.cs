using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IUserActionService
{
    Task<IEnumerable<UserActionDTO>> GetUserActionsAsync();
    Task<UserActionDTO?> GetUserActionByIdAsync(int id);
    Task<bool> SaveAsync(UserActionDTO dto);
    Task<bool> DeleteAsync(int id);
}

public class UserActionService : ServiceBase, IUserActionService
{
    private const string _path = "UserAction";
    private readonly IRestProvider _restProvider;

    public UserActionService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<UserActionDTO>> GetUserActionsAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var items = await JsonProvider.DeserializeAsync<IEnumerable<UserActionDTO>>(response);
        return items;
    }

    public async Task<UserActionDTO?> GetUserActionByIdAsync(int id)
    {
        // Igual que RoleService: el id va dentro de la URL completa y mandamos id:null.
        var url = $"{SetPathUrl(_path)}/{id}";
        try
        {
            var response = await _restProvider.GetAsync(url, id: null);
            return await JsonProvider.DeserializeAsync<UserActionDTO>(response);
        }
        catch (ApplicationException)
        {
            // El API responde 404 cuando el registro no existe.
            return null;
        }
    }

    public async Task<bool> SaveAsync(UserActionDTO dto)
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
