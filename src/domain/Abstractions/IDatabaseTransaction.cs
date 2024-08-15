using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace domain.Abstractions;

public interface IDatabaseTransaction
{
    Task<IDbContextTransaction> BeginAsync();
    Task CommitAsync(IDbContextTransaction transaction);
    Task RollbackAsync(IDbContextTransaction transaction);
    void Dispose(IDbContextTransaction transaction);
}
