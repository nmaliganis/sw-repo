using System;
using System.Threading;
using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.Locations;
using sw.routing.common.dtos.Vms.Locations;
using sw.routing.contracts.ContractRepositories;
using sw.routing.contracts.V1.Locations;
using sw.routing.model.ItineraryTemplates.LocationPoints;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using Serilog;

namespace sw.routing.services.V1.Locations;

public class UpdateLocationProcessor : IUpdateLocationProcessor, IRequestHandler<UpdateLocationCommand, BusinessResult<LocationModificationUiModel>>
{
    private readonly IUnitOfWork _uOf;
    private readonly ILocationRepository _locationRepository;
    private readonly IAutoMapper _autoMapper;
    public UpdateLocationProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
        ILocationRepository locationRepository)
    {
        _uOf = uOf;
        _autoMapper = autoMapper;
        _locationRepository = locationRepository;
    }

    public async Task<BusinessResult<LocationModificationUiModel>> Handle(UpdateLocationCommand updateCommand, CancellationToken cancellationToken)
    {
        return await UpdateLocationAsync(updateCommand);
    }

    public async Task<BusinessResult<LocationModificationUiModel>> UpdateLocationAsync(UpdateLocationCommand updateCommand)
    {
        var bc = new BusinessResult<LocationModificationUiModel>(new LocationModificationUiModel());

        if (updateCommand.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_UPDATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }

        try
        {
            var Location = _locationRepository.FindBy(updateCommand.Id);
            if (Location is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "Location Id does not exist"));
                return bc;
            }

            //Location.ModifyWithAudit(updateCommand.ModifiedById, updateCommand);

            Persist(Location, updateCommand.Id);

            var response = _autoMapper.Map<LocationModificationUiModel>(Location);

            response.Message = $"Location id: {response.Id} updated successfully";

            bc.Model = response;
        }
        catch (Exception e)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
                $"Update Location: {updateCommand.Name}" +
                $"Error Message:{errorMessage}" +
                $"--UpdateLocation--  @fail@ [UpdateLocationProcessor]. " +
                $"@innerfault:{e.Message} and {e.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    private void Persist(LocationPoint location, long id)
    {
        this._locationRepository.Save(location, id);
        this._uOf.Commit();
    }
}