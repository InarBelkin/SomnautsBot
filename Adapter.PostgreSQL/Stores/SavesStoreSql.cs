using Core.Interfaces.Driven;
using Microsoft.EntityFrameworkCore;

namespace Adapter.PostgreSQL.Stores;

public class SavesStoreSql : ISavesStore
{
    private readonly SomnContext _db;

    public SavesStoreSql(SomnContext db)
    {
        _db = db;
    }

    public async Task<Dictionary<Guid, int>> GetSavesCountByUserId(int userId)
    {
        var query = _db.Books.Select(b =>
            new KeyValuePair<Guid, int>(b.Description.GenId, b.Saves.Count(s => s.User.Id == userId)));

        return await query.ToDictionaryAsync(p => p.Key, p => p.Value);
    }
}