using System.Text.Json;
using Core.Interfaces.Driven;
using Core.Interfaces.Driving;
using Core.Models.Book;
using Core.Utils;
using Microsoft.Extensions.Options;
using Utils;

namespace Core.Services;

public class BooksService : IBooksService
{
    private readonly IBooksStore _booksStore;
    private readonly IUserService _userService;
    private readonly ISavesStore _savesStore;
    private readonly BooksOptions _options;

    public BooksService(IOptions<BooksOptions> options, IBooksStore booksStore, IUserService userService,
        ISavesStore savesStore)
    {
        _booksStore = booksStore;
        _userService = userService;
        _savesStore = savesStore;
        _options = options.Value;
    }

    public async Task<IList<BookHandleModel>> GetListOfHandleBooks(bool onlyVisible = true)
    {
        return await _booksStore.GetAll(onlyVisible);
    }

    public async Task<IEnumerable<BookModel>> GetBooks()
    {
        var user = await _userService.GetUserOrNull();
        var books = await _booksStore.GetAllModels(user?.Id);
        return books;
    }

    public async Task<ScanBooksResultModel> ScanAvailableBooks(ScanBooksParamsModel scanBooksParams)
    {
        var books = await SearchInDirectory(_options.Path).ToListAsync();
        var result = new ScanBooksResultModel
        {
            FoundBooks = books.Select(b =>
                new SearchResult(b.ContainingFolderPath,
                    b.BookDescription != null ? "Success" : $"Fail:{b.Exception!.Message}")).ToList(),
            Message = ""
        };

        var correctBooks = books.Where(b => b.BookDescription != null)
            .Select(b => (path: b.ContainingFolderPath, bookDescription: b.BookDescription!)).ToList();

        var (noRepeated, repeats) = TestNoRepeatedCodes(correctBooks);
        if (!noRepeated)
        {
            result.Message += "Error: there are books with the same generated id: \n" +
                              JsonSerializer.Serialize(repeats) + "/n";
            return result;
        }

        await UpdateBooks(scanBooksParams, correctBooks);

        return result;
    }

    //This method must be somewhere in other service
    private async IAsyncEnumerable<BookScanResult> SearchInDirectory(string directory)
    {
        var files = Directory.GetFiles(directory);
        var projFile = files.FirstOrDefault(f =>
            string.Equals(Path.GetFileName(f), _options.ProjectFileName, StringComparison.CurrentCultureIgnoreCase));
        if (projFile != null)
        {
            await using var fs = new FileStream(projFile, FileMode.Open);
            BookDescriptionModel? bookDescription = null;
            Exception? exception = null;
            try
            {
                bookDescription = await JsonSerializer.DeserializeAsync<BookDescriptionModel>(fs,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception e)
            {
                exception = e;
            }

            if (bookDescription != null)
            {
                yield return new BookScanResult(directory, bookDescription, null);
                yield break;
            }

            yield return new BookScanResult(directory, null,
                exception ?? new NullReferenceException("Book model was null"));
            yield break;
        }

        foreach (var innerDirectory in Directory.GetDirectories(directory))
        await foreach (var bookDescription in SearchInDirectory(innerDirectory))
            yield return bookDescription;
    }

    //result
    private (bool noRepeated, IEnumerable<Guid> repeats) TestNoRepeatedCodes(
        List<(string path, BookDescriptionModel bookDescription)> books)
    {
        var repeatedCodes = books
            .GroupBy(b => b.bookDescription.GenId)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();
        if (repeatedCodes.Count != 0) return (false, repeatedCodes);

        return (true, Array.Empty<Guid>());
    }

    public async Task UpdateBooks(ScanBooksParamsModel scanParams,
        List<(string path, BookDescriptionModel bookDescription)> scannedBooks)
    {
        var existingBooks = await _booksStore.GetAll(false);

        foreach (var existingBook in existingBooks)
            if (scannedBooks.Where(b => b.bookDescription.GenId == existingBook.Description.GenId)
                .TryGetFirst(out var scannedBook))
            {
                await _booksStore.Update(new BookHandleModel(
                    scannedBook.path,
                    Description: scannedBook.bookDescription,
                    IsVisibleToUsers: scanParams.SetVisibleFoundBooks || existingBook.IsVisibleToUsers));
                scannedBooks.Remove(scannedBook);
            }
            else
            {
                switch (scanParams.DoWithNotExistingBooks.Name)
                {
                    case nameof(DoWithNotExistingBooksEnum.Nothing):
                        break;
                    case nameof(DoWithNotExistingBooksEnum.Delete):
                        await _booksStore.Update(existingBook with { IsVisibleToUsers = false });
                        break;
                    case nameof(DoWithNotExistingBooksEnum.Invisible):
                        await _booksStore.Remove(existingBook.Description.GenId);
                        break;
                }
            }

        await _booksStore
            .AddRange(scannedBooks.Select(sb => new BookHandleModel(sb.path, true, sb.bookDescription)));
    }
}