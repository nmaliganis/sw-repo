using AutoMapper;
using sw.asset.common.dtos.Vms.Simcards;
using sw.asset.model.Devices.Simcards;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Simcards;

internal class SimcardToSimcardUiAutoMapperProfile : Profile
{
  public SimcardToSimcardUiAutoMapperProfile()
  {
    ConfigureMapping();
  }

  public void ConfigureMapping()
  {
    CreateMap<Simcard, SimcardUiModel>()
      .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
      .ForMember(dest => dest.Message, opt => opt.Ignore())
      .ReverseMap()
      .MaxDepth(1);
  }
}