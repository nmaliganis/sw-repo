using AutoMapper;
using sw.asset.common.dtos.Cqrs.Simcards;
using sw.asset.model.Devices.Simcards;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Simcards;

internal class UpdateSimcardCommandToSimcardAutoMapperProfile : Profile
{
  public UpdateSimcardCommandToSimcardAutoMapperProfile()
  {
    ConfigureMapping();
  }

  public void ConfigureMapping()
  {
    CreateMap<UpdateSimcardCommand, Simcard>()
      .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Parameters.Number))
      .ReverseMap()
      .MaxDepth(1);
  }
}