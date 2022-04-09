namespace Boilerplate.Common.Data.Querying;

public class StringOperatorEnum : OperatorEnum
{
    private const string startsWith = "startswith";
    public static readonly OperatorEnum StartsWith = new(startsWith, 9);

    private const string endsWith = "endswith";
    public static readonly OperatorEnum EndsWith = new(endsWith, 10);

    private const string contains = "contains";
    public static readonly OperatorEnum Contains = new(contains, 11);

    private const string doesNotContain = "doesnotcontain";
    public static readonly OperatorEnum DoesNotContain = new(doesNotContain, 12);

    private const string isEmpty = "isempty";
    public static readonly OperatorEnum IsEmpty = new(isEmpty, 13);

    private const string isNotEmpty = "isnotempty";
    public static readonly OperatorEnum IsNotEmpty = new(isNotEmpty, 14);

    public StringOperatorEnum(string name, int value) : base(name, value)
    {
    }
}
