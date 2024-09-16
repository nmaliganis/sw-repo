using System.Threading;
using System.Threading.Tasks;
using sw.admin.common.dtos.V1.Cqrs.Companies;
using sw.admin.common.dtos.V1.Vms.Companies;
using sw.admin.contracts.V1.CompanyProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.admin.api.Handlers.V1.Companies
{
    internal class UpdateCompanyHandler :
        IRequestHandler<UpdateCompanyCommand, BusinessResult<CompanyModificationUiModel>>
    {
        private readonly IUpdateCompanyProcessor _processor;

        public UpdateCompanyHandler(IUpdateCompanyProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<CompanyModificationUiModel>> Handle(UpdateCompanyCommand updateCommand, CancellationToken cancellationToken)
        {
            return await _processor.UpdateCompanyAsync(updateCommand);
        }
    }
}
