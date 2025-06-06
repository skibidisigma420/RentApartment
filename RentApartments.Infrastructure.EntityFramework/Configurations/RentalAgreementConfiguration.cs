﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentApartments.Domain.Entities;
using RentApartments.Domain.ValueObjects;

namespace RentApartments.Infrastructure.EntityFramework.Configurations
{
    public class RentalAgreementConfiguration : IEntityTypeConfiguration<RentalAgreement>
    {
        public void Configure(EntityTypeBuilder<RentalAgreement> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).IsRequired();

           
            builder.Property(x => x.MonthlyRent)
                .IsRequired()
                .HasConversion(
                    money => money.Value,
                    value => new Money(value)
                );

           
            builder.Property(x => x.CreationDate)
                .IsRequired()
                .HasConversion(
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                );

            builder.Property(x => x.StartDate)
                .IsRequired()
                .HasConversion(
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                );

            builder.Property(x => x.EndDate)
                .IsRequired(false)
                .HasConversion(
                    src => src.HasValue
                        ? (src.Value.Kind == DateTimeKind.Utc ? src.Value : DateTime.SpecifyKind(src.Value, DateTimeKind.Utc))
                        : (DateTime?)null,
                    dst => dst.HasValue
                        ? (dst.Value.Kind == DateTimeKind.Utc ? dst.Value : DateTime.SpecifyKind(dst.Value, DateTimeKind.Utc))
                        : (DateTime?)null
                );
           
            builder.HasOne(x => x.Apartment)
                .WithMany() 
                .IsRequired();

            builder.HasOne(x => x.Tenant)
                .WithMany() 
                .IsRequired();

            builder.HasOne(x => x.Landlord)
                .WithMany() 
                .IsRequired();

            builder.Ignore(x => x.IsActive);
        }
    }
}
