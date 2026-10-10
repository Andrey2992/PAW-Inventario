using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IComponentService
{
    Task<ComponentDTO?> GetComponentByIdAsync(int id);
    Task<IEnumerable<ComponentDTO>> GetComponentsAsync();
    Task<bool> SaveAsync(ComponentDTO dto);
    Task<bool> DeleteAsync(int id);
}

public class ComponentService : ServiceBase, IComponentService
{
    private const string _path = "Component";
    private readonly IRestProvider _restProvider;

    public ComponentService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<ComponentDTO>> GetComponentsAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var items = await JsonProvider.DeserializeAsync<IEnumerable<ComponentDTO>>(response);
        return items;
    }

    public async Task<ComponentDTO?> GetComponentByIdAsync(int id)
    {
        try
        {
            var url = $"{SetPathUrl(_path)}/{id}";
            var response = await _restProvider.GetAsync(url, id: null);
            return await JsonProvider.DeserializeAsync<ComponentDTO>(response);
        }
        catch (ApplicationException)
        {
            // la API responde 404 cuando el registro no existe
            return null;
        }
    }

    public async Task<bool> SaveAsync(ComponentDTO dto)
    {
        var content = JsonProvider.Serialize(new[] { dto }); // la API espera un IEnumerable<ComponentDTO>
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
