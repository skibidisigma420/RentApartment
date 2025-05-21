using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentApartments.Domain.Entities;
using RentApartments.Domain.ValueObjects;
using RentApartments.Domain.ValueObjects.Validators;

namespace RentApartments.Infrastructure.EntityFramework.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.Username)
                .IsRequired()
                .HasConversion(
                    username => username.Value,
                    str => new Username(str)
                )
                .HasMaxLength(UsernameValidator.MAX_LENGTH);

           
            builder.HasMany<Apartment>("_observableApartments")
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "TenantObservableApartments",
                    j => j.HasOne<Apartment>().WithMany().HasForeignKey("ApartmentId"),
                    j => j.HasOne<Tenant>().WithMany().HasForeignKey("TenantId")
                );

            builder.Ignore(x => x.ObservableApartments);
        }
    }
}
