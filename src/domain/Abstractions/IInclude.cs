using System;
using System.Collections;
using System.Linq.Expressions;

namespace domain.Abstractions;

public interface IInclude<T> where T : class
{
    public Expression<Func<T, object?>>[] Expressions { get; }
}
