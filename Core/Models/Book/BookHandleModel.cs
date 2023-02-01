namespace Core.Models.Book;

public record BookHandleModel(string ContainingFolder, bool IsVisibleToUsers, BookDescriptionModel Description);

public record BookHandleModelWithSaves(string ContainingFolder, bool IsVisibleToUsers, BookDescriptionModel Description,
    int CountOfSaves) : BookHandleModel(ContainingFolder, IsVisibleToUsers, Description);