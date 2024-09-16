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
  public class CreateCompanyProcessor : ICreateCompanyProcessor
  {
    private readonly IUnitOfWork _uOf;
    private readonly ICompanyRepository _companyRepository;
    private readonly IAutoMapper _autoMapper;

    public CreateCompanyProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, ICompanyRepository companyRepository)
    {
      _uOf = uOf;
      _autoMapper = autoMapper;
      _companyRepository = companyRepository;
    }

    public async Task<BusinessResult<CompanyCreationUiModel>> CreateCompanyAsync(CreateCompanyCommand createCommand)
    {
      var bc = new BusinessResult<CompanyCreationUiModel>(new CompanyCreationUiModel());

      if (createCommand is null)
      {
        bc.Model = null;
        bc.AddBrokenRule(new BusinessError(null));
        return await Task.FromResult(bc);
      }

      var nameExists = _companyRepository.FindCompanyByName(createCommand.Name);
      if (nameExists != null)
      {
        bc.Model = null;
        bc.AddBrokenRule(BusinessError.CreateInstance(nameof(createCommand.Name), "ERROR_COMPANY_ALREADY_EXISTS"));
        return await Task.FromResult(bc);

      }

      var company = _autoMapper.Map<Company>(createCommand);
      company.Created(createCommand.CreatedById);

      Persist(company);

      var response = _autoMapper.Map<CompanyCreationUiModel>(company);
      response.Message = "Company created successfully.";

      bc.Model = response;

      return await Task.FromResult(bc);
    }

    private void Persist(Company company)
    {
      _companyRepository.Add(company);
      _uOf.Commit();
    }
  }
}
