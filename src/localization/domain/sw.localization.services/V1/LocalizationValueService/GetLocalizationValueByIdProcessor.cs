using sw.localization.common.dtos.Vms.LocalizationValues;
using sw.localization.contracts.ContractRepositories;
using sw.localization.contracts.V1.LocalizationValueProcessors;
using sw.infrastructure.TypeMappings;
using System.Threading.Tasks;

namespace sw.localization.services.LocalizationValueService
{
    public class GetLocalizationValueByIdProcessor :
        IGetLocalizationValueByIdProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly ILocalizationValueRepository _localizationValueRepository;
        public GetLocalizationValueByIdProcessor(ILocalizationValueRepository localizationValueRepository, IAutoMapper autoMapper)
        {
            _localizationValueRepository = localizationValueRepository;
            _autoMapper = autoMapper;
        }

        public Task<LocalizationValueUiModel> GetLocalizationValueById(long id)
        {
            return Task.Run(() =>
            {
                var locale = _localizationValueRepository.FindBy(id);
                return _autoMapper.Map<LocalizationValueUiModel>(locale);
            });
        }
    }
}