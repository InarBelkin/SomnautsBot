using Adapter.TelegramBot.Models;
using Core.Interfaces.Driven;
using Telegram.Bot.Types;

namespace Adapter.TelegramBot.Interfaces;

public interface ITelegramUserProvider : IUserProvider
{
    Task<UserTelegramModel> GetUser();
    void AddUser(User user);
}