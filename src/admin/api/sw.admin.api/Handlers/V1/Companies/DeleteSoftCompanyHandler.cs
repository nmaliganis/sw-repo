using System.Threading;
using System.Threading.Tasks;
using sw.admin.common.dtos.V1.Cqrs.Companies;
using sw.admin.common.dtos.V1.Vms.Companies;
using sw.admin.contracts.V1.CompanyProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.admin.api.Handlers.V1.Companies
{
    internal class DeleteSoftCompanyHandler :
        IRequestHandler<DeleteSoftCompanyCommand, BusinessResult<CompanyDeletionUiModel>>
    {
        private readonly IDeleteSoftCompanyProcessor _processor;

        public DeleteSoftCompanyHandler(IDeleteSoftCompanyProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<CompanyDeletionUiModel>> Handle(DeleteSoftCompanyCommand deleteCommand, CancellationToken cancellationToken)
        {
            return await _processor.DeleteSoftCompanyAsync(deleteCommand);
        }
    }
}
