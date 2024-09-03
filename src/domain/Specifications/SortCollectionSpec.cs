using Ardalis.Specification;
using System.Linq.Expressions;
using System;

namespace domain.Specifications;

public abstract class SortCollectionSpec<T>(int skip, int count, bool orderByDesc, Expression<Func<T, object?>> expression) 
    : Specification<T> where T : class
{
    public int SkipCount { get; set; } = skip;
    public int Count { get; set; } = count;
    public bool OrderByDesc { get; set; } = orderByDesc;
    private Expression<Func<T, object?>> OrderByExpression { get; } = Expression.Lambda<Func<T, object?>>(
            Expression.Convert(expression.Body, typeof(object)),
            expression.Parameters);

    protected void Initialize()
    {
        if (OrderByDesc)
            Query.OrderByDescending(OrderByExpression);
        else 
            Query.OrderBy(OrderByExpression);

        Query.Skip(SkipCount).Take(Count);
    }
}
