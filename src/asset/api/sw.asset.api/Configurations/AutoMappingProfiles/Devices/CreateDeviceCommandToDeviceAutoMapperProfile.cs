using AutoMapper;
using sw.asset.common.dtos.Cqrs.Devices;
using System.Net;
using sw.asset.model.Devices;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Devices;

internal class CreateDeviceCommandToDeviceAutoMapperProfile : Profile
{
    public CreateDeviceCommandToDeviceAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<CreateDeviceCommand, Device>()
            .ForMember(dest => dest.Imei, opt => opt.MapFrom(src => src.Parameters.Imei))
            .ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(src => src.Parameters.SerialNumber))
            .ForMember(dest => dest.IpAddress, opt => opt.MapFrom<CreateIpAddressResolver>())
            .ReverseMap()
            .MaxDepth(1)
            ;
    }
}

internal class CreateIpAddressResolver : IValueResolver<CreateDeviceCommand, Device, IPAddress>
{
    public IPAddress Resolve(CreateDeviceCommand source, Device destination, IPAddress member, ResolutionContext context)
    {
        if (IPAddress.TryParse(source.Parameters.IpAddress, out var address))
        {
            return address;
        }

        return null;
    }
}