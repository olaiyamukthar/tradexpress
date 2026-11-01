using Microsoft.EntityFrameworkCore;
using TradeXpress.Domain.Entities;

namespace TradeXpress.Business.Common;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Account> Accounts { get; }
    DbSet<AccountUser> AccountUsers { get; }
    DbSet<CompanyProfile> CompanyProfiles { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
