using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.Itineraries;
using sw.routing.common.dtos.Vms.Itineraries;
using sw.routing.common.infrastructure.Exceptions.Drivers;
using sw.routing.common.infrastructure.Exceptions.Itineraries;
using sw.routing.common.infrastructure.Exceptions.Templates;
using sw.routing.common.infrastructure.Exceptions.Vehicles;
using sw.routing.contracts.ContractRepositories;
using sw.routing.contracts.V1.Itineraries;
using sw.routing.model.CustomTypes;
using sw.routing.model.Itineraries;
using sw.routing.model.Jobs;
using sw.routing.model.TransportCombinations;
using sw.routing.model.Vehicles;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using Microsoft.VisualBasic;
using NHibernate.Cfg;
using Serilog;

namespace sw.routing.services.V1.Itineraries;
public class CreateItineraryProcessor :
  ICreateItineraryProcessor,
  IRequestHandler<CreateItineraryCommand, BusinessResult<ItineraryUiModel>>
{
    private readonly IUnitOfWork _uOf;
    private readonly IItineraryRepository _itineraryRepository;
    private readonly IDriverRepository _driverRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IItineraryTemplateRepository _itineraryTemplateRepository;
    private readonly IAutoMapper _autoMapper;

    public CreateItineraryProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, 
        IItineraryRepository itineraryRepository, 
        IItineraryTemplateRepository itineraryTemplateRepository, IDriverRepository driverRepository, IVehicleRepository vehicleRepository)
    {
        this._uOf = uOf;
        this._autoMapper = autoMapper;
        this._itineraryRepository = itineraryRepository;
        this._itineraryTemplateRepository = itineraryTemplateRepository;
        this._driverRepository = driverRepository;
        this._vehicleRepository = vehicleRepository;
    }

    public async Task<BusinessResult<ItineraryUiModel>> Handle(CreateItineraryCommand createCommand, CancellationToken cancellationToken)
    {
        return await this.CreateItineraryAsync(createCommand);
    }

    public async Task<BusinessResult<ItineraryUiModel>> CreateItineraryAsync(CreateItineraryCommand createCommand)
    {
        var bc = new BusinessResult<ItineraryUiModel>(new ItineraryUiModel());

        if (createCommand.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_CREATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }

        try
        {
            var itineraryToBeCreated = new Itinerary();

            var templateToBeInjected = _itineraryTemplateRepository.FindBy(createCommand.Parameters.ItineraryTemplateId);
            if (templateToBeInjected.IsNull())
                throw new ItineraryTemplateDoesNotExistException(createCommand.Parameters.ItineraryTemplateId);

            itineraryToBeCreated.InjectWithItineraryTemplate(templateToBeInjected);

            var transportCombination = new TransportCombination();

            foreach (var vehicle in createCommand.Parameters.Vehicles)
            {
                var vehicleToBeInjected = _vehicleRepository.FindBy(vehicle.Id);
                if (vehicleToBeInjected.IsNull())
                    throw new VehicleDoesNotExistException(vehicle.Id);

                var vehicleTransportCombination = new VehicleTransportCombination()
                {
                    Type = (TransportCombinationType)vehicle.Type,
                };

                vehicleTransportCombination.InjectWithVehicle(vehicleToBeInjected);
                transportCombination.InjectWithVehicle(vehicleTransportCombination);
            }

            var driverTransportCombination = new DriverTransportCombination();
            driverTransportCombination.InjectWithTransportCombination(transportCombination);

            var driverToBeInjected = _driverRepository.FindBy(createCommand.Parameters.DriverId);
            if (driverToBeInjected.IsNull())
                throw new DriverDoesNotExistException(createCommand.Parameters.DriverId);

            driverTransportCombination.InjectWithDriver(driverToBeInjected);
            itineraryToBeCreated.InjectWithDriverTransportCombination(driverTransportCombination);

            itineraryToBeCreated.InjectWithInitialAttributes(createCommand);
            itineraryToBeCreated.InjectWithConfig(createCommand.ObjectToJson<CreateItineraryCommand>());
            itineraryToBeCreated.InjectWithAudit(createCommand.CreatedById);

            foreach (var container in createCommand.Parameters.Containers)
            {
                var jobToBeInjected = new Job()
                {
                    Container = container.Id,
                    ScheduledArrival = container.ScheduledArrival,
                    Seq = string.Join(",", container.Seq),
                    Config = new ConfigJsonbType()
                    {
                        Params = container.AsJson()
                    }
                };
                itineraryToBeCreated.InjectWithJob(jobToBeInjected);
            }

            this.ThrowExcIfItineraryCannotBeCreated(itineraryToBeCreated);
            this.ThrowExcIfThisItineraryAlreadyExist(itineraryToBeCreated);

            Log.Debug(
              $"Create Itinerary: {createCommand.Parameters.Name}" +
              "--CreateItinerary--  @NotComplete@ [CreateItineraryProcessor]. " +
              "Message: Just Before MakeItPersistence");

            MakeItineraryPersistent(itineraryToBeCreated);

            Log.Debug(
              $"Create Itinerary: {createCommand.Parameters.Name}" +
              "--CreateItinerary--  @NotComplete@ [CreateItineraryProcessor]. " +
              "Message: Just After MakeItPersistence");
            bc.Model = ThrowExcIfItineraryWasNotBeMadePersistent(itineraryToBeCreated);
        }
        catch (ItineraryTemplateDoesNotExistException e)
        {
            string errorMessage = "ERROR_INVALID_ITINERARY_TEMPLATE_MODEL";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
                $"Create Itinerary: {createCommand.Parameters.Name}" +
                $"Error Message:{errorMessage}" +
                "--CreateItinerary--  @NotComplete@ [CreateItineraryProcessor]. " +
                $"@innerfault:{e?.Message} and {e?.InnerException}");
        }
        catch (InvalidItineraryException e)
        {
            string errorMessage = "ERROR_INVALID_ITINERARY_MODEL";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Itinerary: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              "--CreateItinerary--  @NotComplete@ [CreateItineraryProcessor]. " +
              $"Broken rules: {e.BrokenRules}");
        }
        catch (ItineraryAlreadyExistsException ex)
        {
            string errorMessage = "ERROR_ITINERARY_ALREADY_EXISTS";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Itinerary: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              "--CreateItinerary--  @fail@ [CreateItineraryProcessor]. " +
              $"@innerfault:{ex?.Message} and {ex?.InnerException}");
        }
        catch (ItineraryDoesNotExistAfterMadePersistentException exx)
        {
            string errorMessage = "ERROR_ITINERARY_NOT_MADE_PERSISTENT";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Itinerary: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              "--CreateItinerary--  @fail@ [CreateItineraryProcessor]." +
              $" @innerfault:{exx?.Message} and {exx?.InnerException}");
        }
        catch (Exception exxx)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Itinerary: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              $"--CreateItinerary--  @fail@ [CreateItineraryProcessor]. " +
              $"@innerfault:{exxx.Message} and {exxx.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    private void ThrowExcIfThisItineraryAlreadyExist(Itinerary itineraryToBeCreated)
    {
        var itineraryRetrieved = this._itineraryRepository.FindItineraryByName(itineraryToBeCreated.Name);
        if (itineraryRetrieved != null)
        {
            throw new ItineraryAlreadyExistsException(itineraryToBeCreated.Name,
              itineraryToBeCreated.GetBrokenRulesAsString());
        }
    }

    private ItineraryUiModel ThrowExcIfItineraryWasNotBeMadePersistent(Itinerary itineraryToBeCreated)
    {
        var retrievedItinerary = this._itineraryRepository.FindItineraryByName(itineraryToBeCreated.Name);
        if (retrievedItinerary != null)
        {
            return this._autoMapper.Map<ItineraryUiModel>(retrievedItinerary);
        }

        throw new ItineraryDoesNotExistAfterMadePersistentException(itineraryToBeCreated.Name);
    }

    private void ThrowExcIfItineraryCannotBeCreated(Itinerary itineraryToBeCreated)
    {
        bool canBeCreated = !itineraryToBeCreated.GetBrokenRules().Any();
        if (!canBeCreated)
        {
            throw new InvalidItineraryException(itineraryToBeCreated.GetBrokenRulesAsString());
        }
    }
    private void MakeItineraryPersistent(Itinerary itinerary)
    {
        this._itineraryRepository.Save(itinerary);
        this._uOf.Commit();
    }
}//Class : CreateItineraryProcessor