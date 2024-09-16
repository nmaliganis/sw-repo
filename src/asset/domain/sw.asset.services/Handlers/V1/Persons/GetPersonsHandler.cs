using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Persons;
using sw.asset.common.dtos.Vms.Persons;
using sw.asset.contracts.V1.PersonProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.asset.services.Handlers.V1.Persons;

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
