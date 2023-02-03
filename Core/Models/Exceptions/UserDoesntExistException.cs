namespace Core.Models.Exceptions;

public class UserDoesntExistException : ArgumentException
{
    public UserDoesntExistException() : base("User hasn't been added")
    {
    }
}