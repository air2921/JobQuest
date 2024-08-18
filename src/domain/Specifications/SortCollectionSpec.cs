using Ardalis.Specification;
using System.Linq.Expressions;
using System;

namespace domain.Specifications;

public abstract class SortCollectionSpec<T> : IncludeSpec<T> where T : class
{
    protected SortCollectionSpec(int skip, int count, bool orderByDesc, Expression<Func<T, object?>> expression)
    {
        SkipCount = skip;
        Count = count;
        OrderByDesc = orderByDesc;

        OrderByExpression = Expression.Lambda<Func<T, object?>>(
            Expression.Convert(expression.Body, typeof(object)),
            expression.Parameters);
    }

    public int SkipCount { get; set; }
    public int Count { get; set; }
    public bool OrderByDesc { get; set; }
    public Expression<Func<T, object?>>[]? Expressions { get; set; }
    private Expression<Func<T, object?>> OrderByExpression { get; }

    protected void Initialize()
    {
        if (OrderByDesc)
            Query.OrderByDescending(OrderByExpression);
        else 
            Query.OrderBy(OrderByExpression);

        Query.Skip(SkipCount).Take(Count);

        if (Expressions is not null)
            IncludeEntities(Expressions);
    }
}
