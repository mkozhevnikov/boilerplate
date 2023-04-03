using System.ComponentModel;

namespace Boilerplate.Common.UnitTests.Data.Querying;

internal class TestPropertyDescriptor : PropertyDescriptor
{
    public TestPropertyDescriptor(MemberDescriptor descr) : base(descr)
    {
    }

    public TestPropertyDescriptor(MemberDescriptor descr, Attribute[]? attrs) : base(descr, attrs)
    {
    }

    public TestPropertyDescriptor(string name, Attribute[]? attrs) : base(name, attrs)
    {
    }

    public override bool CanResetValue(object component) => throw new NotImplementedException();

    public override object GetValue(object? component) => throw new NotImplementedException();

    public override void ResetValue(object component) => throw new NotImplementedException();

    public override void SetValue(object? component, object? value) => throw new NotImplementedException();

    public override bool ShouldSerializeValue(object component) => throw new NotImplementedException();

    public override Type ComponentType => typeof(TestValueType);
    public override bool IsReadOnly => false;
    public override Type PropertyType => typeof(int);
}

internal record TestValueType(int Index, int Value);

internal record StringValue(string Value);
