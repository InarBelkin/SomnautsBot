using Utils.Language;

namespace Core.Models.Book;

public record BookDescriptionModel
{
    public required Guid GenId { get; init; }
    public required Dictionary<string, string> Name { get; init; }
    public required Dictionary<string, string> Description { get; init; }
    public required LangEnum[] Languages { get; init; }
    public required int BookVersion { get; init; }
    public required string EngineVersion { get; init; }
    public required string[] Technologies { get; init; }
}