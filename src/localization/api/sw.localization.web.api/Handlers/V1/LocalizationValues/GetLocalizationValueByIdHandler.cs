using sw.localization.common.dtos.Cqrs.LocalizationValues;
using sw.localization.common.dtos.Vms.LocalizationValues;
using sw.localization.contracts.V1.LocalizationValueProcessors;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.localization.api.Handlers.V1.LocalizationValues;

internal class GetLocalizationValueByIdHandler :
    IRequestHandler<GetLocalizationValueByIdQuery, LocalizationValueUiModel>
{
    private readonly IGetLocalizationValueByIdProcessor _processor;

    public GetLocalizationValueByIdHandler(IGetLocalizationValueByIdProcessor processor)
    {
        _processor = processor;
    }

    public async Task<LocalizationValueUiModel> Handle(GetLocalizationValueByIdQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetLocalizationValueById(qry.Id);
    }
}