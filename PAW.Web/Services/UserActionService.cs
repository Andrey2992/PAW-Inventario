using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IUserActionService
{
    Task<IEnumerable<UserActionDTO>> GetUserActionsAsync();
}

public class UserActionService : ServiceBase, IUserActionService
{
    private const string _path = "UserAction";
    private readonly IRestProvider _restProvider;

    public UserActionService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<UserActionDTO>> GetUserActionsAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var userActions = await JsonProvider.DeserializeAsync<IEnumerable<UserActionDTO>>(response);
        return userActions;
    }
}
