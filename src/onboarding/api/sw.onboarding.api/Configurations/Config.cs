
using sw.auth.api.Helpers;
using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Companies;
using sw.auth.contracts.V1.DepartmentProcessors;
using sw.auth.contracts.V1.Departments;
using sw.auth.contracts.V1.Members;
using sw.auth.contracts.V1.Roles;
using sw.auth.contracts.V1.Users;
using sw.auth.repository.Mappings.Users;
using sw.auth.repository.Repositories;
using sw.auth.services.V1.Companies;
using sw.auth.services.V1.Departments;
using sw.auth.services.V1.DepartmentService;
using sw.auth.services.V1.Users;
using sw.common.dtos.Vms.Accounts;
using sw.onboarding.api.Helpers;
using sw.onboarding.contracts.ContractRepositories;
using sw.onboarding.repository.Repositories;
using sw.onboarding.services.V1.Companies;
using sw.onboarding.services.V1.Departments;
using sw.onboarding.services.V1.Members;
using sw.onboarding.services.V1.Roles;
using sw.onboarding.services.V1.Users;
using sw.infrastructure.Exceptions.Repositories.NHibernate;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.PropertyMappings.TypeHelpers;
using sw.infrastructure.Serializers;
using sw.infrastructure.UnitOfWorks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Driver;
using NHibernate.Spatial.Dialect;
using NHibernate.Spatial.Mapping;
using NHibernate.Spatial.Metadata;
using System;
using System.Reflection;
using sw.onboarding.repository.NhUnitOfWork;
using ISession = NHibernate.ISession;

namespace sw.onboarding.api.Configurations;

internal static class Config
{
    public static void ConfigureNHibernateWithNpgsql(this IServiceCollection services, string connectionString)
    {
        HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
        try
        {
            var cfg = Fluently.Configure()
              .Database(PostgreSQLConfiguration.PostgreSQL82
                .ConnectionString(connectionString)
                .Driver<NpgsqlDriver>()
                .Dialect<PostGis20Dialect>()
                .ShowSql()
                .MaxFetchDepth(5)
                .FormatSql()
                .Raw("transaction.use_connection_on_system_prepare", "true")
                .AdoNetBatchSize(100)
              )
              .Mappings(x => x.FluentMappings.AddFromAssemblyOf<UserMap>())
              .Cache(c => c.UseSecondLevelCache().UseQueryCache()
                .ProviderClass(typeof(NHibernate.Caches.RtMemoryCache.RtMemoryCacheProvider)
                  .AssemblyQualifiedName)
              )
              .CurrentSessionContext("web")
              .BuildConfiguration();

            cfg.AddAssembly(Assembly.GetExecutingAssembly());
            cfg.AddAuxiliaryDatabaseObject(new SpatialAuxiliaryDatabaseObject(cfg));
            Metadata.AddMapping(cfg, MetadataClass.GeometryColumn);
            Metadata.AddMapping(cfg, MetadataClass.SpatialReferenceSystem);

            var sessionFactory = cfg.BuildSessionFactory();

            services.AddSingleton<ISessionFactory>(sessionFactory);

            services.AddScoped<ISession>((ctx) =>
            {
                var sf = ctx.GetRequiredService<ISessionFactory>();

                return sf.OpenSession();
            });
        }
        catch (Exception ex)
        {
            throw new NHibernateInitializationException(ex.Message, ex.InnerException?.Message);
        }
    }

    internal static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IPropertyMappingService, PropertyMappingService>();
        services.AddSingleton<ITypeHelperService, TypeHelperService>();
        services.AddSingleton<IJsonSerializer, JsonSerializer>();
        services.AddScoped<IUnitOfWork, NhUnitOfWork>();

        return services;
    }

    internal static IServiceCollection ConfigureContracts(this IServiceCollection services)
    {
        services.ConfigureRoleContracts();
        services.ConfigureCompanyContracts();
        services.ConfigureUserContracts();
        services.ConfigureMemberContracts();
        services.ConfigureDepartmentContracts();

        return services;
    }

    private static IServiceCollection ConfigureRoleContracts(this IServiceCollection services)
    {
        services.AddScoped<ICreateRoleProcessor, CreateRoleProcessor>();
        services.AddScoped<IUpdateRoleProcessor, UpdateRoleProcessor>();
        services.AddScoped<IDeleteSoftRoleProcessor, DeleteSoftRoleProcessor>();
        services.AddScoped<IDeleteHardRoleProcessor, DeleteHardRoleProcessor>();
        services.AddScoped<IGetRoleByIdProcessor, GetRoleByIdProcessor>();
        services.AddScoped<IGetRolesProcessor, GetRolesProcessor>();

        services.AddScoped<IRoleRepository, RoleRepository>();
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

    private static IServiceCollection ConfigureUserContracts(this IServiceCollection services)
    {
        services.AddScoped<ICreateUserProcessor, CreateUserProcessor>();
        services.AddScoped<IUpdateUserProcessor, UpdateUserProcessor>();
        services.AddScoped<IDeleteSoftUserProcessor, DeleteSoftUserProcessor>();
        services.AddScoped<IDeleteHardUserProcessor, DeleteHardUserProcessor>();
        services.AddScoped<IGetUserByIdProcessor, GetUserByIdProcessor>();
        services.AddScoped<IGetUserByEmailProcessor, GetUserByEmailProcessor>();
        services.AddScoped<IGetUserByLoginAndPasswordProcessor, GetUserByLoginAndPasswordProcessor>();
        services.AddScoped<IGetUsersProcessor, GetUsersProcessor>();

        services.AddScoped<IUserRepository, UserRepository>();
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

    private static IServiceCollection ConfigureMemberContracts(this IServiceCollection services)
    {
        services.AddScoped<ISearchMemberByEmailOrLoginProcessor, SearchMemberByEmailOrLoginProcessor>();

        services.AddScoped<IMemberRepository, MemberRepository>();
        return services;
    }

    public static IServiceCollection AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<AuthSettings>(configuration);
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
        {
              var problemDetails = new ValidationProblemDetails(context.ModelState)
              {
                  Instance = context.HttpContext.Request.Path,
                  Status = StatusCodes.Status400BadRequest,
                  Detail = "Please refer to the errors property for additional details."
              };

              return new BadRequestObjectResult(problemDetails)
              {
                  ContentTypes = { "application/problem+json", "application/problem+xml" }
              };
          };
        });

        return services;
    }

    public static IServiceCollection AddApplicationInsights(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationInsightsTelemetry(configuration);

        return services;
    }

    public static IServiceCollection AddCustomMvc(this IServiceCollection services)
    {
        // Add framework services.
        services.AddControllers(options =>
          {
              options.Filters.Add(typeof(HttpGlobalExceptionFilter));
          })
          // Added for functional tests
          .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
          builder => builder
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
        });

        return services;
    }
}