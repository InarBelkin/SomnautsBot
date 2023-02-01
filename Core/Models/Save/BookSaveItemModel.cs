using Utils.Language;

namespace Core.Models.Save;

public record BookSaveItemModel(int Id, DateTime CreatedDate, DateTime UpdatedDate, LangEnum Language);