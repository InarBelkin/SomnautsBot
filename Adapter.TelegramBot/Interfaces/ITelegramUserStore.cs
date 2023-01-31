using Adapter.TelegramBot.Models;
using Telegram.Bot.Types;

namespace Adapter.TelegramBot.Interfaces;

public interface ITelegramUserStore
{
    public Task<UserTelegramModel> GetOrCreateUser(User tgUser);
}