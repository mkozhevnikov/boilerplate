using System.Linq.Expressions;
using Boilerplate.Common.Data.Querying;

namespace Boilerplate.Common.Data;

/// <summary>
/// An expansion of <see cref="ISpec{T}"/> with sorting description
/// note: inheritance allows FirstOfOrder logic to be implemented with <see cref="IReadRepository{T,TR}.GetBySpec"/>
/// </summary>
public interface ISortingSpec<T> : ISpec<T>
{
    /// <summary>
    /// Sorting description to return objects in specific order
    /// </summary>
    ICollection<SortingDescriptor> Sorting { get; }
}

public class SortingSpec<T> : Spec<T>, ISortingSpec<T>
{
    public ICollection<SortingDescriptor> Sorting { get; set; }

    public SortingSpec(Expression<Func<T, bool>> expression) : base(expression)
    {
    }

    public SortingSpec(ISpec<T> specification) : base(specification.Expression)
    {
        Skip = specification.Skip;
        Take = specification.Take;
    }
}
