using AutoMapper;
using sw.auth.api.Configurations.AutoMappingProfiles.Companies;
using sw.auth.api.Configurations.AutoMappingProfiles.Departments;
using sw.auth.api.Configurations.AutoMappingProfiles.Departments.DepartmentRoles;
using sw.auth.api.Configurations.AutoMappingProfiles.Members;
using sw.auth.api.Configurations.AutoMappingProfiles.Members.MemberDepartments;
using sw.auth.api.Configurations.AutoMappingProfiles.Roles;
using sw.auth.api.Configurations.AutoMappingProfiles.Users;
using sw.infrastructure.TypeMappings;
using Microsoft.Extensions.DependencyInjection;

namespace sw.auth.api.Configurations.Installers;

internal static class AutoMapperInstaller
{
    public static IServiceCollection AddAutoMapperInstaller(this IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            // Company
            cfg.AddProfile<CreateCompanyCommandToCompanyAutoMapperProfile>();
            cfg.AddProfile<CompanyToCompanyUiAutoMapperProfile>();
            cfg.AddProfile<CompanyToCompanyModificationUiAutoMapperProfile>();
            // Role
            cfg.AddProfile<CreateRoleCommandToRoleAutoMapperProfile>();
            cfg.AddProfile<RoleToRoleUiAutoMapperProfile>();
            cfg.AddProfile<RoleToRoleModificationUiAutoMapperProfile>();
            // User
            cfg.AddProfile<UserToUserUiAutoMapperProfile>();
            // Members
            cfg.AddProfile<MemberToMemberUiAutoMapperProfile>();
            cfg.AddProfile<MemberDepartmentToMemberDepartmentUiAutoMapperProfile>();
            // Department
            cfg.AddProfile<DepartmentToDepartmentCreationUiAutoMapperProfile>();
            cfg.AddProfile<DepartmentToDepartmentModificationUiAutoMapperProfile>();
            cfg.AddProfile<DepartmentToDepartmentUiAutoMapperProfile>();
            cfg.AddProfile<DepartmentRoleToDepartmentRoleUiAutoMapperProfile>();
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        services.AddSingleton<IAutoMapper, AutoMapperAdapter>();

        return services;
    }
}