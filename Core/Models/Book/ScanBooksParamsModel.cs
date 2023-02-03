namespace Core.Models.Book;

public class ScanBooksParamsModel
{
    public DoWithNotExistingBooksEnum DoWithNotExistingBooks { get; init; } = DoWithNotExistingBooksEnum.Nothing;
    public bool SetVisibleFoundBooks { get; set; } = false;
}