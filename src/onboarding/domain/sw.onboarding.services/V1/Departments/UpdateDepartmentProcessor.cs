using System;
using System.Threading;
using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Departments;
using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.DepartmentProcessors;
using sw.auth.model.Departments;
using sw.common.dtos.Vms.Departments;
using sw.onboarding.model.Departments;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using Serilog;

namespace sw.auth.services.V1.Departments
{
    public class UpdateDepartmentProcessor :
        IUpdateDepartmentProcessor,
        IRequestHandler<UpdateDepartmentCommand, BusinessResult<DepartmentModificationUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IAutoMapper _autoMapper;
        public UpdateDepartmentProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            IDepartmentRepository departmentRepository, ICompanyRepository companyRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _departmentRepository = departmentRepository;
            _companyRepository = companyRepository;
        }

        public async Task<BusinessResult<DepartmentModificationUiModel>> Handle(UpdateDepartmentCommand updateCommand, CancellationToken cancellationToken)
        {
            return await UpdateDepartmentAsync(updateCommand);
        }

        public async Task<BusinessResult<DepartmentModificationUiModel>> UpdateDepartmentAsync(UpdateDepartmentCommand updateCommand)
        {
            var bc = new BusinessResult<DepartmentModificationUiModel>(new DepartmentModificationUiModel());

            if (updateCommand.IsNull())
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_UPDATE_COMMAND_MODEL"));
                return await Task.FromResult(bc);
            }

            try
            {
                var department = _departmentRepository.FindBy(updateCommand.Id);
                if (department is null)
                {
                    bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "Department Id does not exist"));
                    return bc;
                }

                department.ModifyWithAudit(updateCommand.ModifiedById, updateCommand);

                Persist(department, updateCommand.Id);

                var response = _autoMapper.Map<DepartmentModificationUiModel>(department);
                response.Message = $"Department id: {response.Id} updated successfully";

                bc.Model = response;
            }
            catch (Exception e)
            {
                string errorMessage = "UNKNOWN_ERROR";
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError(errorMessage));
                Log.Error(
                    $"Update Department: {updateCommand.Parameters.Name}" +
                    $"Error Message:{errorMessage}" +
                    $"--UpdateDepartment--  @fail@ [UpdateDepartmentProcessor]. " +
                    $"@innerfault:{e.Message} and {e.InnerException}");
            }

            return await Task.FromResult(bc);
        }

        private void Persist(Department department, long id)
        {
            _departmentRepository.Save(department, id);
            _uOf.Commit();
        }
    }
}
