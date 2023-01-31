using Utils.Language;

namespace Core.Models.User;

public record UserModel(int Id, string UserName, LangEnum InterfaceLang);