using sw.localization.common.dtos.Cqrs.LocalizationValues;
using sw.localization.common.dtos.Vms.LocalizationValues;
using sw.localization.contracts.V1.LocalizationValueProcessors;
using MediatR;
using OneOf;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace sw.localization.api.Handlers.V1.LocalizationValues;

internal class CreateLocalizationValueHandler :
    IRequestHandler<CreateLocalizationValueCommand, OneOf<LocalizationValueCreationUiModel, Exception>>
{
    private readonly ICreateLocalizationValueProcessor _processor;

    public CreateLocalizationValueHandler(ICreateLocalizationValueProcessor processor)
    {
        _processor = processor;
    }

    public async Task<OneOf<LocalizationValueCreationUiModel, Exception>> Handle(CreateLocalizationValueCommand createCommand, CancellationToken cancellationToken)
    {
        return await _processor.CreateLocalizationValueAsync(createCommand);
    }
}