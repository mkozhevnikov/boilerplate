using System.ComponentModel;
using Boilerplate.Common.Data.Querying;
using FluentAssertions;
using Xunit;

namespace Boilerplate.Common.UnitTests.Data.Querying;

public class QueryableTests
{
    [Fact]
    public void Queryable_OrderByAscending_Sorted()
    {
        var queryable = CreateQueryable();
        var valuePropertyDescriptor = new TestPropertyDescriptor(nameof(TestValueType.Value), null);
        var sortDescription = new ListSortDescription(valuePropertyDescriptor, ListSortDirection.Ascending);

        var sorted = queryable.OrderBy(sortDescription).ToList();

        sorted.Should().BeInAscendingOrder(vt => vt.Value);
    }

    [Fact]
    public void Queryable_OrderByDescending_Sorted()
    {
        var queryable = CreateQueryable();
        var valuePropertyDescriptor = new TestPropertyDescriptor(nameof(TestValueType.Value), null);
        var sortDescription = new ListSortDescription(valuePropertyDescriptor, ListSortDirection.Descending);

        var sorted = queryable.OrderBy(sortDescription).ToList();

        sorted.Should().BeInDescendingOrder(vt => vt.Value);
    }

    [Fact]
    public void Queryable_MultiOrderBy_UseLastApplied()
    {
        var queryable = CreateQueryable();
        var valuePropertyDescriptor = new TestPropertyDescriptor(nameof(TestValueType.Value), null);
        var sortDescription = new ListSortDescription(valuePropertyDescriptor, ListSortDirection.Ascending);
        var sortDescription2 = new ListSortDescription(valuePropertyDescriptor, ListSortDirection.Descending);

        var sorted = queryable.OrderBy(sortDescription).OrderBy(sortDescription2).ToList();

        sorted.Should().BeInDescendingOrder(vt => vt.Value);
    }

    [Fact]
    public void Queryable_ThenByAscending_Sorted()
    {
        var queryable = CreateQueryable();
        var indexPropertyDescriptor = new TestPropertyDescriptor(nameof(TestValueType.Index), null);
        var indexSortDescription = new ListSortDescription(indexPropertyDescriptor, ListSortDirection.Ascending);

        var sorted = queryable.OrderBy(vt => vt.Value).ThenBy(indexSortDescription).ToList();

        sorted.Should().BeInAscendingOrder(vt => vt.Value).And.ThenBeInAscendingOrder(vt => vt.Index);
    }

    [Fact]
    public void Queryable_ThenByDescending_Sorted()
    {
        var queryable = CreateQueryable();
        var indexPropertyDescriptor = new TestPropertyDescriptor(nameof(TestValueType.Index), null);
        var indexSortDescription = new ListSortDescription(indexPropertyDescriptor, ListSortDirection.Descending);

        var sorted = queryable.OrderBy(vt => vt.Value).ThenBy(indexSortDescription).ToList();

        sorted.Should().BeInAscendingOrder(vt => vt.Value).And.ThenBeInDescendingOrder(vt => vt.Index);
    }

    [Fact]
    public void Queryable_OrderBy_ThenBy_Sorted()
    {
        var queryable = CreateQueryable();
        var valuePropertyDescriptor = new TestPropertyDescriptor(nameof(TestValueType.Value), null);
        var valueSortDescription = new ListSortDescription(valuePropertyDescriptor, ListSortDirection.Ascending);
        var indexPropertyDescriptor = new TestPropertyDescriptor(nameof(TestValueType.Index), null);
        var indexSortDescription = new ListSortDescription(indexPropertyDescriptor, ListSortDirection.Descending);

        var sorted = queryable.OrderBy(valueSortDescription).ThenBy(indexSortDescription).ToList();

        sorted.Should().BeInAscendingOrder(vt => vt.Value).And.ThenBeInDescendingOrder(vt => vt.Index);
    }

    [Fact]
    public void Queryable_Sort_NoDescription()
    {
        var queryable = CreateQueryable();
        var sorting = new ListSortDescriptionCollection();

        var sorted = queryable.Sort(sorting).ToList();

        sorted.Should().NotBeInAscendingOrder(vt => vt.Value);
    }

    [Fact]
    public void Queryable_Sort_OneAscending()
    {
        var queryable = CreateQueryable();
        var valuePropertyDescriptor = new TestPropertyDescriptor(nameof(TestValueType.Value), null);
        var sorting = new ListSortDescriptionCollection(new[] {
            new ListSortDescription(valuePropertyDescriptor, ListSortDirection.Ascending)
        });

        var sorted = queryable.Sort(sorting).ToList();

        sorted.Should().BeInAscendingOrder(vt => vt.Value);
    }

    [Fact]
    public void Queryable_Sort_OneDescending()
    {
        var queryable = CreateQueryable();
        var valuePropertyDescriptor = new TestPropertyDescriptor(nameof(TestValueType.Value), null);
        var sorting = new ListSortDescriptionCollection(new[] {
            new ListSortDescription(valuePropertyDescriptor, ListSortDirection.Descending)
        });

        var sorted = queryable.Sort(sorting).ToList();

        sorted.Should().BeInDescendingOrder(vt => vt.Value);
    }

    [Fact]
    public void Queryable_Sort_TwoAscending()
    {
        var queryable = CreateQueryable();
        var valuePropertyDescriptor = new TestPropertyDescriptor(nameof(TestValueType.Value), null);
        var indexPropertyDescriptor = new TestPropertyDescriptor(nameof(TestValueType.Index), null);
        var sorting = new ListSortDescriptionCollection(new[] {
            new ListSortDescription(valuePropertyDescriptor, ListSortDirection.Ascending),
            new ListSortDescription(indexPropertyDescriptor, ListSortDirection.Ascending)
        });

        var sorted = queryable.Sort(sorting).ToList();

        sorted.Should().BeInAscendingOrder(vt => vt.Value).And.ThenBeInAscendingOrder(vt => vt.Index);
    }

    [Fact]
    public void Queryable_Sort_TwoDescending()
    {
        var queryable = CreateQueryable();
        var valuePropertyDescriptor = new TestPropertyDescriptor(nameof(TestValueType.Value), null);
        var indexPropertyDescriptor = new TestPropertyDescriptor(nameof(TestValueType.Index), null);
        var sorting = new ListSortDescriptionCollection(new[] {
            new ListSortDescription(valuePropertyDescriptor, ListSortDirection.Descending),
            new ListSortDescription(indexPropertyDescriptor, ListSortDirection.Descending)
        });

        var sorted = queryable.Sort(sorting).ToList();

        sorted.Should().BeInDescendingOrder(vt => vt.Value).And.ThenBeInDescendingOrder(vt => vt.Index);
    }

    [Fact]
    public void Queryable_Sort_FirstDescendingThenAscending()
    {
        var queryable = CreateQueryable();
        var valuePropertyDescriptor = new TestPropertyDescriptor(nameof(TestValueType.Value), null);
        var indexPropertyDescriptor = new TestPropertyDescriptor(nameof(TestValueType.Index), null);
        var sorting = new ListSortDescriptionCollection(new[] {
            new ListSortDescription(valuePropertyDescriptor, ListSortDirection.Descending),
            new ListSortDescription(indexPropertyDescriptor, ListSortDirection.Ascending)
        });

        var sorted = queryable.Sort(sorting).ToList();

        sorted.Should().BeInDescendingOrder(vt => vt.Value).And.ThenBeInAscendingOrder(vt => vt.Index);
    }

    private static IQueryable<TestValueType> CreateQueryable()
    {
        var random = new Random();
        var queryable = Enumerable.Range(0, 100)
            .Select(i => new TestValueType(i, random.Next(100)))
            .AsQueryable();
        return queryable;
    }

    private record TestValueType(int Index, int Value);

    private class TestPropertyDescriptor : PropertyDescriptor
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

        public override object? GetValue(object? component) => throw new NotImplementedException();

        public override void ResetValue(object component) => throw new NotImplementedException();

        public override void SetValue(object? component, object? value) => throw new NotImplementedException();

        public override bool ShouldSerializeValue(object component) => throw new NotImplementedException();

        public override Type ComponentType => typeof(TestValueType);
        public override bool IsReadOnly => false;
        public override Type PropertyType => typeof(int);
    }
}
