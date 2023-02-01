namespace Core.Models.Book;

public record BookModel(string ContainingFolder, bool IsVisibleToUsers, BookDescriptionModel Description);