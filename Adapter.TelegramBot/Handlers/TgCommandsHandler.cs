using System.Text;
using Adapter.TelegramBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Utils.Language;

namespace Adapter.TelegramBot.Handlers;

public interface ITgCommandsHandler
{
    Task InvokeAsync(string command);
}

public class TgCommandsHandler : ITgCommandsHandler
{
    private readonly ITelegramBotClient _bot;
    private readonly UiLocalization _uiResources;

    public TgCommandsHandler(UiLocalization uiResources, ITelegramBotClient bot)
    {
        _uiResources = uiResources;
        _bot = bot;
    }

    public async Task InvokeAsync(string command)
    {
        var commandWords = command.ToLower().Split(new[] { '/', '_', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        switch (commandWords)
        {
            case ["start"] or ["help"]:
                break;
        }
    }
}