using System.ComponentModel;
using System.Linq.Expressions;

namespace Boilerplate.Common.Data;

/// <summary>
/// An expansion of <see cref="ISpec{T}"/> with sorting description
/// note: inheritance allows FirstOfOrder logic to be implemented with <see cref="IReadRepository{T,TR}.GetBySpec"/>
/// </summary>
public interface ISortedSpec<T> : ISpec<T>
{
    /// <summary>
    /// Sorting description to return objects in specific order
    /// </summary>
    ListSortDescriptionCollection Sort { get; }
}

public class SortedSpec<T> : Spec<T>, ISortedSpec<T>
{
    public ListSortDescriptionCollection Sort { get; set; }

    public SortedSpec(Expression<Func<T, bool>> expression) : base(expression)
    {
    }

    public SortedSpec(ISpec<T> specification) : base(specification.Expression)
    {
        Skip = specification.Skip;
        Take = specification.Take;
    }
}
