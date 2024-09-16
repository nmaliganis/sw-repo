using sw.localization.common.dtos.Vms.LocalizationValues;
using sw.localization.contracts.ContractRepositories;
using sw.localization.contracts.V1.LocalizationValueProcessors;
using sw.infrastructure.TypeMappings;
using System.Threading.Tasks;

namespace sw.localization.services.LocalizationValueService
{
    public class GetLocalizationValueByKeyProcessor :
        IGetLocalizationValueByKeyProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly ILocalizationValueRepository _localizationValueRepository;
        public GetLocalizationValueByKeyProcessor(ILocalizationValueRepository localizationValueRepository, IAutoMapper autoMapper)
        {
            _localizationValueRepository = localizationValueRepository;
            _autoMapper = autoMapper;
        }

        public Task<LocalizationValueUiModel> GetLocalizationValueByKey(string key, string domain, string lang)
        {
            return Task.Run(() =>
            {
                var locale = _localizationValueRepository.GetLocaleByKey(key, domain, lang);
                return _autoMapper.Map<LocalizationValueUiModel>(locale);
            });
        }
    }
}