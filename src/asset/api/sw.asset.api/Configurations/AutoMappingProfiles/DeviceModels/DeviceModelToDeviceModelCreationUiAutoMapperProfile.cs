using AutoMapper;
using sw.asset.common.dtos.Vms.DeviceModels;
using sw.asset.model.Devices;

namespace sw.asset.api.Configurations.AutoMappingProfiles.DeviceModels
{
    internal class DeviceModelToDeviceModelCreationUiAutoMapperProfile : Profile
    {
        public DeviceModelToDeviceModelCreationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<DeviceModel, DeviceModelCreationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
