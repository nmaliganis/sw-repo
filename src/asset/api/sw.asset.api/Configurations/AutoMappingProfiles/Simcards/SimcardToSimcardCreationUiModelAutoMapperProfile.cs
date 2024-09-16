using AutoMapper;
using sw.asset.common.dtos.Vms.Simcards;
using sw.asset.model.Devices.Simcards;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Simcards;

internal class SimcardToSimcardCreationUiModelAutoMapperProfile : Profile
{
  public SimcardToSimcardCreationUiModelAutoMapperProfile()
  {
    ConfigureMapping();
  }

  public void ConfigureMapping()
  {
    CreateMap<Simcard, SimcardCreationUiModel>()
      .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
      .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
      .ForMember(dest => dest.Message, opt => opt.Ignore())
      .ReverseMap()
      .MaxDepth(1)
      ;

  }
}