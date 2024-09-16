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

public class DeleteHardItineraryProcessor : IDeleteHardItineraryProcessor, IRequestHandler<DeleteHardItineraryCommand, BusinessResult<ItineraryDeletionUiModel>>
{
    private readonly IUnitOfWork _uOf;
    private readonly IItineraryRepository _itineraryRepository;

    public DeleteHardItineraryProcessor(IUnitOfWork uOf, IItineraryRepository itineraryRepository)
    {
        this._uOf = uOf;
        this._itineraryRepository = itineraryRepository;
    }

    public async Task<BusinessResult<ItineraryDeletionUiModel>> Handle(DeleteHardItineraryCommand deleteCommand, CancellationToken cancellationToken)
    {
        return await this.DeleteHardItineraryAsync(deleteCommand);
    }

    public async Task<BusinessResult<ItineraryDeletionUiModel>> DeleteHardItineraryAsync(DeleteHardItineraryCommand deleteCommand)
    {
        var bc = new BusinessResult<ItineraryDeletionUiModel>(new ItineraryDeletionUiModel());

        if (deleteCommand is null)
        {
            bc.AddBrokenRule(new BusinessError(null));
        }

        var itinerary = this._itineraryRepository.FindBy(deleteCommand.Id);
        if (itinerary is null)
        {
            bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "Itinerary Id does not exist"));
            return bc;
        }

        this.Persist(itinerary);

        //bc.Model.Id = deleteCommand.Id;
        //bc.Model.Successful = true;
        //bc.Model.Hard = true;
        bc.Model.Message = $"Itinerary with id: {deleteCommand.Id} has been hard deleted successfully.";

        return await Task.FromResult(bc);
    }

    private void Persist(Itinerary itinerary)
    {
        this._itineraryRepository.Remove(itinerary);
        this._uOf.Commit();
    }

}//Class : DeleteHardItineraryProcessor