using Adapter.TelegramBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Adapter.TelegramBot.Handlers;

public interface ITgButtonsHandler
{
    Task InvokeAsync(CallbackQuery updateCallbackQuery);
}

public class TgButtonsHandler : ITgButtonsHandler
{
    private readonly ITelegramBotClient _bot;
    private readonly UiLocalization _resources;

    public TgButtonsHandler(ITelegramBotClient bot, UiLocalization resources)
    {
        _bot = bot;
        _resources = resources;
    }

    public async Task InvokeAsync(CallbackQuery updateCallbackQuery)
    {
    }
}