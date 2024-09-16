using AutoMapper;
using sw.landmark.common.dtos.V1.Vms.Landmarks;
using sw.landmark.model;

namespace sw.landmark.web.api.Configurations.AutoMappingProfiles.Landmarks
{
    internal class LandmarkToLandmarkCreationUiModelAutoMapperProfile : Profile
    {
        public LandmarkToLandmarkCreationUiModelAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Landmark, LandmarkCreationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.CodeErp))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.CrossStreet, opt => opt.MapFrom(src => src.CrossStreet))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Prefecture, opt => opt.MapFrom(src => src.Prefecture))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.Zipcode, opt => opt.MapFrom(src => src.Zipcode))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.PhoneNumber2, opt => opt.MapFrom(src => src.PhoneNumber2))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Fax, opt => opt.MapFrom(src => src.Fax))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.PersonInCharge, opt => opt.MapFrom(src => src.PersonInCharge))
                .ForMember(dest => dest.Vat, opt => opt.MapFrom(src => src.Vat))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.IsBase, opt => opt.MapFrom(src => src.IsBase))
                .ForMember(dest => dest.ExcludeFromSpace, opt => opt.MapFrom(src => src.ExcludeFromSpace))
                .ForMember(dest => dest.HasSpacePriority, opt => opt.MapFrom(src => src.HasSpacePriority))
                .ForMember(dest => dest.SpeedLimit, opt => opt.MapFrom(src => src.SpeedLimit))
                .ForMember(dest => dest.Expired, opt => opt.MapFrom(src => src.Expired))
                .ForMember(dest => dest.RootId, opt => opt.MapFrom(src => src.RootId))
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentId))
                .ForMember(dest => dest.LandmarkCategoryId, opt => opt.MapFrom(src => src.LandmarkCategoryId))

                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
