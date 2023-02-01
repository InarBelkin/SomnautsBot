using Core.Models.Book;

namespace Core.Interfaces.Driving;

public interface IBooksService
{
    public Task<IList<BookModel>> GetListOfBooks(bool onlyVisible = true);

    public Task<ScanBooksResultModel> ScanAvailableBooks(ScanBooksParamsModel scanBooksParams);
}