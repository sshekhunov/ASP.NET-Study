using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Reflection.Emit;

namespace PromoCodeFactory.DataAccess.Configurations
{
    internal class CustomerPreferenceConfiguration : IEntityTypeConfiguration<CustomerPreference>
    {
        public void Configure(EntityTypeBuilder<CustomerPreference> builder)
        {
            builder.HasKey(cp => new { cp.CustomerId, cp.PreferenceId });

            builder.HasOne(cp => cp.Customer)
                .WithMany(c => c.Preferences)
                .HasForeignKey(cp => cp.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cp => cp.Preference)
                .WithMany()
                .HasForeignKey(cp => cp.PreferenceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
