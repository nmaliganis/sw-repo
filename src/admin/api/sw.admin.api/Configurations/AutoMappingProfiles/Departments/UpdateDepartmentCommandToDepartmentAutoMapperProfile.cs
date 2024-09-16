﻿using AutoMapper;
using sw.admin.common.dtos.V1.Cqrs.Departments;
using sw.admin.model;

namespace sw.admin.api.Configurations.AutoMappingProfiles.Departments
{
    internal class UpdateDepartmentCommandToDepartmentAutoMapperProfile : Profile
    {
        public UpdateDepartmentCommandToDepartmentAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<UpdateDepartmentCommand, Department>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Parameters.Name))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Parameters.Notes))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.Parameters.CodeErp))
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.Parameters.CompanyId))

                .MaxDepth(1);
        }
    }
}
