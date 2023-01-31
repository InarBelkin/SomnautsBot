using Core.Models.User;
using Utils.Language;

namespace Core.Interfaces.Driving;

public interface IUserService
{
    public Task<UserModel> GetUser();
    Task UpdateLang(LangEnum lang);
}