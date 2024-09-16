using sw.localization.common.dtos.Cqrs.LocalizationValues;
using sw.localization.common.dtos.Vms.LocalizationLanguages;
using sw.localization.common.dtos.Vms.LocalizationValues;
using sw.infrastructure.Paging;
using System.Threading.Tasks;

namespace sw.localization.contracts.V1.LocalizationValueProcessors
{
    public interface IGetLocalizationValuesProcessor
    {
        Task<PagedList<LocalizationValueUiModel>> GetLocalizationValuesAsync(GetLocalizationValuesQuery qry);
    }
}