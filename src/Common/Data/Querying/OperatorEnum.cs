using Ardalis.SmartEnum;

namespace Boilerplate.Common.Data.Querying;

public class OperatorEnum : SmartEnum<OperatorEnum>
{
    private const string equalTo = "eq";
    public static readonly OperatorEnum EqualTo = new(equalTo, 1);

    private const string notEqual = "neq";
    public static readonly OperatorEnum NotEqualTo = new(notEqual, 2);

    private const string isNull = "isnull";
    public static readonly OperatorEnum IsNull = new(isNull, 3);

    private const string isNotNull = "isnotnull";
    public static readonly OperatorEnum IsNotNull = new(isNotNull, 4);

    private const string lessThan = "lt";
    public static readonly OperatorEnum LessThan = new(lessThan, 5);

    private const string lessThanOrEqualTo = "lte";
    public static readonly OperatorEnum LessThanOrEqualTo = new(lessThanOrEqualTo, 6);

    private const string greaterThan = "gt";
    public static readonly OperatorEnum GreaterThan = new(greaterThan, 7);

    private const string greaterThanOrEqualTo = "gte";
    public static readonly OperatorEnum GreaterThanOrEqualTo = new(greaterThanOrEqualTo, 8);

    public OperatorEnum(string name, int value) : base(name, value)
    {
    }
}
