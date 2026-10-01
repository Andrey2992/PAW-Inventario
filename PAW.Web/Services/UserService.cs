using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;
public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetUsersAsync();
    Task<UserDTO?> GetUserByIdAsync(int id);
    Task<bool> SaveAsync(UserDTO dto);
    Task<bool> DeleteAsync(int id);
}
public class UserService : ServiceBase, IUserService
{
    private const string _path = "User";
    private readonly IRestProvider _restProvider;
    public UserService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }
    public async Task<IEnumerable<UserDTO>> GetUsersAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var users = await JsonProvider.DeserializeAsync<IEnumerable<UserDTO>>(response);
        return users;
    }
    public async Task<UserDTO?> GetUserByIdAsync(int id)
    {
        var url = $"{SetPathUrl(_path)}/{id}";
        var response = await _restProvider.GetAsync(url, id: null);
        return await JsonProvider.DeserializeAsync<UserDTO>(response);
    }
    public async Task<bool> SaveAsync(UserDTO dto)
    {
        var user = UserDTO.ConvertTo(dto);
        var content = JsonProvider.Serialize(new[] { user });
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