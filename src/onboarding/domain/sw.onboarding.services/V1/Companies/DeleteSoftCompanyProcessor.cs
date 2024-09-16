using System;
using System.Threading;
using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Companies;
using sw.auth.common.dtos.Vms.Companies;
using sw.auth.common.infrastructure.Exceptions.Companies;
using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Companies;
using sw.onboarding.model.Companies;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using Serilog;

namespace sw.onboarding.services.V1.Companies;

public class DeleteSoftCompanyProcessor :
  IDeleteSoftCompanyProcessor,
  IRequestHandler<DeleteSoftCompanyCommand, BusinessResult<CompanyDeletionUiModel>>
{
  private readonly IUnitOfWork _uOf;
  private readonly IAutoMapper _autoMapper;
  private readonly ICompanyRepository _companyRepository;

  public DeleteSoftCompanyProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, ICompanyRepository companyRepository)
  {
    this._uOf = uOf;
    this._autoMapper = autoMapper;
    this._companyRepository = companyRepository;
  }

  public async Task<BusinessResult<CompanyDeletionUiModel>> Handle(DeleteSoftCompanyCommand deleteCommand, CancellationToken cancellationToken)
  {
    return await this.DeleteSoftCompanyAsync(deleteCommand);
  }

  public async Task<BusinessResult<CompanyDeletionUiModel>> DeleteSoftCompanyAsync(DeleteSoftCompanyCommand deleteCommand)
  {
    var bc = new BusinessResult<CompanyDeletionUiModel>(new CompanyDeletionUiModel());

    if (deleteCommand.IsNull() || deleteCommand.CompanyIdToBeDeleted <= 0) {
      bc.Model = null;
      bc.AddBrokenRule(new BusinessError("ERROR_DELETE_COMMAND_MODEL"));
      return await Task.FromResult(bc);
    }

    try
    {
      var companyToBeSoftDeleted = _companyRepository.FindBy(deleteCommand!.CompanyIdToBeDeleted);

      if (companyToBeSoftDeleted.IsNull() )
        throw new CompanyDoesNotExistException(deleteCommand!.CompanyIdToBeDeleted);

      companyToBeSoftDeleted.SoftDeleted();
      companyToBeSoftDeleted.DeleteWithAudit(deleteCommand!.DeletedBy, deleteCommand.DeletedReason);

      Log.Debug(
          $"Update-Delete Company: with Id: {deleteCommand!.CompanyIdToBeDeleted}" +
          "--SoftDeleteCompany--  @Ready@ [DeleteCompanyProcessor]. " +
          "Message: Just Before MakeItPersistence");

      MakeCompanyPersistent(companyToBeSoftDeleted);

      Log.Debug(
          $"Update-Delete Company: with Id: {deleteCommand!.CompanyIdToBeDeleted}" +
          "--SoftDeleteCompany--  @Ready@ [DeleteCompanyProcessor]. " +
          "Message: Just After MakeItPersistence");

      bc.Model = ThrowExcIfCompanyWasNotBeMadePersistent(companyToBeSoftDeleted);
      bc.Model.DeletionStatus = true;
      bc.Model.Message = "SUCCESS_DELETION";
    }
    catch (CompanyDoesNotExistException e)
    {
      string errorMessage = "ERROR_COMPANY_DOES_NOT_EXIST";
      bc.Model.Message = errorMessage;
      bc.AddBrokenRule(new BusinessError(errorMessage));
      Log.Error(
          $"Update-Delete Company: Id: {deleteCommand!.CompanyIdToBeDeleted}" +
          $"Error Message:{errorMessage}" +
          "--SoftDeleteCompany--  @NotComplete@ [DeleteCompanyProcessor]. " +
          $"@innerfault:{e?.Message} and {e?.InnerException}");
    }
    catch (CompanyDoesNotExistAfterMadePersistentException ex)
    {
      string errorMessage = "ERROR_COMPANY_DOES_NOT_MADE_PERSISTENCE";
      bc.Model = null;
      bc.AddBrokenRule(new BusinessError(errorMessage));
      Log.Error(
          $"Update-Delete Company: Id: {deleteCommand!.CompanyIdToBeDeleted}" +
          $"Error Message:{errorMessage}" +
          "--SoftDeleteCompany--  @NotComplete@ [DeleteCompanyProcessor]. " +
          $"@innerfault:{ex?.Message} and {ex?.InnerException}");
    }
    catch (Exception exx)
    {
      string errorMessage = "UNKNOWN_ERROR";
      bc.Model = null;
      bc.AddBrokenRule(new BusinessError(errorMessage));
      Log.Error(
          $"Update-Delete Company: Id: {deleteCommand!.CompanyIdToBeDeleted}" +
          $"Error Message:{errorMessage}" +
          $"--SoftDeleteCompany--  @fail@ [DeleteCompanyProcessor]. " +
          $"@innerfault:{exx.Message} and {exx.InnerException}");
    }

    return await Task.FromResult(bc);
  }

  private CompanyDeletionUiModel ThrowExcIfCompanyWasNotBeMadePersistent(Company companyToBeSoftDeleted)
  {
    var retrievedCompany =
      _companyRepository.FindBy(companyToBeSoftDeleted.Id);
    if (retrievedCompany != null)
      return _autoMapper.Map<CompanyDeletionUiModel>(retrievedCompany);
    throw new CompanyDoesNotExistAfterMadePersistentException(companyToBeSoftDeleted.Id);
  }

  private void MakeCompanyPersistent(Company companyToBeUpdated)
  {
    _companyRepository.Save(companyToBeUpdated);
    _uOf.Commit();
  }
}