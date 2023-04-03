using System.Collections;

namespace Boilerplate.Common.Utils;

public static class EnumerableExtensions
{
    public static IEnumerable<T> AsEnumerable<T>(this IEnumerable enumerable)
    {
        var enumerator = enumerable.GetEnumerator();
        while (enumerator.MoveNext()) {
            if (enumerator.Current is not T current) {
                throw new InvalidCastException($"The type of object in enumerable doesn't match Type {typeof(T)}");
            }

            yield return current;
        }
    }
}
