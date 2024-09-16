using AutoMapper;
using sw.landmark.common.dtos.V1.Cqrs.GeocoderProfiles;
using sw.landmark.model;
using System.Text.Json;

namespace sw.landmark.web.api.Configurations.AutoMappingProfiles.GeocoderProfiles
{
    internal class CreateGeocoderProfileCommandToGeocoderProfileAutoMapperProfile : Profile
    {
        public CreateGeocoderProfileCommandToGeocoderProfileAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<CreateGeocoderProfileCommand, GeocoderProfile>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Parameters.Name))
                .ForMember(dest => dest.SourceName, opt => opt.MapFrom(src => src.Parameters.SourceName))
                .ForMember(dest => dest.Params, opt => opt.MapFrom<CreateJsonResolver>())

                .MaxDepth(1);
        }

        internal class CreateJsonResolver : IValueResolver<CreateGeocoderProfileCommand, GeocoderProfile, JsonDocument>
        {
            public JsonDocument Resolve(CreateGeocoderProfileCommand source, GeocoderProfile destination, JsonDocument member, ResolutionContext context)
            {
                return JsonDocument.Parse(source.Parameters.Params);
            }
        }
    }
}
