using Ardalis.Specification;
using System;
using System.Linq.Expressions;

namespace domain.Specifications;

public abstract class IncludeSpec<T> : Specification<T> where T : class
{
    protected void IncludeEntities(Expression<Func<T, object?>>[] expressions)
    {
        if (expressions.Length <= 0)
            return;

        foreach (var expression in expressions)
        {
            var convertedExpression = Expression.Lambda<Func<T, object?>>(
                Expression.Convert(expression.Body, typeof(object)),
                expression.Parameters);

            Query.Include(convertedExpression);
        }
    }
}
