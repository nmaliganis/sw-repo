using sw.localization.common.dtos.Vms.LocalizationLanguages;
using sw.localization.common.dtos.Vms.LocalizationValues;
using System.Threading.Tasks;

namespace sw.localization.contracts.V1.LocalizationValueProcessors
{
    public interface IInquiryLocalizationValueProcessor
    {
        Task<LocalizationValueUiModel> GetLocalizationValueByKeyAsync(string domain, string language, string key);
    }
}