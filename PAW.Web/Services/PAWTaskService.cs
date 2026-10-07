using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IPawTaskService
{
    Task<IEnumerable<PawTaskDTO>> GetPawTasksAsync();
    Task<PawTaskDTO?> GetPawTaskByIdAsync(int id);
    Task<bool> SaveAsync(PawTaskDTO dto);
    Task<bool> DeleteAsync(int id);
}

public class PawTaskService : ServiceBase, IPawTaskService
{
    private const string _path = "PawTask";
    private readonly IRestProvider _restProvider;

    public PawTaskService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<PawTaskDTO>> GetPawTasksAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var items = await JsonProvider.DeserializeAsync<IEnumerable<PawTaskDTO>>(response);
        return items;
    }

    public async Task<PawTaskDTO?> GetPawTaskByIdAsync(int id)
    {
        // Igual que RoleService: el id va dentro de la URL completa y mandamos id:null.
        var url = $"{SetPathUrl(_path)}/{id}";
        try
        {
            var response = await _restProvider.GetAsync(url, id: null);
            return await JsonProvider.DeserializeAsync<PawTaskDTO>(response);
        }
        catch (ApplicationException)
        {
            // El API responde 404 cuando el registro no existe.
            return null;
        }
    }

    public async Task<bool> SaveAsync(PawTaskDTO dto)
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
