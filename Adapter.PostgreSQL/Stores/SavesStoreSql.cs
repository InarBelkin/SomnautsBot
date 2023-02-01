using Core.Interfaces.Driven;
using Core.Models.Exceptions;
using Core.Models.Save;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;

namespace Adapter.PostgreSQL.Stores;

public class SavesStoreSql : ISavesStore
{
    private readonly SomnContext _db;

    public SavesStoreSql(SomnContext db)
    {
        _db = db;
    }

    public async Task<Result<IEnumerable<BookSaveItemModel>>> GetSavesByUser(int userId, Guid genId)
    {
        var isBookExist =
            await _db.Books.Where(b => b.Description.GenId == genId && b.IsVisibleToUsers).CountAsync() == 1;
        if (!isBookExist) return new Result<IEnumerable<BookSaveItemModel>>(new BookDoesntExistException());

        var saves = await _db.BookSaves.Where(s => s.Book.Description.GenId == genId && s.User.Id == userId)
            .Select(s => new BookSaveItemModel(s.Id, s.CreatedDate, s.UpdatedDate, s.Language))
            .ToListAsync();
        return saves;
    }
}