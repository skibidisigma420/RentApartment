using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentApartments.Domain.Entities;
using RentApartments.Domain.ValueObjects;
using RentApartments.Domain.Enums;

namespace RentApartments.Infrastructure.EntityFramework.Configurations
{
    public class RentRequestConfiguration : IEntityTypeConfiguration<RentRequest>
    {
        public void Configure(EntityTypeBuilder<RentRequest> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).IsRequired();

            // Конвертация связанных сущностей через их Id (т.к. Apartment, Tenant, Landlord — сложные объекты)
            builder.Property<Guid>("ApartmentId").IsRequired();
            builder.HasOne(x => x.Apartment)
                .WithMany()
                .HasForeignKey("ApartmentId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property<Guid>("TenantId").IsRequired();
            builder.HasOne(x => x.Tenant)
                .WithMany()
                .HasForeignKey("TenantId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property<Guid>("LandlordId").IsRequired();
            builder.HasOne(x => x.Landlord)
                .WithMany()
                .HasForeignKey("LandlordId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.RequestDate)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<int>(); // Enum в int

            builder.Property(x => x.Message)
                .HasMaxLength(1000)
                .IsRequired(false);
        }
    }
}
