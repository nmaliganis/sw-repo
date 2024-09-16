using AutoMapper;
using sw.auth.api.Configurations.AutoMappingProfiles.Roles;
using sw.auth.api.Configurations.AutoMappingProfiles.Users;
using sw.onboarding.api.Configurations.AutoMappingProfiles.Companies;
using sw.onboarding.api.Configurations.AutoMappingProfiles.Departments;
using sw.onboarding.api.Configurations.AutoMappingProfiles.Roles;
using sw.infrastructure.TypeMappings;
using Microsoft.Extensions.DependencyInjection;

namespace sw.onboarding.api.Configurations.Installers;

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

            // Department
            cfg.AddProfile<DepartmentToDepartmentCreationUiAutoMapperProfile>();
            cfg.AddProfile<DepartmentToDepartmentModificationUiAutoMapperProfile>();
            cfg.AddProfile<DepartmentToDepartmentUiAutoMapperProfile>();
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        services.AddSingleton<IAutoMapper, AutoMapperAdapter>();

        return services;
    }
}