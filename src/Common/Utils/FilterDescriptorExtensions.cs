using System.Linq.Expressions;
using System.Text.Json;
using Boilerplate.Common.Data.Querying;

namespace Boilerplate.Common.Utils;

public static class FilterDescriptorExtensions
{
    /// <summary>
    /// Converts simple or compound filter descriptor into expression
    /// </summary>
    /// <param name="descriptor">Root node of filter descriptors</param>
    /// <typeparam name="T">Type of entity to which filter is applied</typeparam>
    /// <returns>Filter expression matching tree of descriptors</returns>
    public static Expression<Func<T, bool>> ToExpression<T>(this FilterDescriptor descriptor)
    {
        if (descriptor is CompositeFilterDescriptor compositeDescriptor) {
            return compositeDescriptor.ToExpression<T>();
        }

        var param = Expression.Parameter(typeof(T));
        var property = Expression.Property(param, descriptor.Field);
        var extractedValue = descriptor.ExtractValue(property.GetMemberReturnType());
        var valueParam = Expression.Constant(extractedValue);
        var expression = descriptor.Operator.CreateExpression(property, valueParam);

        return Expression.Lambda<Func<T, bool>>(expression, param);
    }

    /// <summary>
    /// Supporting method for <see cref="ToExpression{T}(Boilerplate.Common.Data.Querying.FilterDescriptor)">ToExpression`T</see>
    /// <br/>
    /// Converts compound filter descriptor into expression
    /// </summary>
    /// <param name="descriptor">Root node of filter descriptors</param>
    /// <typeparam name="T">Type of entity to which filter is applied</typeparam>
    /// <returns>Filter expression matching tree of descriptors</returns>
    public static Expression<Func<T, bool>> ToExpression<T>(this CompositeFilterDescriptor descriptor)
    {
        if (!descriptor.Filters.Any()) {
            return _ => true;
        }

        return descriptor.Filters
            .Select(ToExpression<T>)
            .Aggregate((prev, next) => descriptor.Logic.CreateExpression(prev, next));
    }

    private static object? ExtractValue(this FilterDescriptor descriptor, Type returnType)
    {
        return descriptor.Value is JsonElement jsonElement
            ? jsonElement.Deserialize(returnType)
            : descriptor.Value;
    }
}
