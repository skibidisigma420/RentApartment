using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentApartments.Domain.Entities;
using RentApartments.Domain.ValueObjects;
using RentApartments.Domain.Enums;

namespace RentApartments.Infrastructure.EntityFramework.Configurations
{
    public class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
    {
        public void Configure(EntityTypeBuilder<Apartment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasConversion(title => title.Value, str => new Title(str));

            builder.Property(x => x.Description)
                .IsRequired()
                .HasConversion(desc => desc.Value, str => new Description(str));

            builder.Property(x => x.Address)
                .IsRequired()
                .HasConversion(address => address.Value, str => new Address(str));

            builder.Property(x => x.MonthlyRent)
                .IsRequired()
                .HasConversion(rent => rent.Value, value => new Money(value));

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.HasOne(x => x.Landlord)
                .WithMany(x => x.ActiveApartments) 
                .IsRequired();

            builder.HasMany<RentRequest>("_rentRequests")
                .WithOne(x => x.Apartment)
                .OnDelete(DeleteBehavior.Cascade); 

            // Игнорируем вычисляемые свойства
            builder.Ignore(x => x.LastRequest);
            builder.Ignore(x => x.IsAvailable);
        }
    }
}
