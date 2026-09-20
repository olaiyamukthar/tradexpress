using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeXpress.Domain.Entities;

namespace TradeXpress.Data.Configurations;

public class AccountUserConfiguration : IEntityTypeConfiguration<AccountUser>
{
    public void Configure(EntityTypeBuilder<AccountUser> builder)
    {
        // configuration goes here
        builder.HasKey(au => new { au.UserId, au.AccountId });
        builder.Property(au => au.Role).HasConversion<string>();
    }
}