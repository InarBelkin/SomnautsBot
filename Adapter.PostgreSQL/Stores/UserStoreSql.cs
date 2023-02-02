using Core.Interfaces.Driven;
using Core.Models.Exceptions;
using Core.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Adapter.PostgreSQL.Stores;

public class UserStoreSql : IUserStore
{
    private readonly SomnContext _db;

    public UserStoreSql(SomnContext db)
    {
        _db = db;
    }

    public async Task UpdateUser(UserStoredModel user)
    {
        await _db.Users.Where(u => u.Id == user.Id).ExecuteUpdateAsync(p => p
            .SetProperty(u => u.UserName, user.UserName)
            .SetProperty(u => u.InterfaceLang, user.InterfaceLang)
            .SetProperty(u => u.CurrentSaveId, user.CurrentSaveId));
    }

    public async Task<UserStoredModel> GetUser(int id)
    {
        return await _db.Users.Where(u => u.Id == id)
            .Select(u => new UserStoredModel(u.Id, u.UserName, u.InterfaceLang, u.CurrentSaveId))
            .FirstOrDefaultAsync() ?? throw new UserDoesntExistException();
    }
}