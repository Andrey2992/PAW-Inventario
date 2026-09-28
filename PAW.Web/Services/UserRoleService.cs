using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;
public interface IUserRoleService
{
    Task<IEnumerable<UserRoleDTO>> GetUserRolesAsync();
    Task<UserRoleDTO?> GetUserRoleByIdAsync(int id);
    Task<bool> SaveAsync(UserRoleDTO dto);
    Task<bool> DeleteAsync(int id);
}
public class UserRoleService : ServiceBase, IUserRoleService
{
    private const string _path = "UserRole";
    private readonly IRestProvider _restProvider;
    public UserRoleService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }
    public async Task<IEnumerable<UserRoleDTO>> GetUserRolesAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var userRoles = await JsonProvider.DeserializeAsync<IEnumerable<UserRoleDTO>>(response);
        return userRoles;
    }
    public async Task<UserRoleDTO?> GetUserRoleByIdAsync(int id)
    {
        var url = $"{SetPathUrl(_path)}/{id}";
        var response = await _restProvider.GetAsync(url, id: null);
        return await JsonProvider.DeserializeAsync<UserRoleDTO>(response);
    }
    public async Task<bool> SaveAsync(UserRoleDTO dto)
    {
        var userRole = UserRoleDTO.ConvertTo(dto);
        var content = JsonProvider.Serialize(new[] { userRole });
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