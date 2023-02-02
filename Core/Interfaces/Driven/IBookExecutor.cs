using System.Dynamic;
using Core.Models.Book;

namespace Core.Interfaces.Driven;

public interface IBookExecutor
{
    /// <returns>State that must be stored for user</returns>
    public ValueTask<ExpandoObject> CreateInitState(BookHandleModel bookHandleModel);
}