using AutoMapper;
using sw.asset.common.dtos.Vms.DeviceModels;
using sw.asset.model.Devices;

namespace sw.asset.api.Configurations.AutoMappingProfiles.DeviceModels
{
    internal class DeviceModelToDeviceModelUiAutoMapperProfile : Profile
    {
        public DeviceModelToDeviceModelUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<DeviceModel, DeviceModelUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.CodeErp))
                .ForMember(dest => dest.CodeName, opt => opt.MapFrom(src => src.CodeName))
                .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.Enabled))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
