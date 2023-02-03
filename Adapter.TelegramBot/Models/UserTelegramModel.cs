using Core.Models.User;
using Utils.Language;

namespace Adapter.TelegramBot.Models;

public record UserTelegramModel(int Id, string UserName, LangEnum InterfaceLang, long TelegramId)
    : UserModel(Id, UserName, InterfaceLang);