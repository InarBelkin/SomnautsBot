using Adapter.PostgreSQL.Entities;
using Core.Interfaces.Driven;
using Core.Models.Book;
using Core.Models.Exceptions;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;

namespace Adapter.PostgreSQL.Stores;

public class BooksStoreSql : IBooksStore
{
    private readonly SomnContext _db;

    public BooksStoreSql(SomnContext db)
    {
        _db = db;
    }

    public async Task<Result<BookHandleModel>> GetOne(Guid genId, bool onlyVisible = true)
    {
        var book = await _db.Books.Where(b => b.Description.GenId == genId && (!onlyVisible || b.IsVisibleToUsers))
            .FirstOrDefaultAsync();
        return book == null
            ? new Result<BookHandleModel>(new BookDoesntExistException())
            : new BookHandleModel(book.ContainingFolder, book.IsVisibleToUsers, book.Description);
    }

    public async Task<IList<BookHandleModel>> GetAll(bool onlyVisible)
    {
        return await _db.Books.Where(b => !onlyVisible || b.IsVisibleToUsers)
            .Select(b => new BookHandleModel(b.ContainingFolder, b.IsVisibleToUsers, b.Description))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<BookModel>> GetAllModels(int? userId)
    {
        return await _db.Books
            .Where(b => b.IsVisibleToUsers)
            .Select(b => new BookModel(b.Description.GenId, b.Description.Name, b.Description.Description,
                b.Description.Languages, b.Saves.Count(s => s.User.Id == userId))).ToListAsync();
    }

    public async Task Update(BookHandleModel bookModel)
    {
        var book = await _db.Books.SingleAsync(b => b.Description.GenId == bookModel.Description.GenId);
        book.ContainingFolder = bookModel.ContainingFolder;
        book.IsVisibleToUsers = bookModel.IsVisibleToUsers;
        book.Description = bookModel.Description;
        await _db.SaveChangesAsync();
    }

    public async Task Remove(Guid GenId)
    {
        await _db.Books.Where(b => b.Description.GenId == GenId).ExecuteDeleteAsync();
    }

    public async Task AddRange(IEnumerable<BookHandleModel> bookModel)
    {
        _db.AddRange(bookModel.Select(bm => new Book
        {
            ContainingFolder = bm.ContainingFolder,
            IsVisibleToUsers = bm.IsVisibleToUsers,
            Description = bm.Description
        }));
        await _db.SaveChangesAsync();
    }
}