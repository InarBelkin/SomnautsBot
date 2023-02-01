using AutoFixture;
using Core.Interfaces.Driven;
using Core.Models.Book;
using Core.Services;
using Core.Utils;
using Microsoft.Extensions.Options;
using Moq;

namespace Core.Tests.Services;

public class BooksServiceTests
{
    private readonly Mock<IBooksStore> _booksStoreMock = new();
    private readonly Fixture _fixture = new();
    private readonly Mock<IOptions<BooksOptions>> _optionsMock = new();

// 1. Test that the existing books are correctly updated in the database.
    [Fact]
    public async Task UpdateBooks_ExistingBooksUpdatedCorrectly()
    {
        // Arrange
        var scanParams = new ScanBooksParamsModel
        {
            DoWithNotExistingBooks = DoWithNotExistingBooksEnum.Nothing,
            SetVisibleFoundBooks = false
        };

        var firstBookGuid = Guid.Parse("00000000-0000-0000-0000-000000000001");

        //  var existingBooks = _fixture.CreateMany<BookModel>(2).ToList();
        var existingBooks = new List<BookModel>
        {
            new("oldPath1", true, _fixture.Create<BookDescriptionModel>() with
            {
                GenId = firstBookGuid
            })
        };

        var scannedBooks = new List<(string path, BookDescriptionModel bookDescription)>
        {
            ("newPath1", _fixture.Create<BookDescriptionModel>() with
            {
                GenId = firstBookGuid
            }),
            ("newPath2", _fixture.Create<BookDescriptionModel>())
        };


        _booksStoreMock.Setup(b => b.GetAll(false)).ReturnsAsync(existingBooks.ToList());
        var booksService = new BooksService(_optionsMock.Object, _booksStoreMock.Object);
        // Act
        await booksService.UpdateBooks(scanParams, scannedBooks.ToList());

        // Assert
        _booksStoreMock.Verify(b => b.Update(
                It.Is<BookModel>(bm =>
                    bm.ContainingFolder == "newPath1" &&
                    bm.IsVisibleToUsers == existingBooks[0].IsVisibleToUsers &&
                    bm.Description == scannedBooks[0].bookDescription
                )),
            Times.Once);

        _booksStoreMock.Verify<Task>(b =>
            b.AddRange(It.Is<IEnumerable<BookModel>>(e =>
                e.Count() == 1 && e.First() == new BookModel("newPath2", true, scannedBooks[1].bookDescription))));

        _booksStoreMock.Verify(b => b.Remove(It.IsAny<Guid>()), Times.Never);
    }
    //My_job_here_is_done.jpg

// 2. Test that the books that are not found in the database are handled correctly, depending on the 'DoWithNotExistingBooks' parameter.
// 3. Test that the 'ContainingFolder' field of the book model is correctly set.
// 4. Test that the 'IsVisibleToUsers' field of the book model is correctly set.
// 5. Test that the 'Description' field of the book model is correctly set.
// 6. Test that the scanned books are correctly added to the database.
}