namespace Core.Models.Exceptions;

public class UserDoesntExistsException : ArgumentException
{
    public UserDoesntExistsException() : base("User hasn't been added")
    {
    }
}