using Core.Models.User;

namespace Core.Interfaces.Driven;

public interface IUserProvider
{
    public ValueTask<UserModel> GetUser();
    public bool HasUser();
}