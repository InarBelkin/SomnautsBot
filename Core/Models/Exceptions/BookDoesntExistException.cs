namespace Core.Models.Exceptions;

public class BookDoesntExistException : ArgumentException
{
    public BookDoesntExistException() : base("Book with this GenId doesn't exist")
    {
    }
}