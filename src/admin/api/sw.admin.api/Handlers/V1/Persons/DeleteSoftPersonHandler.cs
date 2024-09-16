using sw.admin.common.dtos.V1.Cqrs.Persons;
using sw.admin.common.dtos.V1.Vms.Persons;
using sw.admin.contracts.V1.PersonProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.admin.api.Handlers.V1.Persons
{
    internal class DeleteSoftPersonHandler :
        IRequestHandler<DeleteSoftPersonCommand, BusinessResult<PersonDeletionUiModel>>
    {
        private readonly IDeleteSoftPersonProcessor _processor;

        public DeleteSoftPersonHandler(IDeleteSoftPersonProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<PersonDeletionUiModel>> Handle(DeleteSoftPersonCommand deleteCommand, CancellationToken cancellationToken)
        {
            return await _processor.DeleteSoftPersonAsync(deleteCommand);
        }
    }
}
