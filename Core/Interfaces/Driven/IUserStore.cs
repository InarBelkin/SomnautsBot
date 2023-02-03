using Core.Models.User;

namespace Core.Interfaces.Driven;

public interface IUserStore
{
    Task UpdateUser(UserStoredModel user);
    Task<UserStoredModel> GetUser(int id);
}