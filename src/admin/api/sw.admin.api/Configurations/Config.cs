using System;
using sw.admin.contracts.ContractRepositories;
using sw.admin.contracts.V1.CompanyProcessors;
using sw.admin.contracts.V1.DepartmentPersonRoleProcessors;
using sw.admin.contracts.V1.DepartmentProcessors;
using sw.admin.contracts.V1.PersonProcessors;
using sw.admin.repository.DbContexts;
using sw.admin.repository.EfUnitOfWork;
using sw.admin.repository.Repositories;
using sw.admin.services.V1.CompanyService;
using sw.admin.services.V1.DepartmentPersonRoleService;
using sw.admin.services.V1.DepartmentService;
using sw.admin.services.V1.PersonService;
using sw.admin.api.Helpers;
using sw.infrastructure.Exceptions.Repositories.Marten;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.PropertyMappings.TypeHelpers;
using sw.infrastructure.Serializers;
using sw.infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using sw.infrastructure.Exceptions.Repositories.NHibernate;

namespace sw.admin.api.Configurations
{
    internal static class Config
    {
        internal static IServiceCollection ConfigureEfCoreWithNpgsql(this IServiceCollection services, string connectionString)
        {
            try
            {
                services.AddDbContext<swDbContext>(options => options
                    .UseLazyLoadingProxies()
                    .UseNpgsql(connectionString, opt =>
                    {
                        opt.MaxBatchSize(100)
                           .SetPostgresVersion(new Version("11.11"))
                           .UseNetTopologySuite();
                    })
                    .UseLowerCaseNamingConvention()
                );

                return services;
            }
            catch (Exception ex)
            {
                throw new NHibernateInitializationException(ex.Message, ex.InnerException?.Message);
            }
        }

        internal static IServiceCollection CongifureInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IPropertyMappingService, PropertyMappingService>();
            services.AddSingleton<ITypeHelperService, TypeHelperService>();
            services.AddSingleton<IJsonSerializer, JsonSerializer>();
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();

            return services;
        }

        internal static IServiceCollection ConfigureContracts(this IServiceCollection services)
        {
            services.ConfigureCompanyContracts();
            services.ConfigureDepartmentContracts();
            services.ConfigureDepartmentPersonRoleContracts();
            services.ConfigurePersonContracts();

            return services;
        }

        private static IServiceCollection ConfigureCompanyContracts(this IServiceCollection services)
        {
            services.AddScoped<ICreateCompanyProcessor, CreateCompanyProcessor>();
            services.AddScoped<IUpdateCompanyProcessor, UpdateCompanyProcessor>();
            services.AddScoped<IDeleteSoftCompanyProcessor, DeleteSoftCompanyProcessor>();
            services.AddScoped<IDeleteHardCompanyProcessor, DeleteHardCompanyProcessor>();
            services.AddScoped<IGetCompanyByIdProcessor, GetCompanyByIdProcessor>();
            services.AddScoped<IGetCompaniesProcessor, GetCompaniesProcessor>();

            services.AddScoped<ICompanyRepository, CompanyRepository>();
            return services;
        }

        private static IServiceCollection ConfigureDepartmentContracts(this IServiceCollection services)
        {
            services.AddScoped<ICreateDepartmentProcessor, CreateDepartmentProcessor>();
            services.AddScoped<IUpdateDepartmentProcessor, UpdateDepartmentProcessor>();
            services.AddScoped<IDeleteSoftDepartmentProcessor, DeleteSoftDepartmentProcessor>();
            services.AddScoped<IDeleteHardDepartmentProcessor, DeleteHardDepartmentProcessor>();
            services.AddScoped<IGetDepartmentByIdProcessor, GetDepartmentByIdProcessor>();
            services.AddScoped<IGetDepartmentsProcessor, GetDepartmentsProcessor>();

            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            return services;
        }

        private static IServiceCollection ConfigureDepartmentPersonRoleContracts(this IServiceCollection services)
        {
            services.AddScoped<ICreateDepartmentPersonRoleProcessor, CreateDepartmentPersonRoleProcessor>();
            services.AddScoped<IUpdateDepartmentPersonRoleProcessor, UpdateDepartmentPersonRoleProcessor>();
            services.AddScoped<IDeleteSoftDepartmentPersonRoleProcessor, DeleteSoftDepartmentPersonRoleProcessor>();
            services.AddScoped<IDeleteHardDepartmentPersonRoleProcessor, DeleteHardDepartmentPersonRoleProcessor>();
            services.AddScoped<IGetDepartmentPersonRoleByIdProcessor, GetDepartmentPersonRoleByIdProcessor>();
            services.AddScoped<IGetDepartmentPersonRolesProcessor, GetDepartmentPersonRolesProcessor>();

            services.AddScoped<IDepartmentPersonRoleRepository, DepartmentPersonRoleRepository>();
            return services;
        }

        private static IServiceCollection ConfigurePersonContracts(this IServiceCollection services)
        {
            services.AddScoped<ICreatePersonProcessor, CreatePersonProcessor>();
            services.AddScoped<IUpdatePersonProcessor, UpdatePersonProcessor>();
            services.AddScoped<IDeleteSoftPersonProcessor, DeleteSoftPersonProcessor>();
            services.AddScoped<IDeleteHardPersonProcessor, DeleteHardPersonProcessor>();
            services.AddScoped<IGetPersonByIdProcessor, GetPersonByIdProcessor>();
            services.AddScoped<IGetPersonsProcessor, GetPersonsProcessor>();

            services.AddScoped<IPersonRepository, PersonRepository>();
            return services;
        }
    }
}