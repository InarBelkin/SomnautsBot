using Core.Models.Book;

namespace Core.Interfaces.Driven;

public interface IBooksStore
{
    public Task<IList<BookModel>> GetAll(bool onlyVisible);

    public Task Update(BookModel bookModel);

    public Task Remove(Guid GenId);

    public Task AddRange(IEnumerable<BookModel> bookModel);
}