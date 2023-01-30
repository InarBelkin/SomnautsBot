using Adapter.TelegramBot.Handlers;
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
    private readonly ITgButtonsHandler _tgButtonsHandler;

    public TgUpdateHandler(ITgCommandsHandler commandsHandler, ITgButtonsHandler tgButtonsHandler)
    {
        _commandsHandler = commandsHandler;
        _tgButtonsHandler = tgButtonsHandler;
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
        //var user = await _usersService.GetOrCreateUser(msg.From); TODO: users 1
        if (msg.Text.StartsWith("/") && msg.Text.Length > 1) await _commandsHandler.InvokeAsync(msg.Text);
    }

    private async Task HandleButton(CallbackQuery updateCallbackQuery)
    {
        //var user = await _usersService.GetOrCreateUser(updateCallbackQuery.From); TODO: users 2
        await _tgButtonsHandler.InvokeAsync(updateCallbackQuery);
    }
}