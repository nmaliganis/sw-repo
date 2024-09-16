using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.Locations;
using sw.routing.common.dtos.Vms.Locations;
using sw.routing.common.infrastructure.Exceptions.Locations;
using sw.routing.contracts.ContractRepositories;
using sw.routing.contracts.V1.Locations;
using sw.routing.model.ItineraryTemplates.LocationPoints;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using Serilog;

namespace sw.routing.services.V1.Locations;
public class CreateLocationProcessor : ICreateLocationProcessor, IRequestHandler<CreateLocationCommand, BusinessResult<LocationUiModel>>
{
    private readonly IUnitOfWork _uOf;
    private readonly ILocationRepository _locationRepository;
    private readonly IAutoMapper _autoMapper;

    public CreateLocationProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, ILocationRepository locationRepository)
    {
        this._uOf = uOf;
        this._autoMapper = autoMapper;
        this._locationRepository = locationRepository;
    }

    public async Task<BusinessResult<LocationUiModel>> Handle(CreateLocationCommand createCommand, CancellationToken cancellationToken)
    {
        return await this.CreateLocationAsync(createCommand);
    }

    public async Task<BusinessResult<LocationUiModel>> CreateLocationAsync(CreateLocationCommand createCommand)
    {
        var bc = new BusinessResult<LocationUiModel>(new LocationUiModel());

        if (createCommand is null)
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_CREATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }

        try
        {
            var locationToBeCreated = new LocationPoint();

            locationToBeCreated.InjectWithInitialAttributes(createCommand);
            locationToBeCreated.InjectWithAudit(createCommand.CreatedById);

            this.ThrowExcIfLocationCannotBeCreated(locationToBeCreated);
            this.ThrowExcIfThisLocationAlreadyExist(locationToBeCreated);

            Log.Debug(
              $"Create Location: {createCommand.Name}" +
              "--CreateLocation--  @NotComplete@ [CreateLocationProcessor]. " +
              "Message: Just Before MakeItPersistence");

            MakeLocationPersistent(locationToBeCreated);

            Log.Debug(
              $"Create Location: {createCommand.Name}" +
              "--CreateLocation--  @NotComplete@ [CreateLocationProcessor]. " +
              "Message: Just After MakeItPersistence");
            bc.Model = ThrowExcIfLocationWasNotBeMadePersistent(locationToBeCreated);
        }
        catch (InvalidLocationException e)
        {
            string errorMessage = "ERROR_INVALID_POINT_MODEL";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Location: {createCommand.Name}" +
              $"Error Message:{errorMessage}" +
              "--CreateLocation--  @NotComplete@ [CreateLocationProcessor]. " +
              $"Broken rules: {e.BrokenRules}");
        }
        catch (LocationAlreadyExistsException ex)
        {
            string errorMessage = "ERROR_POINT_ALREADY_EXISTS";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Location: {createCommand.Name}" +
              $"Error Message:{errorMessage}" +
              "--CreateLocation--  @fail@ [CreateLocationProcessor]. " +
              $"@innerfault:{ex?.Message} and {ex?.InnerException}");
        }
        catch (LocationDoesNotExistAfterMadePersistentException exx)
        {
            string errorMessage = "ERROR_POINT_NOT_MADE_PERSISTENT";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Location: {createCommand.Name}" +
              $"Error Message:{errorMessage}" +
              "--CreateLocation--  @fail@ [CreateLocationProcessor]." +
              $" @innerfault:{exx?.Message} and {exx?.InnerException}");
        }
        catch (Exception exxx)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Location: {createCommand.Name}" +
              $"Error Message:{errorMessage}" +
              $"--CreateLocation--  @fail@ [CreateLocationProcessor]. " +
              $"@innerfault:{exxx.Message} and {exxx.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    private void ThrowExcIfThisLocationAlreadyExist(LocationPoint locationToBeCreated)
    {
        var locationRetrieved = this._locationRepository.FindOneLocationByName(locationToBeCreated.Name);
        if (locationRetrieved != null)
        {
            throw new LocationAlreadyExistsException(locationToBeCreated.Name,
              locationToBeCreated.GetBrokenRulesAsString());
        }
    }

    private LocationUiModel ThrowExcIfLocationWasNotBeMadePersistent(LocationPoint locationToBeCreated)
    {
        var retrievedLocation = this._locationRepository.FindOneLocationByName(locationToBeCreated.Name);
        if (retrievedLocation != null)
        {
            return this._autoMapper.Map<LocationUiModel>(retrievedLocation);
        }

        throw new LocationDoesNotExistAfterMadePersistentException(locationToBeCreated.Name);
    }

    private void ThrowExcIfLocationCannotBeCreated(LocationPoint locationToBeCreated)
    {
        bool canBeCreated = !locationToBeCreated.GetBrokenRules().Any();
        if (!canBeCreated)
        {
            throw new InvalidLocationException(locationToBeCreated.GetBrokenRulesAsString());
        }
    }
    private void MakeLocationPersistent(LocationPoint location)
    {
        this._locationRepository.Save(location);
        this._uOf.Commit();
    }

}//Class : CreateLocationProcessor