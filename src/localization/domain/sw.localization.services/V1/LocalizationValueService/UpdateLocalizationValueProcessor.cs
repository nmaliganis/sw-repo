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
    public class UpdateLocalizationValueProcessor :
        IUpdateLocalizationValueProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly ILocalizationValueRepository _localizationValueRepository;
        private readonly IAutoMapper _autoMapper;
        public UpdateLocalizationValueProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            ILocalizationValueRepository localizationValueRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _localizationValueRepository = localizationValueRepository;
        }

        public async Task<OneOf<LocalizationValueModificationUiModel, Exception>> UpdateLocalizationValueAsync(UpdateLocalizationValueCommand updateCommand)
        {
            if (updateCommand is null)
            {
                return new ArgumentNullException(nameof(updateCommand), "Invalid localization update model.");
            }

            var localizationValue = _localizationValueRepository.FindBy(updateCommand.Id);
            if (localizationValue is null)
            {
                return new LocalizationIdDoesNotExistException(updateCommand.Id);
            }

            var oldValue = localizationValue.Value;

            localizationValue.Value = updateCommand.Value;
            localizationValue.Modified(2); // TODO: Get userId from Auth

            await Task.Run(() => PersistLocalizationValue(localizationValue, updateCommand.Id));

            var response = _autoMapper.Map<LocalizationValueModificationUiModel>(localizationValue);
            response.OldValue = oldValue;
            response.Message = $"Localization key: {response.Key} updated successfully with value: {response.NewValue}";
            return response;
        }

        private void PersistLocalizationValue(LocalizationValue localizationvalue, long id)
        {
            _localizationValueRepository.Save(localizationvalue, id);
            _uOf.Commit();
        }
    }
}