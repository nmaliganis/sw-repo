using sw.localization.common.dtos.Cqrs.LocalizationValues;
using sw.localization.common.dtos.Vms.LocalizationValues;
using sw.localization.common.Exceptions;
using sw.localization.contracts.ContractRepositories;
using sw.localization.contracts.V1.LocalizationValueProcessors;
using sw.localization.model.Localizations;
using sw.infrastructure.UnitOfWorks;
using OneOf;
using System;
using System.Threading.Tasks;

namespace sw.localization.services.LocalizationValueService
{
    public class DeleteLocalizationvalueProcessor :
        IDeleteLocalizationvalueProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly ILocalizationValueRepository _localizationValueRepository;

        public DeleteLocalizationvalueProcessor(IUnitOfWork uOf, ILocalizationValueRepository localizationValueRepository)
        {
            _uOf = uOf;
            _localizationValueRepository = localizationValueRepository;
        }

        public async Task<OneOf<LocalizationValueDeletionUiModel, Exception>> DeleteLocalizationValueAsync(DeleteLocalizationValueCommand deleteCommand)
        {
            if (deleteCommand is null)
            {
                return new ArgumentNullException(nameof(deleteCommand), "Invalid localization delete model.");
            }

            var localizationValue = _localizationValueRepository.FindBy(deleteCommand.Id);
            if (localizationValue is null)
            {
                return new LocalizationIdDoesNotExistException(deleteCommand.Id);
            }

            localizationValue.Deleted(deleteCommand.DeletedBy, deleteCommand.DeletedReason);

            await Task.Run(() => PersistLocalizationValue(localizationValue, deleteCommand.Id));

            return new LocalizationValueDeletionUiModel
            {
                Id = deleteCommand.Id,
                Successful = true,
                Hard = false,
                Message = $"Localization with id: {deleteCommand.Id} has been deleted successfully."
            };
        }

        private void PersistLocalizationValue(LocalizationValue localizationvalue, long id)
        {
            _localizationValueRepository.Save(localizationvalue, id);
            _uOf.Commit();
        }
    }
}