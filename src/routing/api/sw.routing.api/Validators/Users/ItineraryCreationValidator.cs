using sw.routing.common.dtos.Vms.Itineraries;
using FluentValidation;

namespace sw.routing.api.Validators.Users;

/// <summary>
/// Class : ItineraryCreationValidator
/// </summary>
public class ItineraryCreationValidator : AbstractValidator<ItineraryCreationUiModel>
{
    /// <summary>
    /// Constructor : UserForRegistrationValidator
    /// </summary>
    public ItineraryCreationValidator()
    {
        RuleFor(x => x.Name).NotNull();
    }
}// Class : UserForRegistrationValidator