using sw.localization.common.dtos.Vms.LocalizationDomains;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.localization.common.dtos.Cqrs.LocalizationDomains
{
    // Queries

    // Commands
    public record CreateLocalizationDomainCommand(string Domain)
        : IRequest<BusinessResult<LocalizationDomainCreationUiModel>>;

    public record HardDeleteLocalizationDomainCommand(long Id)
        : IRequest<BusinessResult<LocalizationDomainDeletionUiModel>>;

}
