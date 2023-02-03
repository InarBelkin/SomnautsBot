using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using LanguageExt.Common;

namespace Utils;

public static class UtilsExtensions
{
    public static bool TryGetFirst<T>(this IEnumerable<T> collection, [MaybeNullWhen(false)] out T value)
    {
        try
        {
            value = collection.First();
        }
        catch (InvalidOperationException)
        {
            value = default;
            return false;
        }

        return true;
    }

    public static async Task IfSuccAsync<T>(this Result<T> result, Func<T, Task> f)
    {
        if (result.IsSuccess)
        {
            var field = typeof(Result<T>).GetField("Value", BindingFlags.NonPublic | BindingFlags.Instance);
            var val = (T)field!.GetValue(result)!;
            await f(val);
        }
    }

    public static T GetValOrThrow<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            var fieldValue = typeof(Result<T>).GetField("Value", BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)fieldValue!.GetValue(result)!;
        }

        var fieldException = typeof(Result<T>).GetField("exception", BindingFlags.NonPublic | BindingFlags.Instance);
        throw (Exception)fieldException!.GetValue(result)!;
    }
}