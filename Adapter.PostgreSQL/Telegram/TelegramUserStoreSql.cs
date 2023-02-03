using Adapter.TelegramBot.Interfaces;
using Adapter.TelegramBot.Models;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Utils.Language;
using DbUser = Adapter.PostgreSQL.Entities.User;

namespace Adapter.PostgreSQL.Telegram;

public class TelegramUserStoreSql : ITelegramUserStore
{
    private readonly SomnContext _db;

    public TelegramUserStoreSql(SomnContext db)
    {
        _db = db;
    }

    public async Task<UserTelegramModel> GetOrCreateUser(User tgUser)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.TelegramId == tgUser.Id);
        if (user == null)
        {
            user = _db.Users.Add(new DbUser
            {
                UserName = tgUser.Username ?? tgUser.FirstName,
                InterfaceLang = LangEnum.DefineLanguageOrEng(tgUser.LanguageCode),
                TelegramId = tgUser.Id
            }).Entity;
            await _db.SaveChangesAsync();
        }

        var userModel = new UserTelegramModel(user.Id, user.UserName, user.InterfaceLang, user.TelegramId!.Value);
        return userModel;
    }
}