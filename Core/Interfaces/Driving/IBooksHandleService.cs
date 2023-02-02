using Core.Models.Book;

namespace Core.Interfaces.Driving;

public interface IBooksHandleService
{
    Task<IList<BookHandleModel>> GetListOfHandleBooks(bool onlyVisible = true);
    Task<IEnumerable<BookModel>> GetBooks();

    Task<ScanBooksResultModel> ScanAvailableBooks(ScanBooksParamsModel scanBooksParams);
}