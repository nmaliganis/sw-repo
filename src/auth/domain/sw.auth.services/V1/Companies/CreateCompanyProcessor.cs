using sw.auth.common.dtos.Cqrs.Companies;
using sw.auth.common.infrastructure.Exceptions.Companies;
using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Companies;
using sw.auth.model.Companies;
using sw.common.dtos.Vms.Companies;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using sw.auth.common.dtos.Vms.Companies;

namespace sw.auth.services.V1.Companies
{
  public class CreateCompanyProcessor :
        ICreateCompanyProcessor,
        IRequestHandler<CreateCompanyCommand, BusinessResult<CompanyUiModel>>
  {
    private readonly IUnitOfWork _uOf;
    private readonly ICompanyRepository _companyRepository;
    private readonly IAutoMapper _autoMapper;

    public CreateCompanyProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, ICompanyRepository companyRepository)
    {
      this._uOf = uOf;
      this._autoMapper = autoMapper;
      this._companyRepository = companyRepository;
    }

    public async Task<BusinessResult<CompanyUiModel>> Handle(CreateCompanyCommand createCommand, CancellationToken cancellationToken)
    {
      return await this.CreateCompanyAsync(createCommand);
    }

    public async Task<BusinessResult<CompanyUiModel>> CreateCompanyAsync(CreateCompanyCommand createCommand)
    {
      var bc = new BusinessResult<CompanyUiModel>(new CompanyUiModel());

      if (createCommand.IsNull())
      {
        bc.Model = null;
        bc.AddBrokenRule(new BusinessError("ERROR_CREATE_COMMAND_MODEL"));
        return await Task.FromResult(bc);
      }

      try
      {
        var companyToBeCreated = new Company();

        companyToBeCreated.InjectWithInitialAttributes(createCommand);
        companyToBeCreated.InjectWithAudit(createCommand.CreatedById);

        this.ThrowExcIfCompanyCannotBeCreated(companyToBeCreated);
        this.ThrowExcIfThisCompanyAlreadyExist(companyToBeCreated);

        Log.Debug(
            $"Create Company: {createCommand.Name}" +
            "--CreateCompany--  @NotComplete@ [CreateCompanyProcessor]. " +
            "Message: Just Before MakeItPersistence");

        MakeCompanyPersistent(companyToBeCreated);

        Log.Debug(
            $"Create Company: {createCommand.Name}" +
            "--CreateCompany--  @NotComplete@ [CreateCompanyProcessor]. " +
            "Message: Just After MakeItPersistence");
        bc.Model = ThrowExcIfCompanyWasNotBeMadePersistent(companyToBeCreated);
        bc.Model.Message = "SUCCESS_CREATION";
      }
      catch (InvalidCompanyException e)
      {
        string errorMessage = "ERROR_INVALID_COMPANY_MODEL";
        bc.Model.Message = errorMessage;
        bc.AddBrokenRule(new BusinessError(errorMessage));
        Log.Error(
            $"Create Company: {createCommand.Name}" +
            $"Error Message:{errorMessage}" +
            "--CreateCompany--  @NotComplete@ [CreateCompanyProcessor]. " +
            $"Broken rules: {e.BrokenRules}");
      }
      catch (CompanyAlreadyExistsException ex)
      {
        string errorMessage = "ERROR_COMPANY_ALREADY_EXISTS";
        bc.Model.Message = errorMessage;
        bc.AddBrokenRule(new BusinessError(errorMessage));
        Log.Error(
            $"Create Company: {createCommand.Name}" +
            $"Error Message:{errorMessage}" +
            "--CreateCompany--  @fail@ [CreateCompanyProcessor]. " +
            $"@innerfault:{ex?.Message} and {ex?.InnerException}");
      }
      catch (CompanyDoesNotExistAfterMadePersistentException exx)
      {
        string errorMessage = "ERROR_COMPANY_NOT_MADE_PERSISTENT";
        bc.Model.Message = errorMessage;
        bc.AddBrokenRule(new BusinessError(errorMessage));
        Log.Error(
            $"Create Company: {createCommand.Name}" +
            $"Error Message:{errorMessage}" +
            "--CreateCompany--  @fail@ [CreateCompanyProcessor]." +
            $" @innerfault:{exx?.Message} and {exx?.InnerException}");
      }
      catch (Exception exxx)
      {
        string errorMessage = "UNKNOWN_ERROR";
        bc.Model.Message = errorMessage;
        bc.AddBrokenRule(new BusinessError(errorMessage));
        Log.Error(
            $"Create Company: {createCommand.Name}" +
            $"Error Message:{errorMessage}" +
            $"--CreateCompany--  @fail@ [CreateCompanyProcessor]. " +
            $"@innerfault:{exxx.Message} and {exxx.InnerException}");
      }

      return await Task.FromResult(bc);
    }

    private void ThrowExcIfThisCompanyAlreadyExist(Company companyToBeCreated)
    {
      var companyRetrieved = this._companyRepository.FindCompanyByName(companyToBeCreated.Name);
      if (companyRetrieved != null)
      {
        throw new CompanyAlreadyExistsException(companyToBeCreated.Name,
            companyToBeCreated.GetBrokenRulesAsString());
      }
    }

    private CompanyUiModel ThrowExcIfCompanyWasNotBeMadePersistent(Company companyToBeCreated)
    {
      var retrievedCompany = this._companyRepository.FindCompanyByName(companyToBeCreated.Name);
      if (retrievedCompany != null)
      {
        return this._autoMapper.Map<CompanyUiModel>(retrievedCompany);
      }

      throw new CompanyDoesNotExistAfterMadePersistentException(companyToBeCreated.Name);
    }

    private void ThrowExcIfCompanyCannotBeCreated(Company companyToBeCreated)
    {
      bool canBeCreated = !companyToBeCreated.GetBrokenRules().Any();
      if (!canBeCreated)
      {
        throw new InvalidCompanyException(companyToBeCreated.GetBrokenRulesAsString());
      }
    }

    private void MakeCompanyPersistent(Company company)
    {
      this._companyRepository.Save(company);
      this._uOf.Commit();
    }

  }//Class : CreateCompanyProcessor

}//Namespace : sw.auth.services.V1.Companies
