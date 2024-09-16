using sw.localization.common.dtos.Vms.LocalizationLanguages;
using sw.localization.common.dtos.Vms.LocalizationValues;
using System.Threading.Tasks;

namespace sw.localization.contracts.V1.LocalizationValueProcessors
{
    public interface IGetLocalizationValueByKeyProcessor
    {
        Task<LocalizationValueUiModel> GetLocalizationValueByKey(string key, string domain, string lang);
    }
}