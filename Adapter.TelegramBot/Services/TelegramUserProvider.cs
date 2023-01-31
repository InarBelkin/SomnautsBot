using Adapter.TelegramBot.Interfaces;
using Adapter.TelegramBot.Models;
using Core.Interfaces.Driven;
using Core.Models.User;
using Telegram.Bot.Types;

namespace Adapter.TelegramBot.Services;

public class TelegramUserProvider : IUserProvider, ITelegramUserProvider
{
    private readonly ITelegramUserStore _userStore;

    private User? _tgUser;
    private UserTelegramModel? _userModel;

    public TelegramUserProvider(ITelegramUserStore userStore)
    {
        _userStore = userStore;
    }

    public async Task<UserTelegramModel> GetUser()
    {
        return _userModel ??=
            await _userStore.GetOrCreateUser(_tgUser ?? throw new ArgumentException("User hasn't been added"));
    }

    public void AddUser(User user)
    {
        _tgUser = user;
    }

    async Task<UserModel> IUserProvider.GetUser()
    {
        return await GetUser();
    }

    public bool HasUser()
    {
        return _tgUser != null;
    }
}