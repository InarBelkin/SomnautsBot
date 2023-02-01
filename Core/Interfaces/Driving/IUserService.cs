using Core.Models.User;
using Utils.Language;

namespace Core.Interfaces.Driving;

public interface IUserService
{
    /// <exception cref="Core.Models.Exceptions.UserDoesntExistsException">Throws if all user's providers have no users</exception>
    public Task<UserModel> GetUser();

    Task<UserModel?> GetUserOrNull();
    Task UpdateLang(LangEnum lang);
}