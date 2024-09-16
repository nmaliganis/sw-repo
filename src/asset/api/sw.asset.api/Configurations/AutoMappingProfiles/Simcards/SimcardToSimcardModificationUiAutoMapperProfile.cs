using AutoMapper;
using sw.asset.common.dtos.Vms.Simcards;
using sw.asset.model.Devices.Simcards;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Simcards
{
    internal class SimcardToSimcardModificationUiAutoMapperProfile : Profile
    {
        public SimcardToSimcardModificationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Simcard, SimcardModificationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
