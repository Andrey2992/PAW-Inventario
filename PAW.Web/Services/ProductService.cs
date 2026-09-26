using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetProductsAsync();
}

public class ProductService : ServiceBase, IProductService
{
    private const string _path = "Product";
    private readonly IRestProvider _restProvider;

    public ProductService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var products = await JsonProvider.DeserializeAsync<IEnumerable<ProductDTO>>(response);
        return products;
    }
}
