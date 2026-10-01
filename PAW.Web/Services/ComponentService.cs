using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IComponentService
{
    Task<IEnumerable<ComponentDTO>> GetComponentsAsync();
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
}
