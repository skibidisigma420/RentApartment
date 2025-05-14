using AutoMapper;
using RentApartments.Application.Models.Apartment;
using RentApartments.Application.Models.Tenant;
using RentApartments.Application.Models.RentRequest;
using RentApartments.Application.Models.RentalAgreement;
using RentApartments.Domain.Entities;
using RentApartments.Domain.ValueObjects;
using RentApartments.Application.Models.Landlord;

namespace RentApartments.Application.Services.Mapping
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            // Apartment → ApartmentModel
            CreateMap<Apartment, ApartmentModel>()
                 .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Value))
                 .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Value))
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address.Value))
                 .ForMember(dest => dest.MonthlyRent, opt => opt.MapFrom(src => src.MonthlyRent.Value))
                 .ForMember(dest => dest.LandlordId, opt => opt.MapFrom(src => src.Landlord.Id))
                 .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable));

            // Tenant → TenantModel
            CreateMap<Tenant, TenantModel>()
                 .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username.Value))
                 .ForMember(dest => dest.ObservableApartmentIds, opt => opt.MapFrom(
                     src => src.ObservableApartments.Select(a => a.Id)));

            // LandLord → LandLordModel
            CreateMap<Landlord, LandlordModel>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username.Value))
                .ForMember(dest => dest.ActiveApartmentIds, opt => opt.MapFrom(src =>
                    src.ActiveApartments.Select(a => a.Id).ToList()));

            // RentRequest → RentRequestModel
            CreateMap<RentRequest, RentRequestModel>()
                 .ForMember(dest => dest.ApartmentId, opt => opt.MapFrom(src => src.Apartment.Id))
                 .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.Tenant.Id))
                 .ForMember(dest => dest.LandlordId, opt => opt.MapFrom(src => src.Landlord.Id))
                 .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // RentalAgreement → RentalAgreementModel
            CreateMap<RentalAgreement, RentalAgreementModel>()
                .ForMember(dest => dest.ApartmentId, opt => opt.MapFrom(src => src.Apartment.Id))
                .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.Tenant.Id))
                .ForMember(dest => dest.LandlordId, opt => opt.MapFrom(src => src.Landlord.Id))
                .ForMember(dest => dest.MonthlyRent, opt => opt.MapFrom(src => src.MonthlyRent.Value))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
        }
    }
}
