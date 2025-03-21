﻿using sw.admin.common.dtos.V1.Cqrs.Departments;
using sw.admin.common.dtos.V1.Vms.Departments;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.admin.contracts.V1.DepartmentProcessors
{
    public interface IDeleteSoftDepartmentProcessor
    {
        Task<BusinessResult<DepartmentDeletionUiModel>> DeleteSoftDepartmentAsync(DeleteSoftDepartmentCommand deleteCommand);
    }
}
