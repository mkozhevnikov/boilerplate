using Ardalis.SmartEnum;

namespace Boilerplate.Common.Data.Querying;

public sealed class LogicEnum : SmartEnum<LogicEnum>
{
    private const string or = nameof(or);
    public static readonly LogicEnum Or = new(or, 1);

    private const string and = nameof(and);
    public static readonly LogicEnum And = new(and, 2);

    public LogicEnum(string name, int value) : base(name, value)
    {
    }
}
