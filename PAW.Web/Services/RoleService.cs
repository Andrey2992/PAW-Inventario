using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;
public interface IRoleService
{
    Task<IEnumerable<RoleDTO>> GetRolesAsync();
    Task<RoleDTO?> GetRoleByIdAsync(int id);
    Task<bool> SaveAsync(RoleDTO dto);
    Task<bool> DeleteAsync(int id);
}
public class RoleService : ServiceBase, IRoleService
{
    private const string _path = "Role";
    private readonly IRestProvider _restProvider;
    public RoleService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }
    public async Task<IEnumerable<RoleDTO>> GetRolesAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var roles = await JsonProvider.DeserializeAsync<IEnumerable<RoleDTO>>(response);
        return roles;
    }
    public async Task<RoleDTO?> GetRoleByIdAsync(int id)
    {
        // Truco: metemos el id directo en la URL completa y mandamos id:null a GetAsync.
        // Si no, RestProvider intenta combinar una URL relativa y se rompe (ver LEEME).
        var url = $"{SetPathUrl(_path)}/{id}";
        var response = await _restProvider.GetAsync(url, id: null);
        return await JsonProvider.DeserializeAsync<RoleDTO>(response);
    }
    public async Task<bool> SaveAsync(RoleDTO dto)
    {
        var role = RoleDTO.ConvertTo(dto);
        var content = JsonProvider.Serialize(new[] { role }); // la API espera un IEnumerable<Role>
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