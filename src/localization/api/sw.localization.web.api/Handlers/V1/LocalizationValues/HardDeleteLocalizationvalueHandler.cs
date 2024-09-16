using sw.localization.common.dtos.Cqrs.LocalizationValues;
using sw.localization.common.dtos.Vms.LocalizationValues;
using sw.localization.contracts.V1.LocalizationValueProcessors;
using MediatR;
using OneOf;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace sw.localization.api.Handlers.V1.LocalizationValues;

internal class HardDeleteLocalizationvalueHandler :
    IRequestHandler<HardDeleteLocalizationValueCommand, OneOf<LocalizationValueDeletionUiModel, Exception>>
{
    private readonly IHardDeleteLocalizationValueProcessor _processor;

    public HardDeleteLocalizationvalueHandler(IHardDeleteLocalizationValueProcessor processor)
    {
        _processor = processor;
    }

    public async Task<OneOf<LocalizationValueDeletionUiModel, Exception>> Handle(HardDeleteLocalizationValueCommand deleteCommand, CancellationToken cancellationToken)
    {
        return await _processor.HardDeleteLocalizationValueAsync(deleteCommand);
    }
}