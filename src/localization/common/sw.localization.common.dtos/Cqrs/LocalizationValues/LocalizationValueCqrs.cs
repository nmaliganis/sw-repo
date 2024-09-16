using sw.localization.common.dtos.ResourceParameters.LocalizationValues;
using sw.localization.common.dtos.Vms.LocalizationValues;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;
using OneOf;
using System;
using System.ComponentModel.DataAnnotations;

namespace sw.localization.common.dtos.Cqrs.LocalizationValues
{
    // Queries
    public record GetLocalizationValueByIdQuery(long Id) : IRequest<LocalizationValueUiModel>;
    public record GetLocalizationValueByKeyQuery(string Key, string Domain, string Lang) : IRequest<BusinessResult<LocalizationValueUiModel>>;

    public class GetLocalizationValuesQuery : GetLocalizationValuesResourceParameters, IRequest<PagedList<LocalizationValueUiModel>>
    {
        public GetLocalizationValuesQuery(GetLocalizationValuesResourceParameters parameters) : base()
        {
            Filter = parameters.Filter;
            SearchQuery = parameters.SearchQuery;
            Fields = parameters.Fields;
            OrderBy = parameters.OrderBy;
            PageSize = parameters.PageSize;
            PageIndex = parameters.PageIndex;
        }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Domain { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Language { get; set; }
    }

    // Commands
    public record CreateLocalizationValueCommand(string Key, string Value, string Domain, string Lang)
        : IRequest<OneOf<LocalizationValueCreationUiModel, Exception>>;

    public record UpdateLocalizationValueCommand(long Id, string Value)
        : IRequest<OneOf<LocalizationValueModificationUiModel, Exception>>;

    public record DeleteLocalizationValueCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<OneOf<LocalizationValueDeletionUiModel, Exception>>;

    public record HardDeleteLocalizationValueCommand(long Id)
        : IRequest<OneOf<LocalizationValueDeletionUiModel, Exception>>;

}
