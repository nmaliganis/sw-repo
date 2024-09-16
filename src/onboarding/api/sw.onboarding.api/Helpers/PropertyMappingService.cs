using System;
using System.Collections.Generic;
using sw.auth.common.dtos.Vms.Companies;
using sw.auth.common.dtos.Vms.Roles;
using sw.auth.common.dtos.Vms.Users;
using sw.auth.model.Roles;
using sw.auth.model.Users;
using sw.common.dtos.Vms.Departments;
using sw.onboarding.model.Companies;
using sw.onboarding.model.Departments;
using sw.infrastructure.PropertyMappings;

namespace sw.onboarding.api.Helpers;

/// <summary>
/// Class : PropertyMappingService
/// </summary>
public class PropertyMappingService : BasePropertyMapping
{
    private readonly Dictionary<string, PropertyMappingValue> _rolePropertyMapping =
        new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
        };

    private readonly Dictionary<string, PropertyMappingValue> _userPropertyMapping =
        new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
        };

    private readonly Dictionary<string, PropertyMappingValue> _companyPropertyMapping =
        new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
        };

    private readonly Dictionary<string, PropertyMappingValue> _departmentPropertyMapping =
        new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
        };

    private static readonly IList<IPropertyMapping> PropertyMappings = new List<IPropertyMapping>();

    /// <summary>
    /// PropertyMappingService
    /// </summary>
    public PropertyMappingService() : base(PropertyMappings)
    {
        PropertyMappings.Add(new PropertyMapping<RoleUiModel, Role>(_rolePropertyMapping));
        PropertyMappings.Add(new PropertyMapping<UserUiModel, User>(_userPropertyMapping));
        PropertyMappings.Add(new PropertyMapping<CompanyUiModel, Company>(_companyPropertyMapping));
        PropertyMappings.Add(new PropertyMapping<DepartmentUiModel, Department>(_userPropertyMapping));
    }
}