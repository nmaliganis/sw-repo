using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Departments;
using sw.auth.common.infrastructure.Exceptions.Departments;
using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Departments;
using sw.common.dtos.Vms.Departments;
using sw.onboarding.model.Departments;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using Serilog;

namespace sw.onboarding.services.V1.Departments;
public class CreateDepartmentProcessor :
  ICreateDepartmentProcessor,
  IRequestHandler<CreateDepartmentCommand, BusinessResult<DepartmentCreationUiModel>> {
  private readonly IUnitOfWork _uOf;
  private readonly IDepartmentRepository _departmentRepository;
  private readonly ICompanyRepository _companyRepository;
  private readonly IAutoMapper _autoMapper;

  public CreateDepartmentProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IDepartmentRepository departmentRepository,
    ICompanyRepository companyRepository) {
    this._uOf = uOf;
    this._autoMapper = autoMapper;
    this._departmentRepository = departmentRepository;
    _companyRepository = companyRepository;
    this._departmentRepository = departmentRepository;
  }

  public async Task<BusinessResult<DepartmentCreationUiModel>> Handle(CreateDepartmentCommand createCommand, CancellationToken cancellationToken) {
    return await this.CreateDepartmentAsync(createCommand);
  }

  public async Task<BusinessResult<DepartmentCreationUiModel>> CreateDepartmentAsync(CreateDepartmentCommand createCommand) {
    var bc = new BusinessResult<DepartmentCreationUiModel>(new DepartmentCreationUiModel());

    if (createCommand.IsNull()) {
      bc.Model = null;
      bc.AddBrokenRule(new BusinessError("ERROR_CREATE_COMMAND_MODEL"));
      return await Task.FromResult(bc);
    }

    try {
      var departmentToBeCreated = new Department();

      var companyToBeInjected = _companyRepository.FindBy(createCommand.Parameters.CompanyId);

      if (companyToBeInjected.IsNull()) {
        bc.Model = null;
        bc.AddBrokenRule(new BusinessError("ERROR_FETCH_COMPANY_NOT_EXISTS"));
        return await Task.FromResult(bc);
      }

      departmentToBeCreated.InjectWithInitialAttributes(createCommand);
      departmentToBeCreated.InjectWithAudit(createCommand.CreatedById);

      departmentToBeCreated.InjectWithCompany(companyToBeInjected);

      this.ThrowExcIfDepartmentCannotBeCreated(departmentToBeCreated);
      this.ThrowExcIfThisDepartmentAlreadyExist(departmentToBeCreated);

      Log.Debug(
        $"Create Department: {createCommand.Parameters.Name}" +
        "--CreateDepartment--  @NotComplete@ [CreateDepartmentProcessor]. " +
        "Message: Just Before MakeItPersistence");

      MakeDepartmentPersistent(departmentToBeCreated);

      Log.Debug(
        $"Create Department: {createCommand.Parameters.Name}" +
        "--CreateDepartment--  @NotComplete@ [CreateDepartmentProcessor]. " +
        "Message: Just After MakeItPersistence");
      bc.Model = ThrowExcIfDepartmentWasNotBeMadePersistent(departmentToBeCreated);
      bc.Model.Message = "SUCCESS_DEPARTMENT_CREATION";
    } catch (InvalidDepartmentException e) {
      string errorMessage = "ERROR_INVALID_DEPARTMENT_MODEL";
      bc.Model = null;
      bc.AddBrokenRule(new BusinessError(errorMessage));
      Log.Error(
        $"Create Department: {createCommand.Parameters.Name}" +
        $"Error Message:{errorMessage}" +
        "--CreateDepartment--  @NotComplete@ [CreateDepartmentProcessor]. " +
        $"Broken rules: {e.BrokenRules}");
    } catch (DepartmentAlreadyExistsException ex) {
      string errorMessage = "ERROR_DEPARTMENT_ALREADY_EXISTS";
      bc.Model = null;
      bc.AddBrokenRule(new BusinessError(errorMessage));
      Log.Error(
        $"Create Department: {createCommand.Parameters.Name}" +
        $"Error Message:{errorMessage}" +
        "--CreateDepartment--  @fail@ [CreateDepartmentProcessor]. " +
        $"@innerfault:{ex?.Message} and {ex?.InnerException}");
    } catch (DepartmentDoesNotExistAfterMadePersistentException exx) {
      string errorMessage = "ERROR_DEPARTMENT_NOT_MADE_PERSISTENT";
      bc.Model = null;
      bc.AddBrokenRule(new BusinessError(errorMessage));
      Log.Error(
        $"Create Department: {createCommand.Parameters.Name}" +
        $"Error Message:{errorMessage}" +
        "--CreateDepartment--  @fail@ [CreateDepartmentProcessor]." +
        $" @innerfault:{exx?.Message} and {exx?.InnerException}");
    } catch (Exception exxx) {
      string errorMessage = "UNKNOWN_ERROR";
      bc.Model = null;
      bc.AddBrokenRule(new BusinessError(errorMessage));
      Log.Error(
        $"Create Department: {createCommand.Parameters.Name}" +
        $"Error Message:{errorMessage}" +
        $"--CreateDepartment--  @fail@ [CreateDepartmentProcessor]. " +
        $"@innerfault:{exxx.Message} and {exxx.InnerException}");
    }

    return await Task.FromResult(bc);
  }

  private void ThrowExcIfThisDepartmentAlreadyExist(Department departmentToBeCreated) {
    var departmentRetrieved = this._departmentRepository.FindActiveByName(departmentToBeCreated.Name);
    if (departmentRetrieved != null) {
      throw new DepartmentAlreadyExistsException(departmentToBeCreated.Name,
        departmentToBeCreated.GetBrokenRulesAsString());
    }
  }

  private DepartmentCreationUiModel ThrowExcIfDepartmentWasNotBeMadePersistent(Department departmentToBeCreated) {
    var retrievedDepartment = this._departmentRepository.FindActiveByName(departmentToBeCreated.Name);
    if (retrievedDepartment != null) {
      return this._autoMapper.Map<DepartmentCreationUiModel>(retrievedDepartment);
    }

    throw new DepartmentDoesNotExistAfterMadePersistentException(departmentToBeCreated.Name);
  }

  private void ThrowExcIfDepartmentCannotBeCreated(Department departmentToBeCreated) {
    bool canBeCreated = !departmentToBeCreated.GetBrokenRules().Any();
    if (!canBeCreated) {
      throw new InvalidDepartmentException(departmentToBeCreated.GetBrokenRulesAsString());
    }
  }

  private void MakeDepartmentPersistent(Department department) {
    this._departmentRepository.Add(department);
    this._uOf.Commit();
  }

}//Class : CreateDepartmentProcessor