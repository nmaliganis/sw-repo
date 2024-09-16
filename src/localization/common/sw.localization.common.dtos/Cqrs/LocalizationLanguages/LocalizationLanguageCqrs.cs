using sw.localization.common.dtos.Vms.LocalizationLanguages;
using sw.infrastructure.BrokenRules;
using MediatR;
using OneOf;
using System;

namespace sw.localization.common.dtos.Cqrs.LocalizationLanguages
{
    // Queries

    // Commands
    public record CreateLocalizationLanguageCommand(string Lang)
        : IRequest<BusinessResult<LocalizationLanguageCreationUiModel>>;

    public record HardDeleteLocalizationLanguageCommand(long Id)
        : IRequest<BusinessResult<LocalizationLanguageDeletionUiModel>>;

}
