using System.Threading;
using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.Itineraries;
using sw.routing.common.dtos.Vms.Itineraries;
using sw.routing.contracts.ContractRepositories;
using sw.routing.contracts.V1.Itineraries;
using sw.routing.model.Itineraries;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using MediatR;

namespace sw.routing.services.V1.Itineraries;

public class DeleteSoftItineraryProcessor : IDeleteSoftItineraryProcessor, IRequestHandler<DeleteSoftItineraryCommand, BusinessResult<ItineraryDeletionUiModel>>
{
    private readonly IUnitOfWork _uOf;
    private readonly IItineraryRepository _itineraryRepository;

    public DeleteSoftItineraryProcessor(IUnitOfWork uOf, IItineraryRepository itineraryRepository)
    {
        this._uOf = uOf;
        this._itineraryRepository = itineraryRepository;
    }

    public async Task<BusinessResult<ItineraryDeletionUiModel>> Handle(DeleteSoftItineraryCommand deleteCommand,
        CancellationToken cancellationToken)
    {
        return await this.DeleteSoftItineraryAsync(deleteCommand);
    }

    public async Task<BusinessResult<ItineraryDeletionUiModel>> DeleteSoftItineraryAsync(
        DeleteSoftItineraryCommand deleteCommand)
    {
        var bc = new BusinessResult<ItineraryDeletionUiModel>(new ItineraryDeletionUiModel());

        if (deleteCommand is null)
        {
            bc.AddBrokenRule(new BusinessError(null));
        }

        var itinerary = this._itineraryRepository.FindBy(deleteCommand.Id);
        if (itinerary is null || !itinerary.Active)
        {
            bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "Itinerary Id does not exist"));
            return bc;
        }

        itinerary.DeleteWithAudit(deleteCommand.DeletedBy, deleteCommand.DeletedReason);

        this.Persist(itinerary, deleteCommand.Id);

        bc.Model.Id = deleteCommand.Id;
        bc.Model.Active = false;

        //bc.Model.Hard = false;
        bc.Model.Message = $"Itinerary with id: {deleteCommand.Id} has been deleted successfully.";

        return await Task.FromResult(bc);
    }

    private void Persist(Itinerary itinerary, long id)
    {
        this._itineraryRepository.Save(itinerary, id);
        this._uOf.Commit();
    }
} //Class : DeleteSoftItineraryProcessor