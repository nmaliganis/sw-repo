using System;
using System.Threading;
using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Companies;
using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Companies;
using sw.common.dtos.Vms.Companies;
using sw.onboarding.model.Companies;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using Serilog;

namespace sw.onboarding.services.V1.Companies;

public class UpdateCompanyProcessor : IUpdateCompanyProcessor,
    IRequestHandler<UpdateCompanyCommand, BusinessResult<CompanyModificationUiModel>> {
    private readonly IUnitOfWork _uOf;
    private readonly ICompanyRepository _companyRepository;
    private readonly IAutoMapper _autoMapper;
    public UpdateCompanyProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
        ICompanyRepository companyRepository) {
        this._uOf = uOf;
        this._autoMapper = autoMapper;
        this._companyRepository = companyRepository;
    }

    public async Task<BusinessResult<CompanyModificationUiModel>> Handle(UpdateCompanyCommand updateCommand, CancellationToken cancellationToken) {
        return await this.UpdateCompanyAsync(updateCommand);
    }

    public async Task<BusinessResult<CompanyModificationUiModel>> UpdateCompanyAsync(UpdateCompanyCommand updateCommand) {
        var bc = new BusinessResult<CompanyModificationUiModel>(new CompanyModificationUiModel());

        if (updateCommand.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_UPDATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }

        try
        {
            var company = _companyRepository.FindBy(updateCommand.Id);
            if (company is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "Company Id does not exist"));
                return bc;
            }

            company.ModifyWithAudit(updateCommand.ModifiedById, updateCommand);


            Persist(company, updateCommand.Id);

            var response = _autoMapper.Map<CompanyModificationUiModel>(company);

            response.Message = $"Company id: {response.Id} updated successfully";

            bc.Model = response;
        }
        catch (Exception e)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
                $"Update Company: {updateCommand.Name}" +
                $"Error Message:{errorMessage}" +
                $"--UpdateCompany--  @fail@ [UpdateCompanyProcessor]. " +
                $"@innerfault:{e.Message} and {e.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    private void Persist(Company company, long id) {
        this._companyRepository.Save(company, id);
        this._uOf.Commit();
    }
}