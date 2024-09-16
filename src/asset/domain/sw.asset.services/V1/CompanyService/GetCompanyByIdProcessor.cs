using sw.asset.common.dtos.Vms.Companies;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.CompanyProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using System.Threading.Tasks;
using sw.asset.common.dtos.Vms.Companies.Zones;
using sw.infrastructure.Extensions;

namespace sw.asset.services.V1.CompanyService
{
    public class GetCompanyByIdProcessor :
        IGetCompanyByIdProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly IZoneRepository _zoneRepository;
        public GetCompanyByIdProcessor(ICompanyRepository companyRepository, IZoneRepository zoneRepository, IAutoMapper autoMapper)
        {
            _companyRepository = companyRepository;
            _zoneRepository = zoneRepository;
            _autoMapper = autoMapper;
        }

        public async Task<BusinessResult<CompanyUiModel>> GetCompanyByIdAsync(long id)
        {
            var bc = new BusinessResult<CompanyUiModel>(new CompanyUiModel());

            var company = _companyRepository.FindActiveById(id);
            if (company.IsNull())
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
