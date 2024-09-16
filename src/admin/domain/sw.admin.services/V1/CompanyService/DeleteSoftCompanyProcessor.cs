using sw.admin.common.dtos.V1.Cqrs.Companies;
using sw.admin.common.dtos.V1.Vms.Companies;
using sw.admin.contracts.ContractRepositories;
using sw.admin.contracts.V1.CompanyProcessors;
using sw.admin.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;

namespace sw.admin.services.V1.CompanyService
{
    public class DeleteSoftCompanyProcessor : IDeleteSoftCompanyProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly ICompanyRepository _companyRepository;

        public DeleteSoftCompanyProcessor(IUnitOfWork uOf, ICompanyRepository companyRepository)
        {
            _uOf = uOf;
            _companyRepository = companyRepository;
        }

        public async Task<BusinessResult<CompanyDeletionUiModel>> DeleteSoftCompanyAsync(DeleteSoftCompanyCommand deleteCommand)
        {
            var bc = new BusinessResult<CompanyDeletionUiModel>(new CompanyDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var company = _companyRepository.FindBy(deleteCommand.Id);
            if (company is null || !company.Active)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "Company Id does not exist"));
                return bc;
            }

            company.Deleted(deleteCommand.DeletedBy, deleteCommand.DeletedReason);

            Persist(company, deleteCommand.Id);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = false;
            bc.Model.Message = $"Company with id: {deleteCommand.Id} has been deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(Company company, long id)
        {
            _companyRepository.Save(company, id);
            _uOf.Commit();
        }
    }
}
