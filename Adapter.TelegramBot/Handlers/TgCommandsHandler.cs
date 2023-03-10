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
    private readonly IBooksHandleService _booksHandleService;
    private readonly ITelegramBotClient _bot;
    private readonly ISavesService _savesService;
    private readonly ISendErrorToUserMiddleware _sendErrorToUserMiddleware;
    private readonly UiLocalization _uiResources;
    private readonly ITelegramUserProvider _userProvider;

    public TgCommandsHandler(UiLocalization uiResources, ITelegramBotClient bot, ITelegramUserProvider userProvider,
        IBooksHandleService booksHandleService, ISavesService savesService,
        ISendErrorToUserMiddleware sendErrorToUserMiddleware)
    {
        _uiResources = uiResources;
        _bot = bot;
        _userProvider = userProvider;
        _booksHandleService = booksHandleService;
        _savesService = savesService;
        _sendErrorToUserMiddleware = sendErrorToUserMiddleware;
    }

    public async Task InvokeAsync(string command)
    {
        await _sendErrorToUserMiddleware.Send(async () =>
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
                case ["load", { } saveStringId]:
                    await LoadCommand(saveStringId);
                    break;
                default:
                    await IncorrectCommand();
                    break;
            }
        });
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
        var books = await _booksHandleService.GetBooks();
        var user = await _userProvider.GetUser();
        var text = new StringBuilder();
        text.Append(_uiResources.SelectBook.WithErrorString(user.InterfaceLang));
        text.Append("\n\n");
        foreach (var (genId, name, description, langEnums, countOfSaves) in books)
        {
            text.AppendFormat(_uiResources.SelectBookItemDescription.WithErrorString(user.InterfaceLang),
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
                        text.AppendFormat(
                            _uiResources.SelectBookSaveItemDescription.WithErrorString(user.InterfaceLang),
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
        {
            await _savesService.CreateNewSave(genId);
            await _bot.SendTextMessageAsync(user.TelegramId, "Save created"); //TODO: change to save
        }
        else
        {
            await _bot.SendTextMessageAsync(user.TelegramId,
                _uiResources.BookIdIsntCorrect.WithErrorString(user.InterfaceLang));
        }
    }

    private async Task LoadCommand(string saveStringId)
    {
        var user = await _userProvider.GetUser();
        if (int.TryParse(saveStringId, out var saveId))
        {
            await _savesService.SwitchToSave(saveId);
            var replica = await _savesService.GetCurrentReplica();
            await _bot.PrintReplica(user, replica, _uiResources);
        }
        else
        {
            throw new SaveDoesntExistException();
        }
    }
}