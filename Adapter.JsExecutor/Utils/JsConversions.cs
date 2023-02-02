using Core.Models.Exceptions;
using Core.Models.Executor;

namespace Adapter.JsExecutor.Utils;

public static class JsConversions
{
    public static ReplicaModel ConvertDynamicToReplica(dynamic replicaExpando)
    {
        try
        {
            return new ReplicaModel
            {
                Text = replicaExpando.text,
                TakesFreeText = replicaExpando.takesFreeText,
                Answers = replicaExpando.answers == null
                    ? Array.Empty<AnswerModel>()
                    : (replicaExpando.answers as IEnumerable<object>)!.Select(ConvertDynamicToAnswer).ToArray()
            };
        }
        catch (Exception e)
        {
            throw new BookExecutionConverterException(e);
        }
    }

    public static AnswerModel ConvertDynamicToAnswer(dynamic answerExpando)
    {
        return new AnswerModel
        {
            Id = answerExpando.id,
            Text = answerExpando.text
        };
    }

    public static T WrapJintExceptions<T>(this string message, Func<T> func)
    {
        try
        {
            return func();
        }
        catch (BookExecutionException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new BookExecutionException(message, e);
        }
    }
}