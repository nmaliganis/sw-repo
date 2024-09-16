using sw.auth.common.dtos.Cqrs.Companies;
using sw.auth.common.dtos.Vms.Companies;
using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Companies;
using sw.auth.model.Companies;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
using sw.auth.common.infrastructure.Exceptions.Companies;

namespace sw.auth.services.V1.Companies;

public class DeleteHardCompanyProcessor :
  IDeleteHardCompanyProcessor,
  IRequestHandler<DeleteHardCompanyCommand, BusinessResult<CompanyDeletionUiModel>>
{
  private readonly IUnitOfWork _uOf;
  private readonly ICompanyRepository _companyRepository;

  public DeleteHardCompanyProcessor(IUnitOfWork uOf, ICompanyRepository companyRepository)
  {
    _uOf = uOf;
    _companyRepository = companyRepository;
  }

  public async Task<BusinessResult<CompanyDeletionUiModel>> Handle(DeleteHardCompanyCommand deleteCommand, CancellationToken cancellationToken)
  {
    return await DeleteHardCompanyAsync(deleteCommand);
  }

  public async Task<BusinessResult<CompanyDeletionUiModel>> DeleteHardCompanyAsync(DeleteHardCompanyCommand deleteCommand)
  {
    var bc = new BusinessResult<CompanyDeletionUiModel>(new CompanyDeletionUiModel());

    if (deleteCommand.IsNull() || deleteCommand.CompanyIdToBeDeleted <= 0) {
      bc.Model = null;
      bc.AddBrokenRule(new BusinessError("ERROR_DELETE_COMMAND_MODEL"));
      return await Task.FromResult(bc);
    }

    try
    {
      var companyToBeHardDeleted = _companyRepository.FindBy(deleteCommand!.CompanyIdToBeDeleted);

      if (companyToBeHardDeleted == null)
        throw new CompanyDoesNotExistException(deleteCommand!.CompanyIdToBeDeleted);

      Log.Debug(
          $"Update-Delete Company: with Id: {deleteCommand!.CompanyIdToBeDeleted}" +
          "--HardDeleteCompany--  @Ready@ [DeleteCompanyProcessor]. " +
          "Message: Just Before MakeItPersistence");

      MakeCompanyTransient(companyToBeHardDeleted);

      Log.Debug(
          $"Update-Delete Company: with Id: {deleteCommand!.CompanyIdToBeDeleted}" +
          "--HardDeleteCompany--  @Ready@ [DeleteCompanyProcessor]. " +
          "Message: Just After MakeItPersistence");

      bc.Model.DeletionStatus = ThrowExcIfCompanyWasNotBeMadeTransient(companyToBeHardDeleted);
      bc.Model.Message = "SUCCESS_DELETION";

    }
    catch (CompanyDoesNotExistException e)
    {
      string errorMessage = "ERROR_COMPANY_DOES_NOT_EXIST";
      bc.Model.Message = errorMessage;
      bc.AddBrokenRule(new BusinessError(errorMessage));
      Log.Error(
          $"Delete Company: Id: {deleteCommand!.CompanyIdToBeDeleted}" +
          $"Error Message:{errorMessage}" +
          "--HardDeleteCompany--  @NotComplete@ [DeleteCompanyProcessor]. " +
          $"@innerfault:{e?.Message} and {e?.InnerException}");
    }
    catch (CompanyDoesExistAfterMadeTransientException ex)
    {
      string errorMessage = "ERROR_COMPANY_DOES_NOT_MADE_TRANSIENT";
      bc.Model = null;
      bc.AddBrokenRule(new BusinessError(errorMessage));
      Log.Error(
          $"Delete Company: Id: {deleteCommand!.CompanyIdToBeDeleted}" +
          $"Error Message:{errorMessage}" +
          "--HardDeleteCompany--  @NotComplete@ [DeleteCompanyProcessor]. " +
          $"@innerfault:{ex?.Message} and {ex?.InnerException}");
    }
    catch (Exception exxx)
    {
      string errorMessage = "UNKNOWN_ERROR";
      bc.Model = null;
      bc.AddBrokenRule(new BusinessError(errorMessage));
      Log.Error(
          $"Delete Company: Id: {deleteCommand!.CompanyIdToBeDeleted}" +
          $"Error Message:{errorMessage}" +
          $"--HardDeleteCompany--  @fail@ [DeleteCompanyProcessor]. " +
          $"@innerfault:{exxx.Message} and {exxx.InnerException}");
    }

    return await Task.FromResult(bc);
  }

  private bool ThrowExcIfCompanyWasNotBeMadeTransient(Company companyToBeSoftDeleted)
  {
    var retrievedCompany =
      _companyRepository.FindBy(companyToBeSoftDeleted.Id);
    return retrievedCompany != null
      ? throw new CompanyDoesExistAfterMadeTransientException(companyToBeSoftDeleted.Id)
      : true;
  }

  private void MakeCompanyTransient(Company companyToBeSoftDeleted)
  {
    _companyRepository.Remove(companyToBeSoftDeleted);
    _uOf.Commit();
  }
}