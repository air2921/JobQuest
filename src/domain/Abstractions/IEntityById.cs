using Ardalis.Specification;

namespace domain.Abstractions;

public interface IEntityById<T> : ISpecification<T> where T : class
{
    public int Id { get; set; }
}
