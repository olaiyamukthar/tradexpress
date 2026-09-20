using TradeXpress.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TradeXpress.Business.Common;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Account> Accounts { get; }
    DbSet<AccountUser> AccountUsers { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}