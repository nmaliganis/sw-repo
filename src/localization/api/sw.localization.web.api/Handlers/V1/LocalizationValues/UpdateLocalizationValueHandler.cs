using sw.localization.common.dtos.Cqrs.LocalizationValues;
using sw.localization.common.dtos.Vms.LocalizationValues;
using sw.localization.contracts.V1.LocalizationValueProcessors;
using MediatR;
using OneOf;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace sw.localization.api.Handlers.V1.LocalizationValues;

internal class UpdateLocalizationValueHandler :
    IRequestHandler<UpdateLocalizationValueCommand, OneOf<LocalizationValueModificationUiModel, Exception>>
{
    private readonly IUpdateLocalizationValueProcessor _processor;

    public UpdateLocalizationValueHandler(IUpdateLocalizationValueProcessor processor)
    {
        _processor = processor;
    }

    public async Task<OneOf<LocalizationValueModificationUiModel, Exception>> Handle(UpdateLocalizationValueCommand updateCommand, CancellationToken cancellationToken)
    {
        return await _processor.UpdateLocalizationValueAsync(updateCommand);
    }
}