using sw.admin.common.dtos.V1.Cqrs.Persons;
using sw.admin.common.dtos.V1.Vms.Persons;
using sw.admin.contracts.V1.PersonProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.admin.api.Handlers.V1.Persons
{
    internal class CreatePersonHandler :
        IRequestHandler<CreatePersonCommand, BusinessResult<PersonCreationUiModel>>
    {
        private readonly ICreatePersonProcessor _processor;

        public CreatePersonHandler(ICreatePersonProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<PersonCreationUiModel>> Handle(CreatePersonCommand createCommand, CancellationToken cancellationToken)
        {
            return await _processor.CreatePersonAsync(createCommand);
        }
    }
}
