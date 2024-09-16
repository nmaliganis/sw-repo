using AutoMapper;
using sw.asset.common.dtos.Vms.Devices;
using sw.asset.model;
using sw.asset.model.Devices;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Devices
{
    internal class DeviceToDeviceUiAutoMapperProfile : Profile
    {
        public DeviceToDeviceUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Device, DeviceUiModel>()
                //.ForMember(dest => dest.DeviceModelId, opt => opt.MapFrom(src => src.DeviceModelId))
                .ForMember(dest => dest.Imei, opt => opt.MapFrom(src => src.Imei))
                .ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(src => src.SerialNumber))
                .ForMember(dest => dest.ActivationCode, opt => opt.MapFrom(src => src.ActivationCode))
                .ForMember(dest => dest.ActivationDate, opt => opt.MapFrom(src => src.ActivationDate))
                .ForMember(dest => dest.ActivationBy, opt => opt.MapFrom(src => src.ActivationBy))
                .ForMember(dest => dest.ProvisioningCode, opt => opt.MapFrom(src => src.ProvisioningCode))
                .ForMember(dest => dest.ProvisioningBy, opt => opt.MapFrom(src => src.ProvisioningBy))
                .ForMember(dest => dest.ProvisioningDate, opt => opt.MapFrom(src => src.ProvisioningDate))
                .ForMember(dest => dest.ResetCode, opt => opt.MapFrom(src => src.ResetCode))
                .ForMember(dest => dest.ResetBy, opt => opt.MapFrom(src => src.ResetBy))
                .ForMember(dest => dest.ResetDate, opt => opt.MapFrom(src => src.ResetDate))
                .ForMember(dest => dest.Activated, opt => opt.MapFrom(src => src.Activated))
                .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.Enabled))
                .ForMember(dest => dest.IpAddress, opt => opt.MapFrom(src => src.IpAddress.ToString()))
                .ForMember(dest => dest.LastRecordedDate, opt => opt.MapFrom(src => src.LastRecordedDate))
                .ForMember(dest => dest.LastReceivedDate, opt => opt.MapFrom(src => src.LastReceivedDate))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.CodeErp))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Simcard.Number))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1)
                .ReverseMap()
                ;
        }
    }
}
