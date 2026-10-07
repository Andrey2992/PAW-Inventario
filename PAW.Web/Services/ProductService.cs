using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetProductsAsync();
    Task<ProductDTO?> GetProductByIdAsync(int id);
    Task<bool> SaveAsync(ProductDTO dto);
    Task<bool> DeleteAsync(int id);
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

    public async Task<ProductDTO?> GetProductByIdAsync(int id)
    {
        try
        {
            var url = $"{SetPathUrl(_path)}/{id}";
            var response = await _restProvider.GetAsync(url, id: null);
            return await JsonProvider.DeserializeAsync<ProductDTO>(response);
        }
        catch (ApplicationException)
        {
            // la API responde 404 cuando el producto no existe
            return null;
        }
    }

    public async Task<bool> SaveAsync(ProductDTO dto)
    {
        var content = JsonProvider.Serialize(new[] { dto }); // la API espera un IEnumerable<ProductDTO>
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
