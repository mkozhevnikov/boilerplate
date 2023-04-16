using Boilerplate.Common.Data.Querying;

namespace Boilerplate.Common.UnitTests.Data.Querying;

public class QueryableTests
{
    [Fact]
    public void Queryable_OrderByAscending_Sorted()
    {
        var queryable = CreateQueryable();
        var valuePropertyDescriptor = TestPropertyDescriptor.Value;

        var sorted = queryable.OrderBy(valuePropertyDescriptor, Sort.Ascending).ToList();

        sorted.Should().BeInAscendingOrder(vt => vt.Value);
    }

    [Fact]
    public void Queryable_OrderByDescending_Sorted()
    {
        var queryable = CreateQueryable();
        var valuePropertyDescriptor = TestPropertyDescriptor.Value;

        var sorted = queryable.OrderBy(valuePropertyDescriptor, Sort.Descending).ToList();

        sorted.Should().BeInDescendingOrder(vt => vt.Value);
    }

    [Fact]
    public void Queryable_MultiOrderBy_UseLastApplied()
    {
        var queryable = CreateQueryable();
        var valuePropertyDescriptor = TestPropertyDescriptor.Value;

        var sorted = queryable
            .OrderBy(valuePropertyDescriptor, Sort.Ascending)
            .OrderBy(valuePropertyDescriptor, Sort.Descending)
            .ToList();

        sorted.Should().BeInDescendingOrder(vt => vt.Value);
    }

    [Fact]
    public void Queryable_ThenByAscending_Sorted()
    {
        var queryable = CreateQueryable();
        var indexPropertyDescriptor = TestPropertyDescriptor.Index;

        var sorted = queryable.OrderBy(vt => vt.Value).ThenBy(indexPropertyDescriptor, Sort.Ascending).ToList();

        sorted.Should().BeInAscendingOrder(vt => vt.Value).And.ThenBeInAscendingOrder(vt => vt.Index);
    }

    [Fact]
    public void Queryable_ThenByDescending_Sorted()
    {
        var queryable = CreateQueryable();
        var indexPropertyDescriptor = TestPropertyDescriptor.Index;

        var sorted = queryable.OrderBy(vt => vt.Value).ThenBy(indexPropertyDescriptor, Sort.Descending).ToList();

        sorted.Should().BeInAscendingOrder(vt => vt.Value).And.ThenBeInDescendingOrder(vt => vt.Index);
    }

    [Fact]
    public void Queryable_OrderBy_ThenBy_Sorted()
    {
        var queryable = CreateQueryable();
        var valuePropertyDescriptor = TestPropertyDescriptor.Value;
        var indexPropertyDescriptor = TestPropertyDescriptor.Index;

        var sorted = queryable
            .OrderBy(valuePropertyDescriptor, Sort.Ascending)
            .ThenBy(indexPropertyDescriptor, Sort.Descending)
            .ToList();

        sorted.Should().BeInAscendingOrder(vt => vt.Value).And.ThenBeInDescendingOrder(vt => vt.Index);
    }

    [Fact]
    public void Queryable_Sort_NoDescription()
    {
        var queryable = CreateQueryable();

        var sorted = queryable.Sort(new List<SortingDescriptor>()).ToList();

        sorted.Should().NotBeInAscendingOrder(vt => vt.Value);
    }

    [Fact]
    public void Queryable_Sort_OneAscending()
    {
        var queryable = CreateQueryable();
        var valuePropertyDescriptor = TestPropertyDescriptor.Value;
        var sorting = new List<SortingDescriptor> {
            new() {
                Property = valuePropertyDescriptor, Direction = Sort.Ascending
            }
        };

        var sorted = queryable.Sort(sorting).ToList();

        sorted.Should().BeInAscendingOrder(vt => vt.Value);
    }

    [Fact]
    public void Queryable_Sort_OneDescending()
    {
        var queryable = CreateQueryable();
        var valuePropertyDescriptor = TestPropertyDescriptor.Value;
        var sorting = new List<SortingDescriptor> {
            new() {
                Property = valuePropertyDescriptor, Direction = Sort.Descending
            }
        };

        var sorted = queryable.Sort(sorting).ToList();

        sorted.Should().BeInDescendingOrder(vt => vt.Value);
    }

    [Fact]
    public void Queryable_Sort_TwoAscending()
    {
        var queryable = CreateQueryable();
        var valuePropertyDescriptor = TestPropertyDescriptor.Value;
        var indexPropertyDescriptor = TestPropertyDescriptor.Index;
        var sorting = new List<SortingDescriptor> {
            new() { Property = valuePropertyDescriptor, Direction = Sort.Ascending },
            new() { Property = indexPropertyDescriptor, Direction = Sort.Ascending }
        };

        var sorted = queryable.Sort(sorting).ToList();

        sorted.Should().BeInAscendingOrder(vt => vt.Value).And.ThenBeInAscendingOrder(vt => vt.Index);
    }

    [Fact]
    public void Queryable_Sort_TwoDescending()
    {
        var queryable = CreateQueryable();
        var valuePropertyDescriptor = TestPropertyDescriptor.Value;
        var indexPropertyDescriptor = TestPropertyDescriptor.Index;
        var sorting = new List<SortingDescriptor> {
            new() { Property = valuePropertyDescriptor, Direction = Sort.Descending },
            new() { Property = indexPropertyDescriptor, Direction = Sort.Descending }
        };

        var sorted = queryable.Sort(sorting).ToList();

        sorted.Should().BeInDescendingOrder(vt => vt.Value).And.ThenBeInDescendingOrder(vt => vt.Index);
    }

    [Fact]
    public void Queryable_Sort_FirstDescendingThenAscending()
    {
        var queryable = CreateQueryable();
        var valuePropertyDescriptor = TestPropertyDescriptor.Value;
        var indexPropertyDescriptor = TestPropertyDescriptor.Index;
        var sorting = new List<SortingDescriptor> {
            new() { Property = valuePropertyDescriptor, Direction = Sort.Descending },
            new() { Property = indexPropertyDescriptor, Direction = Sort.Ascending }
        };

        var sorted = queryable.Sort(sorting).ToList();

        sorted.Should().BeInDescendingOrder(vt => vt.Value).And.ThenBeInAscendingOrder(vt => vt.Index);
    }

    [Fact]
    public void Queryable_Sort_FirstAscendingThenDescending()
    {
        var queryable = CreateQueryable();
        var valuePropertyDescriptor = TestPropertyDescriptor.Value;
        var indexPropertyDescriptor = TestPropertyDescriptor.Index;
        var sorting = new List<SortingDescriptor> {
            new() { Property = valuePropertyDescriptor, Direction = Sort.Ascending },
            new() { Property = indexPropertyDescriptor, Direction = Sort.Descending }
        };

        var sorted = queryable.Sort(sorting).ToList();

        sorted.Should().BeInAscendingOrder(vt => vt.Value).And.ThenBeInDescendingOrder(vt => vt.Index);
    }

    private static IQueryable<TestValueType> CreateQueryable()
    {
        var random = new Random();
        var queryable = Enumerable.Range(0, 100)
            .Select(i => new TestValueType(i, random.Next(100)))
            .AsQueryable();
        return queryable;
    }
}
