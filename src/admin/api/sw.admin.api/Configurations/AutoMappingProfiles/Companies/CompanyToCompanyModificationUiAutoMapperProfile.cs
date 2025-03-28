﻿using AutoMapper;
using sw.admin.common.dtos.V1.Vms.Companies;
using sw.admin.model;

namespace sw.admin.api.Configurations.AutoMappingProfiles.Companies
{
    internal class CompanyToCompanyModificationUiAutoMapperProfile : Profile
    {
        public CompanyToCompanyModificationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Company, CompanyModificationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
