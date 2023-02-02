using Adapter.TelegramBot.Handlers;
using Adapter.TelegramBot.Interfaces;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Adapter.TelegramBot;

public interface ITgUpdateHandler
{
    Task InvokeAsync(Update update, CancellationToken token);
}

public class TgUpdateHandler : ITgUpdateHandler
{
    private readonly ITgCommandsHandler _commandsHandler;
    private readonly ITelegramUserProvider _telegramUserProvider;
    private readonly ITgButtonsHandler _tgButtonsHandler;
    private readonly ITgMessagesHandler _tgMessagesHandler;

    public TgUpdateHandler(ITgCommandsHandler commandsHandler, ITgButtonsHandler tgButtonsHandler,
        ITelegramUserProvider telegramUserProvider, ITgMessagesHandler tgMessagesHandler)
    {
        _commandsHandler = commandsHandler;
        _tgButtonsHandler = tgButtonsHandler;
        _telegramUserProvider = telegramUserProvider;
        _tgMessagesHandler = tgMessagesHandler;
    }

    public async Task InvokeAsync(Update update, CancellationToken token)
    {
        switch (update.Type)
        {
            case UpdateType.Message:
                await HandleMessage(update.Message!);
                break;
            case UpdateType.CallbackQuery:
                await HandleButton(update.CallbackQuery!);
                break;
        }
    }

    private async Task HandleMessage(Message msg)
    {
        if (msg.From is null || msg.Text is null) return;
        _telegramUserProvider.AddUser(msg.From);
        if (msg.Text.StartsWith("/")) await _commandsHandler.InvokeAsync(msg.Text);
        else await _tgMessagesHandler.InvokeAsync(msg.Text);
    }

    private async Task HandleButton(CallbackQuery updateCallbackQuery)
    {
        _telegramUserProvider.AddUser(updateCallbackQuery.From);
        await _tgButtonsHandler.InvokeAsync(updateCallbackQuery);
    }
}