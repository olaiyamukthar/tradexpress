using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeXpress.Domain.Entities;

namespace TradeXpress.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // configuration goes here
        builder.Property(u => u.Email);
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.FirstName);
        builder.Property(u => u.LastName);
        builder.Property(u => u.PasswordHash);
        builder.Property(u => u.OnboardingStatus).HasConversion<string>();
        builder.Property(u => u.CreatedAt);
    }
}