using sw.asset.common.dtos.Cqrs.Companies;
using sw.asset.common.dtos.Vms.Companies;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.CompanyProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;
using sw.asset.model.Companies;

namespace sw.asset.services.V1.CompanyService;

public class DeleteHardCompanyProcessor :
  IDeleteHardCompanyProcessor
{
  private readonly IUnitOfWork _uOf;
  private readonly ICompanyRepository _companyRepository;

  public DeleteHardCompanyProcessor(IUnitOfWork uOf, ICompanyRepository companyRepository)
  {
    _uOf = uOf;
    _companyRepository = companyRepository;
  }

  public async Task<BusinessResult<CompanyDeletionUiModel>> DeleteHardCompanyAsync(DeleteHardCompanyCommand deleteCommand)
  {
    var bc = new BusinessResult<CompanyDeletionUiModel>(new CompanyDeletionUiModel());

    if (deleteCommand is null)
    {
      bc.AddBrokenRule(new BusinessError(null));
    }

    var company = _companyRepository.FindBy(deleteCommand.Id);
    if (company is null)
    {
      bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "Company Id does not exist"));
      return bc;
    }

    Persist(company);

    bc.Model.Id = deleteCommand.Id;
    bc.Model.Successful = true;
    bc.Model.Hard = true;
    bc.Model.Message = $"Company with id: {deleteCommand.Id} has been hard deleted successfully.";

    return await Task.FromResult(bc);
  }

  private void Persist(Company company)
  {
    _companyRepository.Remove(company);
    _uOf.Commit();
  }
}