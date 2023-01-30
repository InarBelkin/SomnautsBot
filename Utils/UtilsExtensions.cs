using System.Diagnostics.CodeAnalysis;

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
}