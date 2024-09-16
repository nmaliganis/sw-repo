using AutoMapper;
using sw.asset.common.dtos.Cqrs.SensorTypes;
using sw.asset.model;
using sw.asset.model.SensorTypes;

namespace sw.asset.api.Configurations.AutoMappingProfiles.SensorTypes
{
    internal class CreateSensorTypeCommandToSensorTypeAutoMapperProfile : Profile
    {
        public CreateSensorTypeCommandToSensorTypeAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<CreateSensorTypeCommand, SensorType>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Parameters.Name))
                .ForMember(dest => dest.ShowAtStatus, opt => opt.MapFrom(src => src.Parameters.ShowAtStatus))
                .ForMember(dest => dest.StatusExpiryMinutes, opt => opt.MapFrom(src => src.Parameters.StatusExpiryMinutes))
                .ForMember(dest => dest.ShowOnMap, opt => opt.MapFrom(src => src.Parameters.ShowOnMap))
                .ForMember(dest => dest.ShowAtReport, opt => opt.MapFrom(src => src.Parameters.ShowAtReport))
                .ForMember(dest => dest.ShowAtChart, opt => opt.MapFrom(src => src.Parameters.ShowAtChart))
                .ForMember(dest => dest.ResetValues, opt => opt.MapFrom(src => src.Parameters.ResetValues))
                .ForMember(dest => dest.SumValues, opt => opt.MapFrom(src => src.Parameters.SumValues))
                .ForMember(dest => dest.Precision, opt => opt.MapFrom(src => src.Parameters.Precision))
                .ForMember(dest => dest.Tunit, opt => opt.MapFrom(src => src.Parameters.Tunit))
                .ForMember(dest => dest.CalcPosition, opt => opt.MapFrom(src => src.Parameters.CalcPosition))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.Parameters.CodeErp))
                .ForMember(dest => dest.SensorTypeIndex, opt => opt.MapFrom(src => src.Parameters.SensorTypeIndex))

                .ReverseMap()
                .MaxDepth(1);
        }
    }
}
