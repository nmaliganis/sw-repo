using AutoMapper;
using sw.asset.common.dtos.Vms.Sensors;
using sw.asset.model;
using sw.asset.model.Sensors;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Sensors
{
    internal class SensorToSensorUiAutoMapperProfile : Profile
    {
        public SensorToSensorUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Sensor, SensorUiModel>()
                // .ForMember(dest => dest.AssetId, opt => opt.MapFrom(src => src.AssetId))
                // .ForMember(dest => dest.DeviceId, opt => opt.MapFrom(src => src.DeviceId))
                // .ForMember(dest => dest.SensorTypeId, opt => opt.MapFrom(src => src.SensorTypeId))
                .ForMember(dest => dest.Params, opt => opt.MapFrom(src => src.Params.Params))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.CodeErp))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsVisible, opt => opt.MapFrom(src => src.IsVisible))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order))
                .ForMember(dest => dest.MinValue, opt => opt.MapFrom(src => src.MinValue))
                .ForMember(dest => dest.MaxValue, opt => opt.MapFrom(src => src.MaxValue))
                .ForMember(dest => dest.MinNotifyValue, opt => opt.MapFrom(src => src.MinNotifyValue))
                .ForMember(dest => dest.MaxNotifyValue, opt => opt.MapFrom(src => src.MaxNotifyValue))
                .ForMember(dest => dest.LastValue, opt => opt.MapFrom(src => src.LastValue))
                .ForMember(dest => dest.LastRecordedDate, opt => opt.MapFrom(src => src.LastRecordedDate))
                .ForMember(dest => dest.LastReceivedDate, opt => opt.MapFrom(src => src.LastReceivedDate))
                .ForMember(dest => dest.HighThreshold, opt => opt.MapFrom(src => src.HighThreshold))
                .ForMember(dest => dest.LowThreshold, opt => opt.MapFrom(src => src.LowThreshold))
                .ForMember(dest => dest.SamplingInterval, opt => opt.MapFrom(src => src.SamplingInterval))
                .ForMember(dest => dest.ReportingInterval, opt => opt.MapFrom(src => src.ReportingInterval))

                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
