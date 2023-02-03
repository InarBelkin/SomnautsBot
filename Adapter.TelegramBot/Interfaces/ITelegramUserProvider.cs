using Adapter.TelegramBot.Models;
using Core.Interfaces.Driven;
using Telegram.Bot.Types;

namespace Adapter.TelegramBot.Interfaces;

public interface ITelegramUserProvider : IUserProvider
{
    new ValueTask<UserTelegramModel> GetUser();
    void AddUser(User user);
}