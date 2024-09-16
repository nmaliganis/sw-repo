using AutoMapper;
using sw.asset.common.dtos.Vms.SensorTypes;
using sw.asset.model.SensorTypes;

namespace sw.asset.api.Configurations.AutoMappingProfiles.SensorTypes;

internal class SensorTypeToSensorTypeModificationUiAutoMapperProfile : Profile
{
  public SensorTypeToSensorTypeModificationUiAutoMapperProfile()
  {
    ConfigureMapping();
  }

  public void ConfigureMapping()
  {
    CreateMap<SensorType, SensorTypeModificationUiModel>()
      .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
      .ForMember(dest => dest.Message, opt => opt.Ignore())
      .ReverseMap()
      .MaxDepth(1);
  }
}