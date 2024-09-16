using sw.admin.common.dtos.V1.Cqrs.Persons;
using sw.admin.common.dtos.V1.Vms.Persons;
using sw.admin.contracts.V1.PersonProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.admin.api.Handlers.V1.Persons
{
    internal class GetPersonsHandler :
        IRequestHandler<GetPersonsQuery, BusinessResult<PagedList<PersonUiModel>>>
    {
        private readonly IGetPersonsProcessor _processor;

        public GetPersonsHandler(IGetPersonsProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<PagedList<PersonUiModel>>> Handle(GetPersonsQuery qry, CancellationToken cancellationToken)
        {
            return await _processor.GetPersonsAsync(qry);
        }
    }
}
