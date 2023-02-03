using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using Utils.Language;

namespace Adapter.PostgreSQL.Entities;

public class BookSave
{
    public int Id { get; set; }
    public required User User { get; set; }
    public required Book Book { get; set; }
    public required DateTime CreatedDate { get; set; }
    public required DateTime UpdatedDate { get; set; }
    public required LangEnum Language { get; set; }

    [Column(TypeName = "jsonb")] public required ExpandoObject BookState { get; set; }
}