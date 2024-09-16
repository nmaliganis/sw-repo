using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Persons;
using sw.asset.common.dtos.Vms.Persons;
using sw.asset.contracts.V1.PersonProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Persons;

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