using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System.Reflection.Emit;

namespace PromoCodeFactory.DataAccess.Configurations
{
    internal class PreferenceConfiguration : IEntityTypeConfiguration<Preference>
    {
        public void Configure(EntityTypeBuilder<Preference> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(p => p.Name)
                .IsUnique();

            builder.HasMany<CustomerPreference>()
                .WithOne(cp => cp.Preference)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
