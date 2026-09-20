using Microsoft.EntityFrameworkCore;
using TradeXpress.Domain.Entities;
using TradeXpress.Data.Configurations;

namespace TradeXpress.Data;

public class TradeXpressDbContext : DbContext
{
    public TradeXpressDbContext(DbContextOptions<TradeXpressDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new AccountUserConfiguration());
    }

    // DbSet properties go here
    public DbSet<User> Users { get; set; }
    public DbSet<AccountUser> AccountUsers { get; set; }
    public DbSet<Account> Accounts { get; set; }
}