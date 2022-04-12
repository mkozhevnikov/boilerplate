using System.ComponentModel;
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
        var request = new TestFilteredRequest {
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
        var request = new TestFilteredRequest {
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
        var request = new TestPagedRequest {
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
        var indexPropertyDescriptor = new TestPropertyDescriptor(nameof(TestValueType.Index), null);
        var request = new TestSortedRequest {
            Sort = new ListSortDescriptionCollection(new[] {
                new ListSortDescription(indexPropertyDescriptor, ListSortDirection.Ascending)
            })
        };

        var predicate = request.ToSpec<int>();

        predicate.Should().BeAssignableTo<ISortedSpec<int>>();
        ((ISortedSpec<int>)predicate).Sort.Should().BeEquivalentTo(request.Sort);
    }

    [Fact]
    public void ListRequest_Spec_PagedAndSorted()
    {
        var indexPropertyDescriptor = new TestPropertyDescriptor(nameof(TestValueType.Index), null);
        var request = new TestPagedSortedRequest {
            Skip = 1,
            Take = 3,
            Sort = new ListSortDescriptionCollection(new[] {
                new ListSortDescription(indexPropertyDescriptor, ListSortDirection.Ascending)
            })
        };

        var predicate = request.ToSpec<int>();

        predicate.Skip.Should().Be(request.Skip);
        predicate.Take.Should().Be(request.Take);
        predicate.Should().BeAssignableTo<ISortedSpec<int>>();
        ((ISortedSpec<int>)predicate).Sort.Should().BeEquivalentTo(request.Sort);
    }

    private class TestListRequest : IListRequest
    {
    }

    private class TestFilteredRequest : IFilteredListRequest
    {
        public FilterDescriptor Filter { get; set; } = null!;
    }

    private class TestPagedRequest : IPagedListRequest
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    private class TestSortedRequest : ISortedListRequest
    {
        public ListSortDescriptionCollection Sort { get; set; } = null!;
    }

    private class TestPagedSortedRequest : IPagedListRequest, ISortedListRequest
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public ListSortDescriptionCollection Sort { get; set; } = null!;
    }
}
