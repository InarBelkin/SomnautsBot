namespace Core.Models.Exceptions;

/// <summary>
///     All scripts execution errors must be wrapped in this exception or on its inheritor
/// </summary>
public class BookExecutionError : Exception
{
    public BookExecutionError(string message, Exception innerException) : base(message, innerException)
    {
    }
}