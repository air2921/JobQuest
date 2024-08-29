using Ardalis.Specification;
using System;
using System.Linq.Expressions;

namespace domain.Abstractions;

public interface IEntityById<T> : ISpecification<T> where T : class
{
    public int Id { get; set; }
    public Expression<Func<T, object?>>[]? Expressions { get; set; }
}
