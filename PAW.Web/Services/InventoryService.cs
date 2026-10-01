using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAW.Web.Services;

public interface IInventoryService
{
    Task<IEnumerable<InventoryDTO>> GetInventoriesAsync();
}

public class InventoryService : ServiceBase, IInventoryService
{
    private const string _path = "Inventory";
    private readonly IRestProvider _restProvider;

    public InventoryService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<InventoryDTO>> GetInventoriesAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var items = await JsonProvider.DeserializeAsync<IEnumerable<InventoryDTO>>(response);
        return items;
    }
}

