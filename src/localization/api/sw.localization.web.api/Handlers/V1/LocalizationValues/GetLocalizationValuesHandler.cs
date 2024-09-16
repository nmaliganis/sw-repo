using sw.localization.common.dtos.Cqrs.LocalizationValues;
using sw.localization.common.dtos.Vms.LocalizationValues;
using sw.localization.contracts.V1.LocalizationValueProcessors;
using sw.infrastructure.Paging;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.localization.api.Handlers.V1.LocalizationValues;

internal class GetLocalizationValuesHandler :
    IRequestHandler<GetLocalizationValuesQuery, PagedList<LocalizationValueUiModel>>
{
    private readonly IGetLocalizationValuesProcessor _processor;

    public GetLocalizationValuesHandler(IGetLocalizationValuesProcessor processor)
    {
        _processor = processor;
    }

    public async Task<PagedList<LocalizationValueUiModel>> Handle(GetLocalizationValuesQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetLocalizationValuesAsync(qry);
    }
}