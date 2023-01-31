using Utils.Language;

namespace Adapter.PostgreSQL.Entities;

public class User
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required LangEnum InterfaceLang { get; set; }

    public int? CurrentSaveId { get; set; }
    public BookSave? CurrentSave { get; set; }

    public int? LastMessageId { get; set; }
    public long? TelegramId { get; set; }
    public List<BookSave> Saves { get; set; } = new();
}