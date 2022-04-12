namespace Boilerplate.Common.Utils;

public static class StringExtensions
{
    public static bool IsEmpty(this string? str) => str is null or "";

    public static bool IsNotEmpty(this string? str) => !str.IsEmpty();
}
