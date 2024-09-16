using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.asset.model.Assets.Containers;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;

namespace sw.asset.services.V1.AssetServices.ContainerService
{
    public class DeleteHardContainerProcessor :
        IDeleteHardContainerProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IContainerRepository _containerRepository;

        public DeleteHardContainerProcessor(IUnitOfWork uOf, IContainerRepository containerRepository)
        {
            _uOf = uOf;
            _containerRepository = containerRepository;
        }

        public async Task<BusinessResult<ContainerDeletionUiModel>> DeleteHardContainerAsync(DeleteHardContainerCommand deleteCommand)
        {
            var bc = new BusinessResult<ContainerDeletionUiModel>(new ContainerDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var container = _containerRepository.FindBy(deleteCommand.Id);
            if (container is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "Container Id does not exist"));
                return bc;
            }

            Persist(container);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = true;
            bc.Model.Message = $"Container with id: {deleteCommand.Id} has been hard deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(Container container)
        {
            _containerRepository.Remove(container);
            _uOf.Commit();
        }
    }
}
