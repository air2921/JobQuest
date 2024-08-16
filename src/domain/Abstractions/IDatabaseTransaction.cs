using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace domain.Abstractions;

public interface IDatabaseTransaction
{
    IDbContextTransaction Begin();
    void Commit(IDbContextTransaction transaction);
    void Rollback(IDbContextTransaction transaction);
    void Dispose(IDbContextTransaction transaction);
}
