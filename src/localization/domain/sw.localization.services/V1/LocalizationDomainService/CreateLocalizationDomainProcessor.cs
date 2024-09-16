using sw.localization.common.dtos.Cqrs.LocalizationDomains;
using sw.localization.common.dtos.Vms.LocalizationDomains;
using sw.localization.contracts.ContractRepositories;
using sw.localization.contracts.V1.LocalizationDomainProcessors;
using sw.localization.model.Localizations;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;

namespace sw.localization.services.LocalizationDomainService
{
    public class CreateLocalizationDomainProcessor :
        ICreateLocalizationDomainProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly ILocalizationDomainRepository _LocalizationDomainRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateLocalizationDomainProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, ILocalizationDomainRepository LocalizationDomainRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _LocalizationDomainRepository = LocalizationDomainRepository;
        }

        public async Task<BusinessResult<LocalizationDomainCreationUiModel>> CreateLocalizationDomainAsync(CreateLocalizationDomainCommand createCommand)
        {
            var bc = new BusinessResult<LocalizationDomainCreationUiModel>(new LocalizationDomainCreationUiModel());

            if (createCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var langExists = _LocalizationDomainRepository.HasDomain(createCommand.Domain);
            if (langExists)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(createCommand.Domain), "Localization Domain already exists"));
            }

            var LocalizationDomain = new LocalizationDomain(2) // TODO: Get userId from Auth
            {
                Name = createCommand.Domain
            };

            await Task.Run(() => PersistLocalizationDomain(LocalizationDomain));

            var response = _autoMapper.Map<LocalizationDomainCreationUiModel>(LocalizationDomain);
            response.Message = "Localization domain created successfully.";

            bc.Model = response;

            return bc;
        }

        private void PersistLocalizationDomain(LocalizationDomain LocalizationDomain)
        {
            _LocalizationDomainRepository.Add(LocalizationDomain);
            _uOf.Commit();
        }
    }
}