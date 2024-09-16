using AutoMapper;
using sw.asset.common.dtos.Vms.Sensors;
using sw.asset.model;
using sw.asset.model.Sensors;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Sensors
{
    internal class SensorToSensorCreationUiModelAutoMapperProfile : Profile
    {
        public SensorToSensorCreationUiModelAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Sensor, SensorCreationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);

        }
    }
}
