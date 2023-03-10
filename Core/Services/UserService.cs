using Core.Interfaces.Driven;
using Core.Interfaces.Driving;
using Core.Models.Exceptions;
using Core.Models.User;
using Utils.Language;

namespace Core.Services;

public class UserService : IUserService
{
    private readonly IEnumerable<IUserProvider> _userProviders;
    private readonly IUserStore _userStore;

    public UserService(IEnumerable<IUserProvider> userProviders, IUserStore userStore)
    {
        _userProviders = userProviders;
        _userStore = userStore;
    }

    public async ValueTask<UserModel> GetUser()
    {
        return await GetUserOrNull() ?? throw new UserDoesntExistException();
    }

    public async Task<UserModel?> GetUserOrNull()
    {
        var provider = _userProviders.FirstOrDefault(p => p.HasUser());
        return provider == null ? null : await provider.GetUser();
    }

    public async Task<UserStoredModel> GetStoredUser()
    {
        var user = await GetUser();
        return await _userStore.GetUser(user.Id);
    }

    public async Task UpdateLang(LangEnum lang)
    {
        var user = await GetStoredUser();
        await _userStore.UpdateUser(user with { InterfaceLang = lang });
    }

    public async Task UpdateCurrentSaveId(int id)
    {
        var user = await GetStoredUser();
        await _userStore.UpdateUser(user with { CurrentSaveId = id });
    }
}