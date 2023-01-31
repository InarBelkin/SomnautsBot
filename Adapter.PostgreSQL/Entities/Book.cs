namespace Adapter.PostgreSQL.Entities;

public class Book
{
    public int Id { get; set; }
    public List<BookSave> Saves { get; set; } = new();
    public required string ContainingFolder { get; set; }
    public required bool IsVisibleToUsers { get; set; }

    public required BookDescriptionEntity Description { get; set; }
}