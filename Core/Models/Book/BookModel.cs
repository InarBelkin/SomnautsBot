using Utils.Language;

namespace Core.Models.Book;

public record BookModel(
    Guid GenId,
    Dictionary<string, string> Name,
    Dictionary<string, string> Description,
    LangEnum[] Languages,
    int CountOfSaves);