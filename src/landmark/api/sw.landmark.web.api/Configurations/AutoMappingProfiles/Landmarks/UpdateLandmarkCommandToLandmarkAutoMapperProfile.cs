using AutoMapper;
using sw.landmark.common.dtos.V1.Cqrs.Landmarks;
using sw.landmark.model;

namespace sw.landmark.web.api.Configurations.AutoMappingProfiles.Landmarks
{
    internal class UpdateLandmarkCommandToLandmarkAutoMapperProfile : Profile
    {
        public UpdateLandmarkCommandToLandmarkAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<UpdateLandmarkCommand, Landmark>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Parameters.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Parameters.Description))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.Parameters.CodeErp))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Parameters.Street))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Parameters.Number))
                .ForMember(dest => dest.CrossStreet, opt => opt.MapFrom(src => src.Parameters.CrossStreet))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Parameters.City))
                .ForMember(dest => dest.Prefecture, opt => opt.MapFrom(src => src.Parameters.Prefecture))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Parameters.Country))
                .ForMember(dest => dest.Zipcode, opt => opt.MapFrom(src => src.Parameters.Zipcode))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Parameters.PhoneNumber))
                .ForMember(dest => dest.PhoneNumber2, opt => opt.MapFrom(src => src.Parameters.PhoneNumber2))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Parameters.Email))
                .ForMember(dest => dest.Fax, opt => opt.MapFrom(src => src.Parameters.Fax))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Parameters.Url))
                .ForMember(dest => dest.PersonInCharge, opt => opt.MapFrom(src => src.Parameters.PersonInCharge))
                .ForMember(dest => dest.Vat, opt => opt.MapFrom(src => src.Parameters.Vat))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Parameters.Image))
                .ForMember(dest => dest.IsBase, opt => opt.MapFrom(src => src.Parameters.IsBase))
                .ForMember(dest => dest.ExcludeFromSpace, opt => opt.MapFrom(src => src.Parameters.ExcludeFromSpace))
                .ForMember(dest => dest.HasSpacePriority, opt => opt.MapFrom(src => src.Parameters.HasSpacePriority))
                .ForMember(dest => dest.SpeedLimit, opt => opt.MapFrom(src => src.Parameters.SpeedLimit))
                .ForMember(dest => dest.Expired, opt => opt.MapFrom(src => src.Parameters.Expired))
                .ForMember(dest => dest.RootId, opt => opt.MapFrom(src => src.Parameters.RootId))
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.Parameters.ParentId))
                .ForMember(dest => dest.LandmarkCategoryId, opt => opt.MapFrom(src => src.Parameters.LandmarkCategoryId))

                .MaxDepth(1);
        }
    }
}
