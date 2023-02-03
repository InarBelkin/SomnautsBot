namespace Core.Models.Exceptions;

public class UserCurrentSaveIsNull : ArgumentException
{
    public UserCurrentSaveIsNull() : base("Current save is null")
    {
    }
}