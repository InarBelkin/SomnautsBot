using Adapter.PostgreSQL.Entities;
using Core.Interfaces.Driven;
using Core.Models.Book;
using Microsoft.EntityFrameworkCore;

namespace Adapter.PostgreSQL.Stores;

public class BooksStoreSql : IBooksStore
{
    private readonly SomnContext _db;

    public BooksStoreSql(SomnContext db)
    {
        _db = db;
    }

    public async Task<IList<BookModel>> GetAll(bool onlyVisible)
    {
        return await _db.Books.Where(b => !onlyVisible || b.IsVisibleToUsers)
            .Select(b => new BookModel(b.ContainingFolder, b.IsVisibleToUsers, b.Description))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task Update(BookModel bookModel)
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

    public async Task AddRange(IEnumerable<BookModel> bookModel)
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