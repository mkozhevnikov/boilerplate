using Boilerplate.Common.Data;
using Boilerplate.Common.Data.Querying;
using FluentAssertions;
using Xunit;

namespace Boilerplate.Common.UnitTests.Data.Querying;

public class ListRequestTests
{
    [Fact]
    public void ListRequest_Spec_Default()
    {
        var testValue = new TestValueType(1, 10);
        var request = new TestListRequest();

        var spec = request.ToSpec<TestValueType>();

        spec.Expression.Compile().Invoke(testValue).Should().BeTrue();
    }

    [Fact]
    public void ListRequest_Spec_Default_NullArg()
    {
        var request = new TestListRequest();

        var spec = request.ToSpec<TestValueType>();

        spec.Expression.Compile().Invoke(null!).Should().BeTrue();
    }

    [Fact]
    public void ListRequest_Spec_Filtered()
    {
        var testValue = new TestValueType(1, 10);
        var request = new TestFilteredListRequest {
            Filter = new FilterDescriptor {
                Field = nameof(TestValueType.Value),
                Operator = Operator.In,
                Value = new[] { 10, 20 }
            }
        };

        var spec = request.ToSpec<TestValueType>();

        spec.Expression.Compile().Invoke(testValue).Should().BeTrue();
    }

    [Fact]
    public void ListRequest_Spec_Filtered_Negative()
    {
        var testValue = new TestValueType(1, 10);
        var request = new TestFilteredListRequest {
            Filter = new FilterDescriptor {
                Field = nameof(TestValueType.Value),
                Operator = Operator.In,
                Value = new[] { 20 }
            }
        };

        var spec = request.ToSpec<TestValueType>();

        spec.Expression.Compile().Invoke(testValue).Should().BeFalse();
    }

    [Fact]
    public void ListRequest_Spec_Paged()
    {
        var request = new TestPagedListRequest {
            Skip = 1,
            Take = 3
        };

        var predicate = request.ToSpec<int>();

        predicate.Skip.Should().Be(request.Skip);
        predicate.Take.Should().Be(request.Take);
    }

    [Fact]
    public void ListRequest_Spec_Sorted()
    {
        var indexPropertyDescriptor = TestPropertyDescriptor.Index;
        var sorting = new List<SortingDescriptor> {
            new() {
                Property = indexPropertyDescriptor, Direction = Sort.Ascending
            }
        };
        var request = new TestSortedListRequest {
            Sort = sorting
        };

        var predicate = request.ToSpec<int>();

        predicate.Should().BeAssignableTo<ISortingSpec<int>>();
        ((ISortingSpec<int>)predicate).Sorting.Should().BeEquivalentTo(request.Sort);
    }

    [Fact]
    public void ListRequest_Spec_PagedAndSorted()
    {
        var indexPropertyDescriptor = TestPropertyDescriptor.Index;
        var sorting = new List<SortingDescriptor> {
            new() {
                Property = indexPropertyDescriptor, Direction = Sort.Ascending
            }
        };
        var request = new TestPagedSortedListRequest {
            Skip = 1,
            Take = 3,
            Sort = sorting
        };

        var predicate = request.ToSpec<int>();

        predicate.Skip.Should().Be(request.Skip);
        predicate.Take.Should().Be(request.Take);
        predicate.Should().BeAssignableTo<ISortingSpec<int>>();
        ((ISortingSpec<int>)predicate).Sorting.Should().BeEquivalentTo(request.Sort);
    }
}
