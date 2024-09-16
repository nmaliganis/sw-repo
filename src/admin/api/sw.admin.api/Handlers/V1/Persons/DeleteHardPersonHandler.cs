using sw.admin.common.dtos.V1.Cqrs.Persons;
using sw.admin.common.dtos.V1.Vms.Persons;
using sw.admin.contracts.V1.PersonProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.admin.api.Handlers.V1.Persons
{
    internal class DeleteHardPersonHandler :
        IRequestHandler<DeleteHardPersonCommand, BusinessResult<PersonDeletionUiModel>>
    {
        private readonly IDeleteHardPersonProcessor _processor;

        public DeleteHardPersonHandler(IDeleteHardPersonProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<PersonDeletionUiModel>> Handle(DeleteHardPersonCommand deleteCommand, CancellationToken cancellationToken)
        {
            return await _processor.DeleteHardPersonAsync(deleteCommand);
        }
    }
}
