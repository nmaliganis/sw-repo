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

public class DeleteSoftLocationProcessor 
    : IDeleteSoftLocationProcessor, IRequestHandler<DeleteSoftLocationCommand, BusinessResult<LocationDeletionUiModel>>
{
    private readonly IUnitOfWork _uOf;
    private readonly ILocationRepository _locationRepository;

    public DeleteSoftLocationProcessor(IUnitOfWork uOf, ILocationRepository locationRepository)
    {
        this._uOf = uOf;
        this._locationRepository = locationRepository;
    }

    public async Task<BusinessResult<LocationDeletionUiModel>> Handle(DeleteSoftLocationCommand deleteCommand,
        CancellationToken cancellationToken)
    {
        return await this.DeleteSoftLocationAsync(deleteCommand);
    }

    public async Task<BusinessResult<LocationDeletionUiModel>> DeleteSoftLocationAsync(
        DeleteSoftLocationCommand deleteCommand)
    {
        var bc = new BusinessResult<LocationDeletionUiModel>(new LocationDeletionUiModel());

        if (deleteCommand is null)
        {
            bc.AddBrokenRule(new BusinessError(null));
        }

        var location = this._locationRepository.FindBy(deleteCommand.Id);
        if (location is null || !location.Active)
        {
            bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "Location Id does not exist"));
            return bc;
        }

        location.DeleteWithAudit(deleteCommand.DeletedBy, deleteCommand.DeletedReason);

        this.Persist(location, deleteCommand.Id);

        bc.Model.Id = deleteCommand.Id;
        bc.Model.Active = false;

        //bc.Model.Hard = false;
        bc.Model.Message = $"Location with id: {deleteCommand.Id} has been deleted successfully.";

        return await Task.FromResult(bc);
    }

    private void Persist(LocationPoint location, long id)
    {
        this._locationRepository.Save(location, id);
        this._uOf.Commit();
    }
} //Class : DeleteSoftLocationProcessor