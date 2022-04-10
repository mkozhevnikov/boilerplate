using Boilerplate.Common.Data.Querying;

namespace Boilerplate.Common.Utils;

using System.Linq.Expressions;

public static class Expressions
{
    private class ParameterExpressionVisitor : ExpressionVisitor
    {
        private ParameterExpression oldParam = null!;
        private ParameterExpression newParam = null!;

        protected override Expression VisitParameter(ParameterExpression node) =>
            ReferenceEquals(node, oldParam) ? newParam : base.VisitParameter(node);

        public Expression ReplaceParameter(Expression expr, ParameterExpression left, ParameterExpression right)
        {
            oldParam = left;
            newParam = right;
            return base.Visit(expr);
        }
    }

    public static Expression<Func<T, bool>> Build<T>(
        Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2,
        Func<Expression, Expression, BinaryExpression> operation)
    {
        var visitor = new ParameterExpressionVisitor();
        var result = operation(expr1.Body,
            visitor.ReplaceParameter(expr2.Body, expr2.Parameters[0], expr1.Parameters[0]));
        return Expression.Lambda<Func<T, bool>>(result, expr1.Parameters[0]);
    }

    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2) =>
        Build(expr1, expr2, Expression.AndAlso);

    public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2) =>
        Build(expr1, expr2, Expression.OrElse);

    public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expr)
    {
        var negated = Expression.Not(expr.Body);
        return Expression.Lambda<Func<T, bool>>(negated, expr.Parameters);
    }

    public static Expression<Func<T, bool>> ToExpression<T>(this FilterDescriptor descriptor)
    {
        if (descriptor is CompositeFilterDescriptor compositeDescriptor) {
            return compositeDescriptor.ToExpression<T>();
        }

        var param = Expression.Parameter(typeof(T));
        var property = Expression.Property(param, descriptor.Field);
        var valueParam = Expression.Constant(descriptor.Value);
        var expression = descriptor.Operator.CreateExpression(property, valueParam);

        return Expression.Lambda<Func<T, bool>>(expression, param);
    }

    public static Expression<Func<T, bool>> ToExpression<T>(this CompositeFilterDescriptor descriptor)
    {
        if (!descriptor.Filters.Any()) {
            return _ => true;
        }

        return descriptor.Filters
            .Select(ToExpression<T>)
            .Aggregate((prev, next) => descriptor.Logic.CreateExpression(prev, next));
    }
}
