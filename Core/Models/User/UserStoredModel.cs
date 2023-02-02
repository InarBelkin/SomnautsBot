using Utils.Language;

namespace Core.Models.User;

public record UserStoredModel(int Id, string UserName, LangEnum InterfaceLang, int? CurrentSaveId);