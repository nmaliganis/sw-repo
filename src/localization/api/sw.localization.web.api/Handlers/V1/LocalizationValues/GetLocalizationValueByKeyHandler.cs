using sw.localization.common.dtos.Cqrs.LocalizationValues;
using sw.localization.common.dtos.Vms.LocalizationValues;
using sw.localization.contracts.V1.LocalizationValueProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.localization.api.Handlers.V1.LocalizationValues;

internal class GetLocalizationValueByKeyHandler :
    IRequestHandler<GetLocalizationValueByKeyQuery, BusinessResult<LocalizationValueUiModel>>
{
    private readonly IGetLocalizationValueByKeyProcessor _processor;

    public GetLocalizationValueByKeyHandler(IGetLocalizationValueByKeyProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<LocalizationValueUiModel>> Handle(GetLocalizationValueByKeyQuery qry, CancellationToken cancellationToken)
    {
        var bc = new BusinessResult<LocalizationValueUiModel>(new LocalizationValueUiModel());
        var fetchedLocalizationValueUiModel = await _processor.GetLocalizationValueByKey(qry.Key, qry.Domain, qry.Lang);
        bc.Model = fetchedLocalizationValueUiModel;
        return bc;
    }
}