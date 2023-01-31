using Core.Interfaces.Driven;
using Core.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Adapter.PostgreSQL.Services;

public class UserStoreSql : IUserStore
{
    private readonly SomnContext _db;

    public UserStoreSql(SomnContext db)
    {
        _db = db;
    }

    public async Task UpdateUser(UserModel user)
    {
        var dbUser = await _db.Users.SingleAsync(u => u.Id == user.Id);
        dbUser.UserName = user.UserName;
        dbUser.InterfaceLang = user.InterfaceLang;
        await _db.SaveChangesAsync();
    }
}