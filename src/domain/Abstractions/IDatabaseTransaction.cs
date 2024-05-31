using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace domain.Abstractions
{
    public interface IDatabaseTransaction
    {
        public Task<IDbContextTransaction> BeginAsync();
        public Task CommitAsync(IDbContextTransaction transaction);
        public Task RollbackAsync(IDbContextTransaction transaction);
        public void Dispose(IDbContextTransaction transaction);
    }
}
