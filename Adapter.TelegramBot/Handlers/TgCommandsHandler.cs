using Adapter.TelegramBot.Interfaces;
using Adapter.TelegramBot.Utils;
using Telegram.Bot;
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
    private readonly ITelegramUserProvider _userProvider;

    public TgCommandsHandler(UiLocalization uiResources, ITelegramBotClient bot, ITelegramUserProvider userProvider)
    {
        _uiResources = uiResources;
        _bot = bot;
        _userProvider = userProvider;
    }

    public async Task InvokeAsync(string command)
    {
        var commandWords = command.ToLower().Split(new[] { '/', '_', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        switch (commandWords)
        {
            case ["start"] or ["help"]:
                await StartCommand();
                break;

            case ["interfacelang"]:
                await InterfaceLangCommand();
                break;
        }
    }

    private async Task StartCommand()
    {
        var user = await _userProvider.GetUser();
        await _bot.SendTextMessageAsync(user.TelegramId,
            _uiResources.Help.WithErrorString(user.InterfaceLang));
    }

    private async Task InterfaceLangCommand()
    {
        var user = await _userProvider.GetUser();
        await _bot.SendTextMessageAsync(user.TelegramId,
            _uiResources.SelectUiLang.WithErrorString(user.InterfaceLang),
            replyMarkup: new InlineKeyboardMarkup(LangEnum.List.Select(l =>
                    InlineKeyboardButton.WithCallbackData(l.LangName, $"#interfacelang_{l.Value}"))
                .Select(b => new[] { b }))
        );
    }
}