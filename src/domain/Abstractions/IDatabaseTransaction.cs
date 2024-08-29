using Microsoft.EntityFrameworkCore.Storage;

namespace domain.Abstractions;

public interface IDatabaseTransaction
{
    IDbContextTransaction Begin();
}
