using sw.localization.common.dtos.Cqrs.LocalizationDomains;
using sw.localization.common.dtos.Vms.LocalizationDomains;
using sw.localization.contracts.ContractRepositories;
using sw.localization.contracts.V1.LocalizationDomainProcessors;
using sw.localization.model.Localizations;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;

namespace sw.localization.services.LocalizationDomainService
{
    public class HardDeleteLocalizationDomainProcessor :
        IHardDeleteLocalizationDomainProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly ILocalizationDomainRepository _localizationDomainRepository;

        public HardDeleteLocalizationDomainProcessor(IUnitOfWork uOf, ILocalizationDomainRepository localizationDomainRepository)
        {
            _uOf = uOf;
            _localizationDomainRepository = localizationDomainRepository;
        }

        public async Task<BusinessResult<LocalizationDomainDeletionUiModel>> HardDeleteLocalizationDomainAsync(HardDeleteLocalizationDomainCommand deleteCommand)
        {
            var bc = new BusinessResult<LocalizationDomainDeletionUiModel>(new LocalizationDomainDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var localizationDomain = _localizationDomainRepository.FindBy(deleteCommand.Id);
            if (localizationDomain is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "Localization Domain Id does not exist"));
            }

            await Task.Run(() => PersistLocalizationDomain(localizationDomain));

            return new LocalizationDomainDeletionUiModel
            {
                Id = deleteCommand.Id,
                Successful = true,
                Hard = true,
                Message = $"Localization with id: {deleteCommand.Id} has been hard deleted successfully."
            };
        }

        private void PersistLocalizationDomain(LocalizationDomain localizationDomain)
        {
            _localizationDomainRepository.Remove(localizationDomain);
            _uOf.Commit();
        }
    }
}