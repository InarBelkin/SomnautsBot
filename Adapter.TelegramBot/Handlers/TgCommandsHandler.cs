using System.Text;
using Adapter.TelegramBot.Interfaces;
using Adapter.TelegramBot.Utils;
using Core.Interfaces.Driving;
using Telegram.Bot;
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
    private readonly ITelegramUserProvider _userProvider;
    private readonly IBooksService _booksService;

    public TgCommandsHandler(UiLocalization uiResources, ITelegramBotClient bot, ITelegramUserProvider userProvider,
        IBooksService booksService)
    {
        _uiResources = uiResources;
        _bot = bot;
        _userProvider = userProvider;
        _booksService = booksService;
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
            case ["books"]:
                await BooksCommand();
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

    private async Task BooksCommand()
    {
        var books = await _booksService.GetBooks();
        var user = await _userProvider.GetUser();
        var text = new StringBuilder();
        text.Append(_uiResources.SelectBook.WithErrorString(user.InterfaceLang));
        text.Append("\n\n");
        foreach (var (genId, name, description, langEnums, countOfSaves) in books)
        {
            text.AppendFormat(_uiResources.SelectBookDescription.WithErrorString(user.InterfaceLang),
                name.NearestLang(user.InterfaceLang),
                description.NearestLang(user.InterfaceLang),
                string.Join<LangEnum>(",", langEnums),
                countOfSaves,
                $"/read_{genId:N}");
            text.Append("\n\n");
        }

        await _bot.SendTextMessageAsync(user.TelegramId, text.ToString(), ParseMode.Html);
    }
}