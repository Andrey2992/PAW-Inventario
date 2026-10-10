using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDTO>> GetCategoriesAsync();
    Task<CategoryDTO?> GetCategoryByIdAsync(int id);
    Task<bool> SaveAsync(CategoryDTO dto);
    Task<bool> DeleteAsync(int id);
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
    public async Task<CategoryDTO?> GetCategoryByIdAsync(int id)
    {
        try
        {
            var url = $"{SetPathUrl(_path)}/{id}";
            var response = await _restProvider.GetAsync(url, id: null);
            return await JsonProvider.DeserializeAsync<CategoryDTO>(response);
        }
        catch (ApplicationException)
        {
            // la API responde 404 cuando el registro no existe
            return null;
        }
    }

    public async Task<bool> SaveAsync(CategoryDTO dto)
    {
        var content = JsonProvider.Serialize(new[] { dto }); // la API espera un IEnumerable<CategoryDTO>
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
