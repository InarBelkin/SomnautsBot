using Core.Models.Book;
using LanguageExt.Common;

namespace Core.Interfaces.Driven;

public interface IBooksStore
{
    Task<Result<BookHandleModel>> GetOne(Guid genId, bool onlyVisible = true);
    public Task<IList<BookHandleModel>> GetAll(bool onlyVisible);
    public Task<IEnumerable<BookModel>> GetAllModels(int? userId);

    public Task Update(BookHandleModel bookModel);

    public Task Remove(Guid GenId);

    public Task AddRange(IEnumerable<BookHandleModel> bookModel);
}