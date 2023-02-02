using Adapter.TelegramBot.Interfaces;
using Adapter.TelegramBot.Utils;
using Core.Interfaces.Driving;
using Core.Models.Exceptions;
using Telegram.Bot;
using Utils.Language;

namespace Adapter.TelegramBot.Handlers;

public interface ITgMessagesHandler
{
    Task InvokeAsync(string command);
}

public class TgMessagesHandler : ITgMessagesHandler
{
    private readonly ITelegramBotClient _bot;
    private readonly ISavesService _savesService;
    private readonly ISendErrorToUserMiddleware _sendErrorToUserMiddleware;
    private readonly UiLocalization _uiResources;
    private readonly ITelegramUserProvider _userProvider;

    public TgMessagesHandler(ISavesService savesService, ISendErrorToUserMiddleware sendErrorToUserMiddleware,
        ITelegramBotClient bot, ITelegramUserProvider userProvider, UiLocalization uiResources)
    {
        _savesService = savesService;
        _sendErrorToUserMiddleware = sendErrorToUserMiddleware;
        _bot = bot;
        _userProvider = userProvider;
        _uiResources = uiResources;
    }

    public async Task InvokeAsync(string command)
    {
        await _sendErrorToUserMiddleware.Send(async () => { await HandleMessageToBook(command); });
    }

    private async Task HandleMessageToBook(string message)
    {
        var user = await _userProvider.GetUser();
        try
        {
            var replica = await _savesService.NextReplica(message);
            await _bot.PrintReplica(user, replica, _uiResources);
        }
        catch (UserCurrentSaveIsNull e)
        {
            await _bot.SendTextMessageAsync(user.TelegramId,
                _uiResources.TextWithoutSaves.WithErrorString(user.InterfaceLang));
        }
    }
}