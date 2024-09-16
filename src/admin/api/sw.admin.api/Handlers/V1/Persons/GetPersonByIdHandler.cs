using sw.admin.common.dtos.V1.Cqrs.Persons;
using sw.admin.common.dtos.V1.Vms.Persons;
using sw.admin.contracts.V1.PersonProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.admin.api.Handlers.V1.Persons
{
    internal class GetPersonByIdHandler :
        IRequestHandler<GetPersonByIdQuery, BusinessResult<PersonUiModel>>
    {
        private readonly IGetPersonByIdProcessor _processor;

        public GetPersonByIdHandler(IGetPersonByIdProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<PersonUiModel>> Handle(GetPersonByIdQuery qry, CancellationToken cancellationToken)
        {
            return await _processor.GetPersonByIdAsync(qry.Id);
        }
    }
}
