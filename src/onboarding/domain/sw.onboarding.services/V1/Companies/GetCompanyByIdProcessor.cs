using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Companies;
using sw.common.dtos.Vms.Companies;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using MediatR;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Companies;
using sw.auth.common.dtos.Vms.Companies;
using sw.infrastructure.Extensions;

namespace sw.auth.services.V1.Companies {
    public class GetCompanyByIdProcessor : IGetCompanyByIdProcessor,
        IRequestHandler<GetCompanyByIdQuery, BusinessResult<CompanyUiModel>> {
        private readonly IAutoMapper _autoMapper;
        private readonly ICompanyRepository _companyRepository;
        public GetCompanyByIdProcessor(ICompanyRepository companyRepository, IAutoMapper autoMapper) {
            this._companyRepository = companyRepository;
            this._autoMapper = autoMapper;
        }

        public async Task<BusinessResult<CompanyUiModel>> GetCompanyByIdAsync(long id) {
            var bc = new BusinessResult<CompanyUiModel>(new CompanyUiModel());

            var company = this._companyRepository.FindActiveById(id);

            // Business logic about Active Companies. 
            // TODO
            // Refactor so soft deleted companies are defined


            if (company.IsNull()) {
                Log.Information(
                    $"--Method:GetCompanyByIdAsync -- Message:COMPANY_FETCH" +
                    $" -- Datetime:{DateTime.Now} -- Just After : _companyRepository.FindBy");
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "Company Id does not exist"));
                return bc;
            }

            var response = this._autoMapper.Map<CompanyUiModel>(company);
            response.Message = $"Company id: {response.Id} fetched successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        public async Task<BusinessResult<CompanyUiModel>> Handle(GetCompanyByIdQuery qry, CancellationToken cancellationToken) {
            return await this.GetCompanyByIdAsync(qry.Id);
        }
    }//Class : GetCompanyByIdProcessor

}//Namespace : sw.auth.services.V1.Companies
