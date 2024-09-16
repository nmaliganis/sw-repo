using AutoMapper;
using sw.admin.api.Configurations.AutoMappingProfiles.Companies;
using sw.admin.api.Configurations.AutoMappingProfiles.DepartmentPersonRoles;
using sw.admin.api.Configurations.AutoMappingProfiles.Departments;
using sw.admin.api.Configurations.AutoMappingProfiles.Persons;
using sw.infrastructure.TypeMappings;
using Microsoft.Extensions.DependencyInjection;

namespace sw.admin.api.Configurations.Installers
{
    internal static class AutoMapperInstaller
    {
        public static IServiceCollection AddAutoMapperInstaller(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                // Company
                cfg.AddProfile<CompanyToCompanyCreationUiAutoMapperProfile>();
                cfg.AddProfile<CompanyToCompanyModificationUiAutoMapperProfile>();
                cfg.AddProfile<CompanyToCompanyUiAutoMapperProfile>();
                cfg.AddProfile<CreateCompanyCommandToCompanyAutoMapperProfile>();
                cfg.AddProfile<UpdateCompanyCommandToCompanyAutoMapperProfile>();

                // Department
                cfg.AddProfile<DepartmentToDepartmentCreationUiAutoMapperProfile>();
                cfg.AddProfile<DepartmentToDepartmentModificationUiAutoMapperProfile>();
                cfg.AddProfile<DepartmentToDepartmentUiAutoMapperProfile>();
                cfg.AddProfile<CreateDepartmentCommandToDepartmentAutoMapperProfile>();
                cfg.AddProfile<UpdateDepartmentCommandToDepartmentAutoMapperProfile>();

                // DepartmentPersonRole
                cfg.AddProfile<DepartmentPersonRoleToDepartmentPersonRoleCreationUiAutoMapperProfile>();
                cfg.AddProfile<DepartmentPersonRoleToDepartmentPersonRoleModificationUiAutoMapperProfile>();
                cfg.AddProfile<DepartmentPersonRoleToDepartmentPersonRoleUiAutoMapperProfile>();
                cfg.AddProfile<CreateDepartmentPersonRoleCommandToDepartmentPersonRoleAutoMapperProfile>();
                cfg.AddProfile<UpdateDepartmentPersonRoleCommandToDepartmentPersonRoleAutoMapperProfile>();

                // Person
                cfg.AddProfile<PersonToPersonCreationUiAutoMapperProfile>();
                cfg.AddProfile<PersonToPersonModificationUiAutoMapperProfile>();
                cfg.AddProfile<PersonToPersonUiAutoMapperProfile>();
                cfg.AddProfile<CreatePersonCommandToPersonAutoMapperProfile>();
                cfg.AddProfile<UpdatePersonCommandToPersonAutoMapperProfile>();
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSingleton<IAutoMapper, AutoMapperAdapter>();

            return services;
        }
    }
}
