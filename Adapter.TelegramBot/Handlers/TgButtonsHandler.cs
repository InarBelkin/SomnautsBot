using Adapter.TelegramBot.Utils;
using Core.Interfaces.Driving;
using Telegram.Bot;
using Telegram.Bot.Types;
using Utils.Language;

namespace Adapter.TelegramBot.Handlers;

public interface ITgButtonsHandler
{
    Task InvokeAsync(CallbackQuery updateCallbackQuery);
}

public class TgButtonsHandler : ITgButtonsHandler
{
    private readonly ITelegramBotClient _bot;
    private readonly UiLocalization _resources;
    private readonly IUserService _userService;

    public TgButtonsHandler(ITelegramBotClient bot, UiLocalization resources, IUserService userService)
    {
        _bot = bot;
        _resources = resources;
        _userService = userService;
    }

    public async Task InvokeAsync(CallbackQuery updateCallbackQuery)
    {
        var query = updateCallbackQuery.Data;
        if (query == null) return;
        var queryWords = query.ToLower().Split('_');
        switch (queryWords)
        {
            case ["#interfacelang", { } langCode]:
                if (LangEnum.TryFromValue(langCode, out var lang))
                {
                    await _userService.UpdateLang(lang);
                    await _bot.AnswerCallbackQueryAsync(updateCallbackQuery.Id,
                        _resources.UiLangSelected.WithErrorString(lang));
                }

                break;
        }
    }
}