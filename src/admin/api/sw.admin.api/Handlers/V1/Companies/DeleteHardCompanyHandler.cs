using System.Threading;
using System.Threading.Tasks;
using sw.admin.common.dtos.V1.Cqrs.Companies;
using sw.admin.common.dtos.V1.Vms.Companies;
using sw.admin.contracts.V1.CompanyProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.admin.api.Handlers.V1.Companies
{
    internal class DeleteHardCompanyHandler :
        IRequestHandler<DeleteHardCompanyCommand, BusinessResult<CompanyDeletionUiModel>>
    {
        private readonly IDeleteHardCompanyProcessor _processor;

        public DeleteHardCompanyHandler(IDeleteHardCompanyProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<CompanyDeletionUiModel>> Handle(DeleteHardCompanyCommand deleteCommand, CancellationToken cancellationToken)
        {
            return await _processor.DeleteHardCompanyAsync(deleteCommand);
        }
    }
}
