using sw.localization.common.dtos.Cqrs.LocalizationLanguages;
using sw.localization.common.dtos.Vms.LocalizationLanguages;
using sw.localization.contracts.ContractRepositories;
using sw.localization.contracts.V1.LocalizationLanguageProcessors;
using sw.localization.model.Localizations;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;

namespace sw.localization.services.LocalizationLanguageService
{
    public class HardDeleteLocalizationLanguageProcessor :
        IHardDeleteLocalizationLanguageProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly ILocalizationLanguageRepository _localizationLanguageRepository;

        public HardDeleteLocalizationLanguageProcessor(IUnitOfWork uOf, ILocalizationLanguageRepository localizationLanguageRepository)
        {
            _uOf = uOf;
            _localizationLanguageRepository = localizationLanguageRepository;
        }

        public async Task<BusinessResult<LocalizationLanguageDeletionUiModel>> HardDeleteLocalizationLanguageAsync(HardDeleteLocalizationLanguageCommand deleteCommand)
        {
            var bc = new BusinessResult<LocalizationLanguageDeletionUiModel>(new LocalizationLanguageDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var localizationLanguage = _localizationLanguageRepository.FindBy(deleteCommand.Id);
            if (localizationLanguage is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "Localization language Id does not exist"));
            }

            await Task.Run(() => PersistLocalizationLanguage(localizationLanguage));

            return new LocalizationLanguageDeletionUiModel
            {
                Id = deleteCommand.Id,
                Successful = true,
                Hard = true,
                Message = $"Localization with id: {deleteCommand.Id} has been hard deleted successfully."
            };
        }

        private void PersistLocalizationLanguage(LocalizationLanguage localizationLanguage)
        {
            _localizationLanguageRepository.Remove(localizationLanguage);
            _uOf.Commit();
        }
    }
}