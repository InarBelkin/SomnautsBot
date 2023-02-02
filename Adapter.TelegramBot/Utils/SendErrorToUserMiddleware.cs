using Adapter.TelegramBot.Interfaces;
using Core.Models.Exceptions;
using Telegram.Bot;
using Utils.Language;

namespace Adapter.TelegramBot.Utils;

public interface ISendErrorToUserMiddleware
{
    Task Send(Func<Task> handler);
}

public class SendErrorToUserMiddleware : ISendErrorToUserMiddleware
{
    private readonly ITelegramBotClient _bot;
    private readonly UiLocalization _uiResources;
    private readonly ITelegramUserProvider _userProvider;

    public SendErrorToUserMiddleware(UiLocalization uiResources, ITelegramBotClient bot,
        ITelegramUserProvider userProvider)
    {
        _uiResources = uiResources;
        _bot = bot;
        _userProvider = userProvider;
    }

    public async Task Send(Func<Task> handler)
    {
        var user = await _userProvider.GetUser();
        try
        {
            await handler();
        }
        catch (Exception e)
        {
            var textDict = e switch
            {
                BookDoesntExistException => _uiResources.BookIdIsntCorrect,
                BookExecutionException => _uiResources.BookExecutionError,
                SaveDoesntExistException => _uiResources.SaveDoesntExist,
                _ => _uiResources.InternalServerError
            };
            try
            {
                await _bot.SendTextMessageAsync(user.TelegramId,
                    textDict.WithErrorString(user.InterfaceLang));
            }
            catch (Exception sendException)
            {
                throw new AggregateException(
                    "There are two exceptions: first - error when attempt to send error to user, second - error which whe where trying to send",
                    sendException,
                    e);
            }

            throw;
        }
    }
}