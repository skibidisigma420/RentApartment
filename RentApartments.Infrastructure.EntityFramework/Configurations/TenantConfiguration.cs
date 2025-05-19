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

            // Конфигурация связи "многие ко многим" (или "многие ко многим" с навигацией)
            builder.HasMany<Apartment>("_observableApartments")
                .WithMany()
                .UsingEntity(j => j.ToTable("TenantObservableApartments"));

            builder.Ignore(x => x.ObservableApartments);
        }
    }
}
