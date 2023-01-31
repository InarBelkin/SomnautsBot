using Core.Models.User;

namespace Core.Interfaces.Driving;

public interface IUserService
{
    public Task<UserModel> GetUser();
}