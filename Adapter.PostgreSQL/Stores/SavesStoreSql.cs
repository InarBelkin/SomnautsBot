using System.Dynamic;
using Adapter.PostgreSQL.Entities;
using Core.Interfaces.Driven;
using Core.Models.Exceptions;
using Core.Models.Save;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Utils.Language;

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

    public async Task CreateNewSave(int userId, Guid genId, ExpandoObject state, LangEnum language)
    {
        var book = await _db.Books.Where(b => b.Description.GenId == genId).FirstOrDefaultAsync();
        var user = await _db.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
        if (book == null) throw new BookDoesntExistException();
        if (user == null) throw new UserDoesntExistException();
        _db.BookSaves.Add(new BookSave
        {
            Book = book,
            User = user,
            Language = language,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            BookState = state
        });
        await _db.SaveChangesAsync();
    }

    public async Task<SaveStateModel> GetStateBySaveId(int saveId)
    {
        var state = await _db.BookSaves.Where(s => s.Id == saveId)
            .Select(s => new SaveStateModel(s.Id, s.User.Id, s.Book.Description.GenId, s.Language, s.BookState))
            .FirstOrDefaultAsync();
        return state ?? throw new SaveDoesntExistException();
    }
}