using Core.Models.Exceptions;
using Core.Models.User;
using Utils.Language;

namespace Core.Interfaces.Driving;

public interface IUserService
{
    /// <exception cref="UserDoesntExistException">Throws if all user's providers have no users</exception>
    public ValueTask<UserModel> GetUser();

    Task<UserModel?> GetUserOrNull();
    Task UpdateLang(LangEnum lang);
}