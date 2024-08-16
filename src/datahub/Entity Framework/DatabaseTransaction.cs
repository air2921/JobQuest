using domain.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace datahub.Entity_Framework;

public class DatabaseTransaction(AppDbContext dbContext) : IDatabaseTransaction
{
    public IDbContextTransaction Begin() => dbContext.Database.BeginTransaction();

    public void Commit(IDbContextTransaction transaction) => transaction.Commit();

    public void Rollback(IDbContextTransaction transaction) => transaction.Rollback();

    public void Dispose(IDbContextTransaction transaction) => transaction.Dispose();
}
