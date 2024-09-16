using AutoMapper;
using sw.asset.model;
using System.Text.Json;
using sw.asset.common.dtos.Cqrs.Sensor;
using sw.asset.model.Sensors;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Sensors
{
    internal class UpdateSensorCommandToSensorAutoMapperProfile : Profile
    {
        public UpdateSensorCommandToSensorAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
             CreateMap<UpdateSensorCommand, Sensor>()
                .ForPath(dest => dest.Params.Params, opt => opt.MapFrom(src => src.Parameters.Params))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Parameters.Name))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.Parameters.CodeErp))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Parameters.IsActive))
                .ForMember(dest => dest.IsVisible, opt => opt.MapFrom(src => src.Parameters.IsVisible))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Parameters.Order))
                .ForMember(dest => dest.MinValue, opt => opt.MapFrom(src => src.Parameters.MinValue))
                .ForMember(dest => dest.MaxValue, opt => opt.MapFrom(src => src.Parameters.MaxValue))
                .ForMember(dest => dest.MinNotifyValue, opt => opt.MapFrom(src => src.Parameters.MinNotifyValue))
                .ForMember(dest => dest.MaxNotifyValue, opt => opt.MapFrom(src => src.Parameters.MaxNotifyValue))
                .ForMember(dest => dest.LastValue, opt => opt.MapFrom(src => src.Parameters.LastValue))
                .ForMember(dest => dest.LastRecordedDate, opt => opt.MapFrom(src => src.Parameters.LastRecordedDate))
                .ForMember(dest => dest.LastReceivedDate, opt => opt.MapFrom(src => src.Parameters.LastReceivedDate))
                .ForMember(dest => dest.HighThreshold, opt => opt.MapFrom(src => src.Parameters.HighThreshold))
                .ForMember(dest => dest.LowThreshold, opt => opt.MapFrom(src => src.Parameters.LowThreshold))
                .ForMember(dest => dest.SamplingInterval, opt => opt.MapFrom(src => src.Parameters.SamplingInterval))
                .ForMember(dest => dest.ReportingInterval, opt => opt.MapFrom(src => src.Parameters.ReportingInterval))

                .MaxDepth(1);
        }

        //internal class UpdateJsonResolver : IValueResolver<UpdateSensorCommand, Sensor, JsonDocument>
        //{
        //    public JsonDocument Resolve(UpdateSensorCommand source, Sensor destination, JsonDocument member, ResolutionContext context)
        //    {
        //        return JsonDocument.Parse(source.Parameters.Params);
        //    }
        //}
    }
}
