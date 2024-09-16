using AutoMapper;
using sw.landmark.common.dtos.V1.Cqrs.LandmarkCategories;
using sw.landmark.model;
using System.Text.Json;

namespace sw.landmark.web.api.Configurations.AutoMappingProfiles.LandmarkCategories
{
    internal class CreateLandmarkCategoryCommandToLandmarkCategoryAutoMapperProfile : Profile
    {
        public CreateLandmarkCategoryCommandToLandmarkCategoryAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<CreateLandmarkCategoryCommand, LandmarkCategory>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Parameters.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Parameters.Description))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.Parameters.CodeErp))
                .ForMember(dest => dest.Params, opt => opt.MapFrom<CreateJsonResolver>())

                .MaxDepth(1);
        }

        internal class CreateJsonResolver : IValueResolver<CreateLandmarkCategoryCommand, LandmarkCategory, JsonDocument>
        {
            public JsonDocument Resolve(CreateLandmarkCategoryCommand source, LandmarkCategory destination, JsonDocument member, ResolutionContext context)
            {
                return JsonDocument.Parse(source.Parameters.Params);
            }
        }
    }
}
