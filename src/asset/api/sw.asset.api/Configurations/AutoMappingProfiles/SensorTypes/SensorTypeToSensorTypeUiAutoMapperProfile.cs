using AutoMapper;
using sw.asset.common.dtos.Vms.SensorTypes;
using sw.asset.model;
using sw.asset.model.SensorTypes;

namespace sw.asset.api.Configurations.AutoMappingProfiles.SensorTypes;

internal class SensorTypeToSensorTypeUiAutoMapperProfile : Profile
{
  public SensorTypeToSensorTypeUiAutoMapperProfile()
  {
    ConfigureMapping();
  }

  public void ConfigureMapping()
  {
    CreateMap<SensorType, SensorTypeUiModel>()
      .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
      .ForMember(dest => dest.ShowAtStatus, opt => opt.MapFrom(src => src.ShowAtStatus))
      .ForMember(dest => dest.StatusExpiryMinutes, opt => opt.MapFrom(src => src.StatusExpiryMinutes))
      .ForMember(dest => dest.ShowOnMap, opt => opt.MapFrom(src => src.ShowOnMap))
      .ForMember(dest => dest.ShowAtReport, opt => opt.MapFrom(src => src.ShowAtReport))
      .ForMember(dest => dest.ShowAtChart, opt => opt.MapFrom(src => src.ShowAtChart))
      .ForMember(dest => dest.ResetValues, opt => opt.MapFrom(src => src.ResetValues))
      .ForMember(dest => dest.SumValues, opt => opt.MapFrom(src => src.SumValues))
      .ForMember(dest => dest.Precision, opt => opt.MapFrom(src => src.Precision))
      .ForMember(dest => dest.Tunit, opt => opt.MapFrom(src => src.Tunit))
      .ForMember(dest => dest.CalcPosition, opt => opt.MapFrom(src => src.CalcPosition))
      .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.CodeErp))
      .ForMember(dest => dest.SensorTypeIndex, opt => opt.MapFrom(src => src.SensorTypeIndex))
      .ForMember(dest => dest.Message, opt => opt.Ignore())
      .ReverseMap()
      .MaxDepth(1);
  }
}