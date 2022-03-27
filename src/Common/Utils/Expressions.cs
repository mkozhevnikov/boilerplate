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

        public Expression ReplaceParameter(Expression expr, ParameterExpression oldParam, ParameterExpression newParam)
        {
            this.oldParam = oldParam;
            this.newParam = newParam;
            return base.Visit(expr);
        }
    }

    public static Expression<Func<T, bool>> Build<T>(Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2, Func<Expression, Expression, BinaryExpression> operation)
    {
        var visitor = new ParameterExpressionVisitor();
        var result = operation(expr1.Body, visitor.ReplaceParameter(expr2.Body, expr2.Parameters[0], expr1.Parameters[0]));
        return Expression.Lambda<Func<T, bool>>(result, expr1.Parameters[0]);
    }

    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2) => Build(expr1, expr2, Expression.AndAlso);

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2) => Build(expr1, expr2, Expression.OrElse);
}
