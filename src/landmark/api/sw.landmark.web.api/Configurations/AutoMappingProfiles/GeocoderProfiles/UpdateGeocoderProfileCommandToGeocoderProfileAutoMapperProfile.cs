using AutoMapper;
using sw.landmark.common.dtos.V1.Cqrs.GeocoderProfiles;
using sw.landmark.model;
using System.Text.Json;

namespace sw.landmark.web.api.Configurations.AutoMappingProfiles.GeocoderProfiles
{
    internal class UpdateGeocoderProfileCommandToGeocoderProfileAutoMapperProfile : Profile
    {
        public UpdateGeocoderProfileCommandToGeocoderProfileAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<UpdateGeocoderProfileCommand, GeocoderProfile>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Parameters.Name))
                .ForMember(dest => dest.SourceName, opt => opt.MapFrom(src => src.Parameters.SourceName))
                .ForMember(dest => dest.Params, opt => opt.MapFrom<CreateJsonResolver>())

                .MaxDepth(1);
        }

        internal class CreateJsonResolver : IValueResolver<UpdateGeocoderProfileCommand, GeocoderProfile, JsonDocument>
        {
            public JsonDocument Resolve(UpdateGeocoderProfileCommand source, GeocoderProfile destination, JsonDocument member, ResolutionContext context)
            {
                return JsonDocument.Parse(source.Parameters.Params);
            }
        }
    }
}
