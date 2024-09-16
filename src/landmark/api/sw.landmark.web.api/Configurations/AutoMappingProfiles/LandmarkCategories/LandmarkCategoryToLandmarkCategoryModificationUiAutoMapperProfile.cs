﻿using AutoMapper;
using sw.landmark.common.dtos.V1.Vms.LandmarkCategories;
using sw.landmark.model;

namespace sw.landmark.web.api.Configurations.AutoMappingProfiles.LandmarkCategories
{
    internal class LandmarkCategoryToLandmarkCategoryModificationUiAutoMapperProfile : Profile
    {
        public LandmarkCategoryToLandmarkCategoryModificationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<LandmarkCategory, LandmarkCategoryModificationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.CodeErp))
                .ForMember(dest => dest.Params, opt => opt.MapFrom(src => src.Params.RootElement.ToString()))

                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
