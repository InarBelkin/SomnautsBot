namespace Core.Models.Book;

public record BookScanResult(string ContainingFolderPath, BookDescriptionModel? BookDescription, Exception? Exception);