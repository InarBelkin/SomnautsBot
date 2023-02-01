namespace Core.Models.Book;

public record SearchResult(string Path, string Result);

public class ScanBooksResultModel
{
    public List<SearchResult> FoundBooks { get; set; } = new();
    public required string Message { get; set; }
}