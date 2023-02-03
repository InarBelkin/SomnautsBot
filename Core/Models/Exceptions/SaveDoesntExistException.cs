namespace Core.Models.Exceptions;

public class SaveDoesntExistException : ArgumentException
{
    public SaveDoesntExistException() : base("Save with this id doesn't exist")
    {
    }
}