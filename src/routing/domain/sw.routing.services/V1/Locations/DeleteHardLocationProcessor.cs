using System.Threading;
using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.Locations;
using sw.routing.common.dtos.Vms.Locations;
using sw.routing.contracts.ContractRepositories;
using sw.routing.contracts.V1.Locations;
using sw.routing.model.ItineraryTemplates.LocationPoints;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using MediatR;

namespace sw.routing.services.V1.Locations;

public class DeleteHardLocationProcessor : IDeleteHardLocationProcessor, IRequestHandler<DeleteHardLocationCommand,
    BusinessResult<LocationDeletionUiModel>>
{
    private readonly IUnitOfWork _uOf;
    private readonly ILocationRepository _locationRepository;

    public DeleteHardLocationProcessor(IUnitOfWork uOf, ILocationRepository locationRepository)
    {
        this._uOf = uOf;
        this._locationRepository = locationRepository;
    }

    public async Task<BusinessResult<LocationDeletionUiModel>> Handle(DeleteHardLocationCommand deleteCommand, CancellationToken cancellationToken)
    {
        return await this.DeleteHardLocationAsync(deleteCommand);
    }

    public async Task<BusinessResult<LocationDeletionUiModel>> DeleteHardLocationAsync(DeleteHardLocationCommand deleteCommand)
    {
        var bc = new BusinessResult<LocationDeletionUiModel>(new LocationDeletionUiModel());

        if (deleteCommand is null)
        {
            bc.AddBrokenRule(new BusinessError(null));
        }

        var location = this._locationRepository.FindBy(deleteCommand.Id);
        if (location is null)
        {
            bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "Location Id does not exist"));
            return bc;
        }

        this.Persist(location);

        //bc.Model.Id = deleteCommand.Id;
        //bc.Model.Successful = true;
        //bc.Model.Hard = true;
        bc.Model.Message = $"Location with id: {deleteCommand.Id} has been hard deleted successfully.";

        return await Task.FromResult(bc);
    }

    private void Persist(LocationPoint location)
    {
        this._locationRepository.Remove(location);
        this._uOf.Commit();
    }

}//Class : DeleteHardLocationProcessor