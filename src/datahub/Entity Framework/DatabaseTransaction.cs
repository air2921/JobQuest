using domain.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace datahub.Entity_Framework;

public class DatabaseTransaction(AppDbContext dbContext) : IDatabaseTransaction
{
    public async Task<IDbContextTransaction> BeginAsync() => await dbContext.Database.BeginTransactionAsync();

    public async Task CommitAsync(IDbContextTransaction transaction) => await transaction.CommitAsync();

    public async Task RollbackAsync(IDbContextTransaction transaction) => await transaction.RollbackAsync();

    public void Dispose(IDbContextTransaction transaction) => transaction.Dispose();
}
