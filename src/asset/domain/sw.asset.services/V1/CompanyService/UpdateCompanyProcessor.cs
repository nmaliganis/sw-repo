using System;
using sw.asset.common.dtos.Cqrs.Companies;
using sw.asset.common.dtos.Vms.Companies;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.CompanyProcessors;
using sw.asset.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;
using sw.asset.model.Companies;
using sw.infrastructure.Extensions;
using MediatR;
using Serilog;
using sw.asset.model.Devices.Simcards;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.common.infrastructure.Exceptions.Assets.Containers;
using sw.asset.common.infrastructure.Exceptions.Companies;

namespace sw.asset.services.V1.CompanyService
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

            if (updateCommand.IsNull())
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var company = _companyRepository.FindBy(updateCommand.Id);
            if (company.IsNull())
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "Company Id does not exist"));
                return bc;
            }

            company.Modified(updateCommand.ModifiedById, updateCommand.Name, updateCommand.CodeErp, updateCommand.Description);

            Persist(company, updateCommand.Id);

            var response = _autoMapper.Map<CompanyModificationUiModel>(company);
            response.Message = $"Company id: {response.Id} updated successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        public async Task<BusinessResult<CompanyModificationUiModel>> UpdateCompanyWithZoneAsync(UpdateCompanyWithZoneCommand updateCommand)
        {
            var bc = new BusinessResult<CompanyModificationUiModel>(new CompanyModificationUiModel());

            try
            {
                var companyToBeUpdated = _companyRepository.FindBy(updateCommand.CompanyId);

                Log.Debug(
                    $"Update Company with Zones: {updateCommand.CompanyId}" +
                    "--UpdateCompanyWithZoneAsync--  @NotComplete@ [UpdateCompanyWithZoneAsync]. " +
                    "Message: Just Before MakeItPersistence");

                MakeCompanyPersistent(companyToBeUpdated);

                Log.Debug(
                    $"Update Company with Zones: {updateCommand.CompanyId}" +
                    "--UpdateCompanyWithZoneAsync--  @NotComplete@ [UpdateCompanyProcessor]. " +
                    "Message: Just After MakeItPersistence");
                bc.Model = ThrowExcIfCompanyWasNotBeMadePersistent(companyToBeUpdated);
                bc.Model.Message = "SUCCESS_COMPANY_ZONE_CREATION";
            }
            catch (Exception exxx)
            {
                string errorMessage = "UNKNOWN_ERROR";
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError(errorMessage));
                Log.Error(
                    $"Update Company with Zones: {updateCommand.CompanyId}" +
                    $"Error Message:{errorMessage}" +
                    $"--UpdateCompanyWithZoneAsync--  @fail@ [UpdateCompanyProcessor]. " +
                    $"@innerfault:{exxx.Message} and {exxx.InnerException}");
            }

            return await Task.FromResult(bc);
        }

        private CompanyModificationUiModel ThrowExcIfCompanyWasNotBeMadePersistent(Company companyToBeCreated)
        {
            var retrievedCompany = this._companyRepository.FindBy(companyToBeCreated.Id);
            if (!retrievedCompany.IsNull())
            {
                return this._autoMapper.Map<CompanyModificationUiModel>(retrievedCompany);
            }

            throw new CompanyDoesNotExistAfterMadePersistentException(companyToBeCreated.Name);
        }

        private void Persist(Company company, long id)
        {
            _companyRepository.Save(company, id);
            _uOf.Commit();
        }

        private void MakeCompanyPersistent(Company companyToBeUpdated)
        {
            this._companyRepository.Save(companyToBeUpdated);
            this._uOf.Commit();
        }

    }
}
