using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface ISupplierService
{
    Task<SupplierDTO?> GetSupplierByIdAsync(int id);
    Task<IEnumerable<SupplierDTO>> GetSuppliersAsync();
    Task<bool> SaveAsync(SupplierDTO supplier);
    Task<bool> DeleteAsync(int id);
}public class SupplierService : ServiceBase, ISupplierService
{
    private const string _path = "Supplier";
    private readonly IRestProvider _restProvider;

    public SupplierService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<SupplierDTO>> GetSuppliersAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var items = await JsonProvider.DeserializeAsync<IEnumerable<SupplierDTO>>(response);
        return items;
    }

    public async Task<SupplierDTO?> GetSupplierByIdAsync(int id)
    {
        try
        {
            var url = $"{SetPathUrl(_path)}/{id}";
            var response = await _restProvider.GetAsync(url, id: null);
            return await JsonProvider.DeserializeAsync<SupplierDTO>(response);
        }
        catch (ApplicationException)
        {
            // la API responde 404 cuando el registro no existe
            return null;
        }
    }

    public async Task<bool> SaveAsync(SupplierDTO dto)
    {
        var content = JsonProvider.Serialize(new[] { dto }); // la API espera un IEnumerable<SupplierDTO>
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
