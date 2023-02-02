using Adapter.TelegramBot.Models;
using Adapter.TelegramBot.Utils;
using Core.Models.Executor;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Utils.Language;

namespace Adapter.TelegramBot.Handlers;

public static class HandlersExtensions
{
    public static Task PrintReplica(this ITelegramBotClient bot, UserTelegramModel user, ReplicaModel replica,
        UiLocalization localization)
    {
        return bot.SendTextMessageAsync(user.TelegramId, replica.Text,
            replyMarkup: new ReplyKeyboardMarkup(replica.Answers.Select(a =>
                new[] { new KeyboardButton(a.Text) }))
            {
                ResizeKeyboard = true, OneTimeKeyboard = true,
                InputFieldPlaceholder =
                    replica.TakesFreeText ? localization.CanWriteYourOwnText.WithErrorString(user.InterfaceLang) : null
            });
    }
}