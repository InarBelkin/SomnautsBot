using System.Dynamic;
using Core.Models.Book;
using Core.Models.Executor;
using Utils.Language;

namespace Core.Interfaces.Driven;

public interface IBookExecutor
{
    /// <returns>State that must be stored for user</returns>
    public ValueTask<ExpandoObject> CreateInitState(BookHandleModel bookHandleModel);

    ValueTask<ReplicaModel> GetCurrentReplica(BookHandleModel book, ExpandoObject state, LangEnum language);

    ValueTask<ReplicaModel> NextReplica(BookHandleModel book, ExpandoObject state, LangEnum language,
        string? answerId, string answerText);
}