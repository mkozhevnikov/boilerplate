using Boilerplate.Common.Data.Querying;

namespace Boilerplate.Common.UnitTests.Data.Querying;

internal class TestPropertyDescriptor : PropertyDescriptor
{
    public TestPropertyDescriptor(string name)
    {
        ComponentType = typeof(TestValueType);
        PropertyType = typeof(int);
        Name = name;
    }

    public static TestPropertyDescriptor Index = new(nameof(TestValueType.Index));
    public static TestPropertyDescriptor Value = new(nameof(TestValueType.Value));
}

internal record TestValueType(int Index, int Value);

internal record StringValue(string Value);
