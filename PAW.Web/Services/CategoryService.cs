using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDTO>> GetCategoriesAsync();
}

public class CategoryService : ServiceBase, ICategoryService
{
    private const string _path = "Category";
    private readonly IRestProvider _restProvider;

    public CategoryService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var items = await JsonProvider.DeserializeAsync<IEnumerable<CategoryDTO>>(response);
        return items;
    }
}
