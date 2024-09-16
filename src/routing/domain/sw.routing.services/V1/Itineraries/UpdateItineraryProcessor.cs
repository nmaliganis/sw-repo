using System;
using System.Threading;
using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.Itineraries;
using sw.routing.common.dtos.Vms.Itineraries;
using sw.routing.contracts.ContractRepositories;
using sw.routing.contracts.V1.Itineraries;
using sw.routing.model.Itineraries;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using Serilog;

namespace sw.routing.services.V1.Itineraries;

public class UpdateItineraryProcessor : IUpdateItineraryProcessor, IRequestHandler<UpdateItineraryCommand, BusinessResult<ItineraryModificationUiModel>>
{
    private readonly IUnitOfWork _uOf;
    private readonly IItineraryRepository _itineraryRepository;
    private readonly IAutoMapper _autoMapper;
    public UpdateItineraryProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
        IItineraryRepository itineraryRepository)
    {
        _uOf = uOf;
        _autoMapper = autoMapper;
        _itineraryRepository = itineraryRepository;
    }

    public async Task<BusinessResult<ItineraryModificationUiModel>> Handle(UpdateItineraryCommand updateCommand, CancellationToken cancellationToken)
    {
        return await UpdateItineraryAsync(updateCommand);
    }

    public async Task<BusinessResult<ItineraryModificationUiModel>> UpdateItineraryAsync(UpdateItineraryCommand updateCommand)
    {
        var bc = new BusinessResult<ItineraryModificationUiModel>(new ItineraryModificationUiModel());

        if (updateCommand.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_UPDATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }

        try
        {
            var itinerary = _itineraryRepository.FindBy(updateCommand.Id);
            if (itinerary is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "Itinerary Id does not exist"));
                return bc;
            }

            //itinerary.ModifyWithAudit(updateCommand.ModifiedById, updateCommand);

            Persist(itinerary, updateCommand.Id);

            var response = _autoMapper.Map<ItineraryModificationUiModel>(itinerary);

            response.Message = $"Itinerary id: {response.Id} updated successfully";

            bc.Model = response;
        }
        catch (Exception e)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
                $"Update Itinerary: {updateCommand.Name}" +
                $"Error Message:{errorMessage}" +
                $"--UpdateItinerary--  @fail@ [UpdateItineraryProcessor]. " +
                $"@innerfault:{e.Message} and {e.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    private void Persist(Itinerary itinerary, long id)
    {
        this._itineraryRepository.Save(itinerary, id);
        this._uOf.Commit();
    }
}