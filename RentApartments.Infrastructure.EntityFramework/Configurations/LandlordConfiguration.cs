using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentApartments.Domain.Entities;
using RentApartments.Domain.ValueObjects;
using RentApartments.Domain.ValueObjects.Validators;

namespace RentApartments.Infrastructure.EntityFramework.Configurations
{
    public class LandlordConfiguration : IEntityTypeConfiguration<Landlord>
    {
        public void Configure(EntityTypeBuilder<Landlord> builder)
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

            builder.HasMany<Apartment>("_apartments")
                .WithOne(x => x.Landlord)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Ignore(x => x.ActiveApartments); 
        }
    }

}
