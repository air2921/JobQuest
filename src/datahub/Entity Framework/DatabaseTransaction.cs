using domain.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace datahub.Entity_Framework;

public class DatabaseTransaction(AppDbContext dbContext) : IDatabaseTransaction
{
    public IDbContextTransaction Begin() => dbContext.Database.BeginTransaction();
}
