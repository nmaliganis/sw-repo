using AutoMapper;
using sw.asset.common.dtos.Vms.Companies.Zones;
using sw.asset.model.Companies.Zones;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Companies.Zones
{
    internal class ZoneToZoneUiAutoMapperProfile : Profile
    {
        public ZoneToZoneUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Zone, ZoneUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
