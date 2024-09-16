using AutoMapper;
using sw.routing.common.dtos.Vms.Itineraries;
using sw.routing.common.dtos.Vms.Jobs;
using sw.routing.model.Itineraries;
using sw.routing.model.Jobs;
using GeoAPI.Geometries;

namespace sw.routing.api.Configurations.AutoMappingProfiles.Jobs;

internal class JobToJobUiAutoMapperProfile : Profile
{
    public JobToJobUiAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<Job, JobUiModel>()
            .ForMember(dest => dest.Seq, opt => opt.MapFrom(src => src.Seq))
            .ForMember(dest => dest.Arrival, opt => opt.MapFrom(src => src.Arrival))
            .ForMember(dest => dest.EstimatedArrival, opt => opt.MapFrom(src => src.EstimatedArrival))
            .ForMember(dest => dest.Departure, opt => opt.MapFrom(src => src.Departure))
            .ForMember(dest => dest.EstimatedDeparture, opt => opt.MapFrom(src => src.EstimatedDeparture))
            .ForMember(dest => dest.ScheduledArrival, opt => opt.MapFrom(src => src.ScheduledArrival))
            .ForMember(dest => dest.Container, opt => opt.MapFrom(src => src.Container))
            .ForMember(dest => dest.Message, opt => opt.Ignore())
            .ReverseMap()
            .MaxDepth(1);
    }
}