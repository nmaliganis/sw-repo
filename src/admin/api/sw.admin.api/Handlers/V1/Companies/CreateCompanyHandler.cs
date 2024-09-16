using System.Threading;
using System.Threading.Tasks;
using sw.admin.common.dtos.V1.Cqrs.Companies;
using sw.admin.common.dtos.V1.Vms.Companies;
using sw.admin.contracts.V1.CompanyProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.admin.api.Handlers.V1.Companies
{
    internal class CreateCompanyHandler :
        IRequestHandler<CreateCompanyCommand, BusinessResult<CompanyCreationUiModel>>
    {
        private readonly ICreateCompanyProcessor _processor;

        public CreateCompanyHandler(ICreateCompanyProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<CompanyCreationUiModel>> Handle(CreateCompanyCommand createCommand, CancellationToken cancellationToken)
        {
            return await _processor.CreateCompanyAsync(createCommand);
        }
    }
}
