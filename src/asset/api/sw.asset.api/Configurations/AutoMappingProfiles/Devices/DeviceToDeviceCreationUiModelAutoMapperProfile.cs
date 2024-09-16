using AutoMapper;
using sw.asset.common.dtos.Vms.Devices;
using sw.asset.model;
using sw.asset.model.Devices;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Devices
{
    internal class DeviceToDeviceCreationUiModelAutoMapperProfile : Profile
    {
        public DeviceToDeviceCreationUiModelAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Device, DeviceCreationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);

        }
    }
}
