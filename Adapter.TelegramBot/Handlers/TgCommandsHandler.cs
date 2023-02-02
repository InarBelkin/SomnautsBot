using System.Text;
using Adapter.TelegramBot.Interfaces;
using Adapter.TelegramBot.Utils;
using Core.Interfaces.Driving;
using Core.Models.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Utils;
using Utils.Language;

namespace Adapter.TelegramBot.Handlers;

public interface ITgCommandsHandler
{
    Task InvokeAsync(string command);
}

public class TgCommandsHandler : ITgCommandsHandler
{
    private readonly IBooksService _booksService;
    private readonly ITelegramBotClient _bot;
    private readonly ISavesService _savesService;
    private readonly UiLocalization _uiResources;
    private readonly ITelegramUserProvider _userProvider;

    public TgCommandsHandler(UiLocalization uiResources, ITelegramBotClient bot, ITelegramUserProvider userProvider,
        IBooksService booksService, ISavesService savesService)
    {
        _uiResources = uiResources;
        _bot = bot;
        _userProvider = userProvider;
        _booksService = booksService;
        _savesService = savesService;
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
            case ["read", { } bookStringId]:
                await ReadCommand(bookStringId);
                break;
            case ["newsave", { } bookStringId]:
                await NewSaveCommand(bookStringId);
                break;
            default:
                await IncorrectCommand();
                break;
        }
    }

    private async Task IncorrectCommand()
    {
        var user = await _userProvider.GetUser();
        await _bot.SendTextMessageAsync(user.TelegramId,
            _uiResources.CommandIsntCorrect.WithErrorString(user.InterfaceLang));
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

    private async Task ReadCommand(string bookStringId)
    {
        var user = await _userProvider.GetUser();
        const string dateTimeFormat = "dd MMMM yyyy HH:mm";
        if (Guid.TryParse(bookStringId, out var genId))
        {
            var result = await _savesService.GetSavesOfThisUser(genId);
            await result.IfSuccAsync(async saves =>
                {
                    var text = new StringBuilder();
                    text.Append(_uiResources.SelectBookSave.WithErrorString(user.InterfaceLang));
                    text.Append("\n\n");
                    foreach (var (id, createdDate, updatedDate, langEnum) in saves)
                    {
                        text.AppendFormat(_uiResources.SelectBookSaveItem.WithErrorString(user.InterfaceLang),
                            createdDate.ToUniversalTime().ToString(dateTimeFormat) + " UTC",
                            updatedDate.ToUniversalTime().ToString(dateTimeFormat) + " UTC",
                            langEnum,
                            $"/load_{id}");
                        text.Append("\n\n");
                    }

                    text.AppendFormat(_uiResources.SelectBookSaveCreate.WithErrorString(user.InterfaceLang),
                        $"/newsave_{genId:N}");
                    await _bot.SendTextMessageAsync(user.TelegramId, text.ToString());
                }
            );
            if (result.IsSuccess) return;
        }

        await _bot.SendTextMessageAsync(user.TelegramId,
            _uiResources.BookIdIsntCorrect.WithErrorString(user.InterfaceLang));
    }

    private async Task NewSaveCommand(string bookStringId)
    {
        var user = await _userProvider.GetUser();
        if (Guid.TryParse(bookStringId, out var genId))
            try
            {
                await _savesService.CreateNewSave(genId);
                await _bot.SendTextMessageAsync(user.TelegramId, "Save created"); //TODO: change to save
            }
            catch (Exception e)
            {
                var textDict = e switch
                {
                    BookDoesntExistException => _uiResources.BookIdIsntCorrect,
                    BookExecutionError => _uiResources.BookExecutionError,
                    _ => _uiResources.InternalServerError
                };
                await _bot.SendTextMessageAsync(user.TelegramId,
                    textDict.WithErrorString(user.InterfaceLang));
                throw;
            }
        else
            await _bot.SendTextMessageAsync(user.TelegramId,
                _uiResources.BookIdIsntCorrect.WithErrorString(user.InterfaceLang));
    }
}