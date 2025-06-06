﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentApartments.Domain.Entities;
using RentApartments.Domain.ValueObjects;
using RentApartments.Domain.ValueObjects.Validators; 
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
                .HasConversion(title => title.Value, str => new Title(str))
                .HasMaxLength(TitleValidator.MAX_LENGTH);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasConversion(desc => desc.Value, str => new Description(str));

            builder.Property(x => x.Address)
                .IsRequired()
                .HasConversion(addr => addr.Value, str => new Address(str));

            builder.Property(x => x.MonthlyRent)
                .IsRequired()
                .HasConversion(rent => rent.Value, value => new Money(value));

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<int>(); 

            builder.HasOne(x => x.Landlord).WithMany()
                .IsRequired();

            builder.HasMany<RentRequest>("_rentRequests").WithOne(x => x.Apartment).OnDelete(DeleteBehavior.Cascade);

            builder.Ignore(x => x.LastRequest);
            builder.Ignore(x => x.IsAvailable);
        }
    }
}
