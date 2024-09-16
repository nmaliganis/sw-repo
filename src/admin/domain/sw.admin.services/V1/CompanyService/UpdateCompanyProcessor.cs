using sw.admin.common.dtos.V1.Cqrs.Companies;
using sw.admin.common.dtos.V1.Vms.Companies;
using sw.admin.contracts.ContractRepositories;
using sw.admin.contracts.V1.CompanyProcessors;
using sw.admin.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;

namespace sw.admin.services.V1.CompanyService
{
    public class UpdateCompanyProcessor : IUpdateCompanyProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly ICompanyRepository _companyRepository;
        private readonly IAutoMapper _autoMapper;
        public UpdateCompanyProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            ICompanyRepository companyRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _companyRepository = companyRepository;
        }

        public async Task<BusinessResult<CompanyModificationUiModel>> UpdateCompanyAsync(UpdateCompanyCommand updateCommand)
        {
            var bc = new BusinessResult<CompanyModificationUiModel>(new CompanyModificationUiModel());

            if (updateCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var company = _companyRepository.FindBy(updateCommand.Id);
            if (company is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "Company Id does not exist"));
                return bc;
            }

            var modifiedCompany = _autoMapper.Map<Company>(updateCommand);
            company.Modified(updateCommand.ModifiedById, modifiedCompany);

            Persist(company, updateCommand.Id);

            var response = _autoMapper.Map<CompanyModificationUiModel>(company);
            response.Message = $"Company id: {response.Id} updated successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(Company company, long id)
        {
            _companyRepository.Save(company, id);
            _uOf.Commit();
        }
    }
}
