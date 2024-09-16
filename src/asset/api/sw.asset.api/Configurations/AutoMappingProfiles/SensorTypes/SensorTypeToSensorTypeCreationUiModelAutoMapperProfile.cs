using AutoMapper;
using sw.asset.common.dtos.Vms.SensorTypes;
using sw.asset.model;
using sw.asset.model.SensorTypes;

namespace sw.asset.api.Configurations.AutoMappingProfiles.SensorTypes;

internal class SensorTypeToSensorTypeCreationUiModelAutoMapperProfile : Profile
{
  public SensorTypeToSensorTypeCreationUiModelAutoMapperProfile()
  {
    ConfigureMapping();
  }

  public void ConfigureMapping()
  {
    CreateMap<SensorType, SensorTypeCreationUiModel>()
      .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
      .ForMember(dest => dest.Message, opt => opt.Ignore())
      .ReverseMap()
      .MaxDepth(1);
  }
}