using AutoMapper;
using sw.asset.common.dtos.Cqrs.Simcards;
using sw.asset.model.Devices.Simcards;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Simcards;

internal class CreateSimcardCommandToSimcardAutoMapperProfile : Profile
{
  public CreateSimcardCommandToSimcardAutoMapperProfile()
  {
    ConfigureMapping();
  }

  public void ConfigureMapping()
  {
    CreateMap<CreateSimcardCommand, Simcard>()
      .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Parameters.Number))

      .MaxDepth(1);
  }
}