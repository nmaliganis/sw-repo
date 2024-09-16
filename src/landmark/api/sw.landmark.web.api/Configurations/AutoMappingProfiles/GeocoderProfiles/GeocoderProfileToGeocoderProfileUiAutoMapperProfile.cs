using AutoMapper;
using sw.landmark.common.dtos.V1.Vms.GeocoderProfiles;
using sw.landmark.model;

namespace sw.landmark.web.api.Configurations.AutoMappingProfiles.GeocoderProfiles
{
    internal class GeocoderProfileToGeocoderProfileUiAutoMapperProfile : Profile
    {
        public GeocoderProfileToGeocoderProfileUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<GeocoderProfile, GeocoderProfileUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.SourceName, opt => opt.MapFrom(src => src.SourceName))
                .ForMember(dest => dest.Params, opt => opt.MapFrom(src => src.Params.RootElement.ToString()))

                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
