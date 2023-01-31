using Core.Interfaces.Driven;
using Core.Interfaces.Driving;
using Core.Models.User;

namespace Core.Services;

public class UserService : IUserService
{
    private readonly IEnumerable<IUserProvider> _userProviders;

    public UserService(IEnumerable<IUserProvider> userProviders)
    {
        _userProviders = userProviders;
    }

    public async Task<UserModel> GetUser()
    {
        var provider = _userProviders.FirstOrDefault(p => p.HasUser());
        if (provider == null) throw new ArgumentException("User hasn't been added");
        return await provider.GetUser();
    }
}