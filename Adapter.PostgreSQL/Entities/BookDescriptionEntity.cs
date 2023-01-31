using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Utils.Language;

namespace Adapter.PostgreSQL.Entities;

[Index(nameof(GenId), IsUnique = true)]
public class BookDescriptionEntity
{
    public required Guid GenId { get; init; }
    public required Dictionary<string, string> Name { get; init; }
    public required Dictionary<string, string> Description { get; init; }
    [Column(TypeName = "jsonb")] public required LangEnum[] Languages { get; init; }
    public required int BookVersion { get; init; }
    public required string EngineVersion { get; init; }
    public required string[] Technologies { get; init; }
}