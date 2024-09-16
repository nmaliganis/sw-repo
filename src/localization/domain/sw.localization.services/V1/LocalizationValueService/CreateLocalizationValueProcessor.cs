using sw.localization.common.dtos.Cqrs.LocalizationValues;
using sw.localization.common.dtos.Vms.LocalizationValues;
using sw.localization.common.Exceptions;
using sw.localization.contracts.ContractRepositories;
using sw.localization.contracts.V1.LocalizationValueProcessors;
using sw.localization.model.Localizations;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using OneOf;
using System;
using System.Threading.Tasks;

namespace sw.localization.services.LocalizationValueService
{
    public class CreateLocalizationValueProcessor :
        ICreateLocalizationValueProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly ILocalizationValueRepository _localizationValueRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateLocalizationValueProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, ILocalizationValueRepository localizationValueRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _localizationValueRepository = localizationValueRepository;
        }

        public async Task<OneOf<LocalizationValueCreationUiModel, Exception>> CreateLocalizationValueAsync(CreateLocalizationValueCommand createCommand)
        {
            if (createCommand is null)
            {
                return new ArgumentNullException(nameof(createCommand), "Invalid localization creation model.");
            }

            long? domainId = _localizationValueRepository.FindDomainId(createCommand.Domain);
            if (!domainId.HasValue)
            {
                return new LocalizationDomainDoesNotExistException(createCommand.Domain);
            }

            long? languageId = _localizationValueRepository.FindLanguageId(createCommand.Lang);
            if (!languageId.HasValue)
            {
                return new LocalizationLanguageDoesNotExistException(createCommand.Lang);
            }

            (bool keyExists, _) = _localizationValueRepository.HasKey(domainId.Value, languageId.Value, createCommand.Key);
            if (keyExists)
            {
                return new LocalizationKeyAlreadyExistsException(createCommand.Domain,
                    createCommand.Lang, createCommand.Key);
            }

            var localizationValue = new LocalizationValue(2) // TODO: Get userId from Auth
            {
                DomainId = domainId.Value,
                LanguageId = languageId.Value,
                Key = createCommand.Key,
                Value = createCommand.Value,
            };

            await Task.Run(() => PersistLocalizationValue(localizationValue));

            var response = _autoMapper.Map<LocalizationValueCreationUiModel>(localizationValue);
            response.Message = "Localization value created successfully.";
            return response;
        }

        private void PersistLocalizationValue(LocalizationValue localizationvalue)
        {
            _localizationValueRepository.Add(localizationvalue);
            _uOf.Commit();
        }
    }
}