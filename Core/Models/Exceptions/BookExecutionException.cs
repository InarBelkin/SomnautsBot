namespace Core.Models.Exceptions;

/// <summary>
///     All scripts execution errors must be wrapped in this exception or on its inheritor
/// </summary>
public class BookExecutionException : Exception
{
    public BookExecutionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

public class BookExecutionConverterException : BookExecutionException
{
    public BookExecutionConverterException(Exception innerException) : base(
        "Converting dynamic to ReplicaModel was unsuccessful", innerException)
    {
    }
}