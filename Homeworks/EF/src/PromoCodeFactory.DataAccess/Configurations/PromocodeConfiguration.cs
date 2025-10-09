using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System.Reflection.Emit;

namespace PromoCodeFactory.DataAccess.Configurations
{
    internal class PromocodeConfiguration : IEntityTypeConfiguration<PromoCode>
    {
        public void Configure(EntityTypeBuilder<PromoCode> builder)
        {
            builder.Property(p => p.Code)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(p => p.ServiceInfo)
                .HasMaxLength(500);

            builder.Property(p => p.PartnerName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.BeginDate)
                .IsRequired();

            builder.Property(p => p.EndDate)
                .IsRequired();

            // Configure many-to-one relationship with Preference
            builder.HasOne(pc => pc.Preference)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // Configure many-to-one relationship with Customer
            builder.HasOne(pc => pc.Customer)
                .WithMany(c => c.PromoCodes)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure many-to-one relationship with Employee (PartnerManager)
            builder.HasOne(pc => pc.PartnerManager)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.Code)
                .IsUnique();
        }
    }
}
