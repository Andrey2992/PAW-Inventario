using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface ISupplierService
{
    Task<IEnumerable<SupplierDTO>> GetSuppliersAsync();
}

public class SupplierService : ServiceBase, ISupplierService
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
}
