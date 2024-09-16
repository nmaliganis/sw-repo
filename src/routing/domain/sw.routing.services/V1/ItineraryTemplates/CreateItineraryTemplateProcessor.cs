using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.ItineraryTemplates;
using sw.routing.common.dtos.Vms.ItineraryTemplates;
using sw.routing.common.infrastructure.Exceptions.Locations;
using sw.routing.common.infrastructure.Exceptions.Templates;
using sw.routing.contracts.ContractRepositories;
using sw.routing.contracts.V1.ItineraryTemplates;
using sw.routing.model.ItineraryTemplates;
using sw.routing.model.ItineraryTemplates.LocationPoints;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using Serilog;

namespace sw.routing.services.V1.ItineraryTemplates;
public class CreateItineraryTemplateProcessor : ICreateItineraryTemplateProcessor, IRequestHandler<CreateItineraryTemplateCommand, BusinessResult<ItineraryTemplateUiModel>>
{
    private readonly IUnitOfWork _uOf;
    private readonly ILocationRepository _locationRepository;
    private readonly IItineraryTemplateRepository _itineraryTemplateRepository;
    private readonly IAutoMapper _autoMapper;

    public CreateItineraryTemplateProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IItineraryTemplateRepository itineraryTemplateRepository, ILocationRepository locationRepository)
    {
        this._uOf = uOf;
        this._autoMapper = autoMapper;
        this._locationRepository = locationRepository;
        this._itineraryTemplateRepository = itineraryTemplateRepository;
    }

    public async Task<BusinessResult<ItineraryTemplateUiModel>> Handle(CreateItineraryTemplateCommand createCommand, CancellationToken cancellationToken)
    {
        return await this.CreateItineraryTemplateAsync(createCommand);
    }

    public async Task<BusinessResult<ItineraryTemplateUiModel>> CreateItineraryTemplateAsync(CreateItineraryTemplateCommand createCommand)
    {
        var bc = new BusinessResult<ItineraryTemplateUiModel>(new ItineraryTemplateUiModel());

        if (createCommand is null)
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_CREATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }

        try
        {
            var itineraryTemplateToBeCreated = new ItineraryTemplate();

            itineraryTemplateToBeCreated.InjectWithInitialAttributes(createCommand);
            itineraryTemplateToBeCreated.InjectWithAudit(createCommand.CreatedById);

            var endToLocation = _locationRepository.FindBy(createCommand.Parameters.EndTo);
            if (endToLocation.IsNull())
                throw new LocationDoesNotExistException(createCommand.Parameters.EndTo);


            ItineraryTemplateLocationPoint endToPoint = new ItineraryTemplateLocationPoint()
            {
                IsStart = false
            };

            endToPoint.InjectWithLocation(endToLocation);
            endToPoint.InjectWithAudit(createCommand.CreatedById);

            itineraryTemplateToBeCreated.InjectWithEndTo(endToPoint);

            var startFromLocation = _locationRepository.FindBy(createCommand.Parameters.StartFrom);
            if (startFromLocation.IsNull())
                throw new LocationDoesNotExistException(createCommand.Parameters.StartFrom);

            ItineraryTemplateLocationPoint startFromPoint = new ItineraryTemplateLocationPoint()
            {
                IsStart = true
            };

            startFromPoint.InjectWithLocation(startFromLocation);
            startFromPoint.InjectWithAudit(createCommand.CreatedById);

            itineraryTemplateToBeCreated.InjectWithStartFrom(startFromPoint);

            this.ThrowExcIfItineraryTemplateCannotBeCreated(itineraryTemplateToBeCreated);
            this.ThrowExcIfThisItineraryTemplateAlreadyExist(itineraryTemplateToBeCreated);

            Log.Information(
              $"Create ItineraryTemplate: {createCommand.Parameters.Name}" +
              "--CreateItineraryTemplate--  @NotComplete@ [CreateItineraryTemplateProcessor]. " +
              "Message: Just Before MakeItPersistence");

            MakeItineraryTemplatePersistent(itineraryTemplateToBeCreated);

            Log.Information(
              $"Create ItineraryTemplate: {createCommand.Parameters.Name}" +
              "--CreateItineraryTemplate--  @NotComplete@ [CreateItineraryTemplateProcessor]. " +
              "Message: Just After MakeItPersistence");
            bc.Model = ThrowExcIfItineraryTemplateWasNotBeMadePersistent(itineraryTemplateToBeCreated);
        }
        catch (InvalidItineraryTemplateException e)
        {
            string errorMessage = "ERROR_INVALID_ITINERARY_TEMPLATE_MODEL";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create ItineraryTemplate: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              "--CreateItineraryTemplate--  @NotComplete@ [CreateItineraryTemplateProcessor]. " +
              $"Broken rules: {e.BrokenRules}");
        }
        catch (ItineraryTemplateAlreadyExistsException ex)
        {
            string errorMessage = "ERROR_ITINERARY_TEMPLATE_ALREADY_EXISTS";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create ItineraryTemplate: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              "--CreateItineraryTemplate--  @fail@ [CreateItineraryTemplateProcessor]. " +
              $"@innerfault:{ex?.Message} and {ex?.InnerException}");
        }
        catch (ItineraryTemplateDoesNotExistAfterMadePersistentException exx)
        {
            string errorMessage = "ERROR_ITINERARY_TEMPLATE_NOT_MADE_PERSISTENT";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create ItineraryTemplate: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              "--CreateItineraryTemplate--  @fail@ [CreateItineraryTemplateProcessor]." +
              $" @innerfault:{exx?.Message} and {exx?.InnerException}");
        }
        catch (Exception exxx)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create ItineraryTemplate: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              $"--CreateItineraryTemplate--  @fail@ [CreateItineraryTemplateProcessor]. " +
              $"@innerfault:{exxx.Message} and {exxx.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    private void ThrowExcIfThisItineraryTemplateAlreadyExist(ItineraryTemplate itineraryTemplateToBeCreated)
    {
        var itineraryTemplateRetrieved = this._itineraryTemplateRepository.FindItineraryTemplateByName(itineraryTemplateToBeCreated.Name);
        if (itineraryTemplateRetrieved != null)
        {
            throw new ItineraryTemplateAlreadyExistsException(itineraryTemplateToBeCreated.Name,
              itineraryTemplateToBeCreated.GetBrokenRulesAsString());
        }
    }

    private ItineraryTemplateUiModel ThrowExcIfItineraryTemplateWasNotBeMadePersistent(ItineraryTemplate itineraryTemplateToBeCreated)
    {
        var retrievedItineraryTemplate = this._itineraryTemplateRepository.FindBy(itineraryTemplateToBeCreated.Id);
        if (retrievedItineraryTemplate != null)
        {
            return this._autoMapper.Map<ItineraryTemplateUiModel>(retrievedItineraryTemplate);
        }

        throw new ItineraryTemplateDoesNotExistAfterMadePersistentException(itineraryTemplateToBeCreated.Name);
    }

    private void ThrowExcIfItineraryTemplateCannotBeCreated(ItineraryTemplate itineraryTemplateToBeCreated)
    {
        bool canBeCreated = !itineraryTemplateToBeCreated.GetBrokenRules().Any();
        if (!canBeCreated)
        {
            throw new InvalidItineraryTemplateException(itineraryTemplateToBeCreated.GetBrokenRulesAsString());
        }
    }
    private void MakeItineraryTemplatePersistent(ItineraryTemplate itineraryTemplate)
    {
        this._itineraryTemplateRepository.Save(itineraryTemplate);
        this._uOf.Commit();
    }

}//Class : CreateItineraryTemplateProcessor