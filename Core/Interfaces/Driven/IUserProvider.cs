using Core.Models.User;

namespace Core.Interfaces.Driven;

public interface IUserProvider
{
    public Task<UserModel> GetUser();
    public bool HasUser();
}