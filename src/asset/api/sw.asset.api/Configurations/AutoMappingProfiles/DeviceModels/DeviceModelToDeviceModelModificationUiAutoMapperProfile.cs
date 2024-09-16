using AutoMapper;
using sw.asset.common.dtos.Vms.DeviceModels;
using sw.asset.model.Devices;

namespace sw.asset.api.Configurations.AutoMappingProfiles.DeviceModels
{
    internal class DeviceModelToDeviceModelModificationUiAutoMapperProfile : Profile
    {
        public DeviceModelToDeviceModelModificationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<DeviceModel, DeviceModelModificationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
