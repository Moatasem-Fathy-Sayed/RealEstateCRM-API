using AutoMapper;
using RealEstateCRM.Core.DTOs;
using RealEstateCRM.Core.Entities;

namespace RealEstateCRM.API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Map Property entity to PropertyDto and map Enums to strings
            CreateMap<Property, PropertyDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // Map CreatePropertyDto input model to Property domain entity
            CreateMap<CreatePropertyDto, Property>();

            // Map Lead entity to LeadDto
            CreateMap<Lead, LeadDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // Map CreateLeadDto to Lead entity
            CreateMap<CreateLeadDto, Lead>();

            // Map InteractionLog entity to InteractionLogDto
            CreateMap<InteractionLog, InteractionLogDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

            // Map CreateInteractionLogDto to InteractionLog entity
            CreateMap<CreateInteractionLogDto, InteractionLog>();
            // Deals Mapping
            CreateMap<Deal, DealDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<CreateDealDto, Deal>();

            // Appointments Mapping
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<CreateAppointmentDto, Appointment>();
        }
    }
}