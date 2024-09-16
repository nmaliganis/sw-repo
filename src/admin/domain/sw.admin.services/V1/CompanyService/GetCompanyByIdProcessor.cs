using sw.admin.common.dtos.V1.Vms.Companies;
using sw.admin.contracts.ContractRepositories;
using sw.admin.contracts.V1.CompanyProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using System.Threading.Tasks;

namespace sw.admin.services.V1.CompanyService
{
    public class GetCompanyByIdProcessor : IGetCompanyByIdProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly ICompanyRepository _companyRepository;

        public GetCompanyByIdProcessor(ICompanyRepository companyRepository, IAutoMapper autoMapper)
        {
            _companyRepository = companyRepository;
            _autoMapper = autoMapper;
        }

        public async Task<BusinessResult<CompanyUiModel>> GetCompanyByIdAsync(long id)
        {
            var bc = new BusinessResult<CompanyUiModel>(new CompanyUiModel());

            var company = _companyRepository.FindActiveBy(id);
            if (company is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "Company Id does not exist"));
                return bc;
            }

            var response = _autoMapper.Map<CompanyUiModel>(company);
            response.Message = $"Company id: {response.Id} fetched successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }
    }
}
