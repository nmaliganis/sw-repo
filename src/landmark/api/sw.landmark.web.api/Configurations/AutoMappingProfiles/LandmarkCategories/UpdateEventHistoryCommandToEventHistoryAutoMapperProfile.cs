using AutoMapper;
using sw.landmark.common.dtos.V1.Cqrs.LandmarkCategories;
using sw.landmark.model;
using System.Text.Json;

namespace sw.landmark.web.api.Configurations.AutoMappingProfiles.LandmarkCategories
{
    internal class UpdateLandmarkCategoryCommandToLandmarkCategoryAutoMapperProfile : Profile
    {
        public UpdateLandmarkCategoryCommandToLandmarkCategoryAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<UpdateLandmarkCategoryCommand, LandmarkCategory>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Parameters.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Parameters.Description))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.Parameters.CodeErp))
                .ForMember(dest => dest.Params, opt => opt.MapFrom<CreateJsonResolver>())

                .MaxDepth(1);
        }

        internal class CreateJsonResolver : IValueResolver<UpdateLandmarkCategoryCommand, LandmarkCategory, JsonDocument>
        {
            public JsonDocument Resolve(UpdateLandmarkCategoryCommand source, LandmarkCategory destination, JsonDocument member, ResolutionContext context)
            {
                return JsonDocument.Parse(source.Parameters.Params);
            }
        }
    }
}
