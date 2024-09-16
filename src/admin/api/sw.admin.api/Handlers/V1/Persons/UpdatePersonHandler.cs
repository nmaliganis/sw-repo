using sw.admin.common.dtos.V1.Cqrs.Persons;
using sw.admin.common.dtos.V1.Vms.Persons;
using sw.admin.contracts.V1.PersonProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.admin.api.Handlers.V1.Persons
{
    internal class UpdatePersonHandler :
        IRequestHandler<UpdatePersonCommand, BusinessResult<PersonModificationUiModel>>
    {
        private readonly IUpdatePersonProcessor _processor;

        public UpdatePersonHandler(IUpdatePersonProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<PersonModificationUiModel>> Handle(UpdatePersonCommand updateCommand, CancellationToken cancellationToken)
        {
            return await _processor.UpdatePersonAsync(updateCommand);
        }
    }
}
