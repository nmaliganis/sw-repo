using sw.admin.common.dtos.V1.Vms.Companies;
using sw.admin.common.dtos.V1.Vms.DepartmentPersonRoles;
using sw.admin.common.dtos.V1.Vms.Departments;
using sw.admin.common.dtos.V1.Vms.Persons;
using sw.admin.model;
using sw.infrastructure.PropertyMappings;
using System;
using System.Collections.Generic;
using sw.landmark.common.dtos.V1.Vms.Departments;

namespace sw.admin.api.Helpers
{
    /// <summary>
    /// Class : PropertyMappingService
    /// </summary>
    public class PropertyMappingService : BasePropertyMapping
    {
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

        private readonly Dictionary<string, PropertyMappingValue> _departmentPersonRolePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
            };

        private readonly Dictionary<string, PropertyMappingValue> _personPropertyMapping =
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
            PropertyMappings.Add(new PropertyMapping<CompanyUiModel, Company>(_companyPropertyMapping));
            PropertyMappings.Add(new PropertyMapping<DepartmentUiModel, Department>(_departmentPropertyMapping));
            PropertyMappings.Add(new PropertyMapping<DepartmentPersonRoleUiModel, DepartmentPersonRole>(_departmentPersonRolePropertyMapping));
            PropertyMappings.Add(new PropertyMapping<PersonUiModel, Person>(_personPropertyMapping));
        }
    }
}
