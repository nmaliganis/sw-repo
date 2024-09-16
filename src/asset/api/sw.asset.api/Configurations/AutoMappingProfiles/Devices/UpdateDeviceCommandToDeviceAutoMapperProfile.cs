using AutoMapper;
using sw.asset.common.dtos.Cqrs.Devices;
using sw.asset.model;
using System.Net;
using sw.asset.model.Devices;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Devices
{
    internal class UpdateDeviceCommandToDeviceAutoMapperProfile : Profile
    {
        public UpdateDeviceCommandToDeviceAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<UpdateDeviceCommand, Device>()
                //.ForMember(dest => dest.DeviceModelId, opt => opt.MapFrom(src => src.parameters.DeviceModelId))
                .ForMember(dest => dest.Imei, opt => opt.MapFrom(src => src.Parameters.Imei))
                .ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(src => src.Parameters.SerialNumber))
                .ForMember(dest => dest.ActivationCode, opt => opt.MapFrom(src => src.Parameters.ActivationCode))
                .ForMember(dest => dest.ActivationDate, opt => opt.MapFrom(src => src.Parameters.ActivationDate))
                .ForMember(dest => dest.ActivationBy, opt => opt.MapFrom(src => src.Parameters.ActivationBy))
                .ForMember(dest => dest.ProvisioningCode, opt => opt.MapFrom(src => src.Parameters.ProvisioningCode))
                .ForMember(dest => dest.ProvisioningBy, opt => opt.MapFrom(src => src.Parameters.ProvisioningBy))
                .ForMember(dest => dest.ProvisioningDate, opt => opt.MapFrom(src => src.Parameters.ProvisioningDate))
                .ForMember(dest => dest.ResetCode, opt => opt.MapFrom(src => src.Parameters.ResetCode))
                .ForMember(dest => dest.ResetBy, opt => opt.MapFrom(src => src.Parameters.ResetBy))
                .ForMember(dest => dest.ResetDate, opt => opt.MapFrom(src => src.Parameters.ResetDate))
                .ForMember(dest => dest.Activated, opt => opt.MapFrom(src => src.Parameters.Activated))
                .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.Parameters.Enabled))
                .ForMember(dest => dest.IpAddress, opt => opt.MapFrom<UpdateIpAddressResolver>())
                .ForMember(dest => dest.LastRecordedDate, opt => opt.MapFrom(src => src.Parameters.LastRecordedDate))
                .ForMember(dest => dest.LastReceivedDate, opt => opt.MapFrom(src => src.Parameters.LastReceivedDate))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.Parameters.CodeErp))

                .MaxDepth(1);
        }
    }

    internal class UpdateIpAddressResolver : IValueResolver<UpdateDeviceCommand, Device, IPAddress>
    {
        public IPAddress Resolve(UpdateDeviceCommand source, Device destination, IPAddress member, ResolutionContext context)
        {
            if (IPAddress.TryParse(source.Parameters.IpAddress, out var address))
            {
                return address;
            }

            return null;
        }
    }// Class: UpdateDeviceCommandToDeviceAutoMapperProfile
}// Namespace: sw.asset.api.Configurations.AutoMappingProfiles.Devices
