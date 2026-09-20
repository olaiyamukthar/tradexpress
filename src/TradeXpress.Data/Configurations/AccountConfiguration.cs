using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeXpress.Domain.Entities;

namespace TradeXpress.Data.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        // configuration goes here
        builder.Property(a => a.KycTier);
        builder.Property(a => a.AccountType).HasConversion<string>();
        builder.Property(a => a.ComplianceStatus).HasConversion<string>();
        builder.Property(a => a.CreatedAt);
    }
}