using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAW.Web.Services;

public interface IInventoryService
{
    Task<IEnumerable<InventoryDTO>> GetInventoriesAsync();
    Task<InventoryDTO?> GetInventoryByIdAsync(int id);
    Task<bool> SaveAsync(InventoryDTO dto);
    Task<bool> DeleteAsync(int id);
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

    public async Task<InventoryDTO?> GetInventoryByIdAsync(int id)
    {
        try
        {
            var url = $"{SetPathUrl(_path)}/{id}";
            var response = await _restProvider.GetAsync(url, id: null);
            return await JsonProvider.DeserializeAsync<InventoryDTO>(response);
        }
        catch (ApplicationException)
        {
            // la API responde 404 cuando el inventario no existe
            return null;
        }
    }

    public async Task<bool> SaveAsync(InventoryDTO dto)
    {
        var content = JsonProvider.Serialize(new[] { dto }); // la API espera un IEnumerable<InventoryDTO>
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
