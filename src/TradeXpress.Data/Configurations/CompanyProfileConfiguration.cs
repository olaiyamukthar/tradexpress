using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeXpress.Domain.Entities;

namespace TradeXpress.Data.Configurations;

public class CompanyProfileConfiguration : IEntityTypeConfiguration<CompanyProfile>
{
    public void Configure(EntityTypeBuilder<CompanyProfile> builder)
    {
        builder.Property(cp => cp.AccountId);
        builder.HasIndex(cp => cp.AccountId).IsUnique();
        builder.Property(cp => cp.CompanyName).HasMaxLength(200);
        builder.Property(cp => cp.RegistrationNumber).HasMaxLength(50);
        builder.HasIndex(cp => cp.RegistrationNumber).IsUnique();
        builder.Property(cp => cp.TaxId).HasMaxLength(50);
        builder.Property(cp => cp.RegisteredAddress).HasMaxLength(500);
        builder.Property(cp => cp.CreatedAt);
    }
}
