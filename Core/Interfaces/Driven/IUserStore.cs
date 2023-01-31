using Core.Models.User;

namespace Core.Interfaces.Driven;

public interface IUserStore
{
    Task UpdateUser(UserModel user);
}