using Adapter.TelegramBot.Models;
using Telegram.Bot.Types;

namespace Adapter.TelegramBot.Interfaces;

public interface ITelegramUserProvider
{
    Task<UserTelegramModel> GetUser();
    void AddUser(User user);
}