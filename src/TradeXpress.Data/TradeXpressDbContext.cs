using Microsoft.EntityFrameworkCore;
using TradeXpress.Business.Common;
using TradeXpress.Data.Configurations;
using TradeXpress.Domain.Entities;

namespace TradeXpress.Data;

public class TradeXpressDbContext : DbContext, IApplicationDbContext
{
    public TradeXpressDbContext(DbContextOptions<TradeXpressDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new AccountUserConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyProfileConfiguration());
    }

    // DbSet properties go here
    public DbSet<User> Users { get; set; }
    public DbSet<AccountUser> AccountUsers { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<CompanyProfile> CompanyProfiles { get; set; }
}
