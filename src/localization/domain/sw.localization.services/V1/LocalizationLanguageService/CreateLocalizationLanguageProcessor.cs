using sw.localization.common.dtos.Cqrs.LocalizationLanguages;
using sw.localization.common.dtos.Vms.LocalizationLanguages;
using sw.localization.contracts.ContractRepositories;
using sw.localization.contracts.V1.LocalizationLanguageProcessors;
using sw.localization.model.Localizations;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;

namespace sw.localization.services.LocalizationLanguageService
{
    public class CreateLocalizationLanguageProcessor :
        ICreateLocalizationLanguageProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly ILocalizationLanguageRepository _LocalizationLanguageRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateLocalizationLanguageProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, ILocalizationLanguageRepository LocalizationLanguageRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _LocalizationLanguageRepository = LocalizationLanguageRepository;
        }

        public async Task<BusinessResult<LocalizationLanguageCreationUiModel>> CreateLocalizationLanguageAsync(CreateLocalizationLanguageCommand createCommand)
        {
            var bc = new BusinessResult<LocalizationLanguageCreationUiModel>(new LocalizationLanguageCreationUiModel());

            if (createCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var langExists = _LocalizationLanguageRepository.HasLanguage(createCommand.Lang);
            if (langExists)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(createCommand.Lang), "Localization language already exists"));
            }

            var LocalizationLanguage = new LocalizationLanguage(2) // TODO: Get userId from Auth
            {
                Name = createCommand.Lang
            };

            await Task.Run(() => PersistLocalizationLanguage(LocalizationLanguage));

            var response = _autoMapper.Map<LocalizationLanguageCreationUiModel>(LocalizationLanguage);
            response.Message = "Localization value created successfully.";

            bc.Model = response;

            return bc;
        }

        private void PersistLocalizationLanguage(LocalizationLanguage LocalizationLanguage)
        {
            _LocalizationLanguageRepository.Add(LocalizationLanguage);
            _uOf.Commit();
        }
    }
}