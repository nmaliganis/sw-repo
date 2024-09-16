using sw.asset.common.dtos.Cqrs.Simcards;
using sw.asset.common.dtos.Vms.Simcards;
using sw.asset.common.infrastructure.Exceptions.Simcards;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.SimcardProcessors;
using sw.asset.model.Devices.Simcards;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace sw.asset.services.V1.SimcardService;
public class CreateSimcardProcessor :
  ICreateSimcardProcessor,
  IRequestHandler<CreateSimcardCommand, BusinessResult<SimcardCreationUiModel>>
{
    private readonly IUnitOfWork _uOf;
    private readonly ISimcardRepository _simcardRepository;
    private readonly IAutoMapper _autoMapper;

    public CreateSimcardProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, ISimcardRepository simcardRepository)
    {
        this._uOf = uOf;
        this._autoMapper = autoMapper;
        this._simcardRepository = simcardRepository;
        this._simcardRepository = simcardRepository;
    }

    public async Task<BusinessResult<SimcardCreationUiModel>> Handle(CreateSimcardCommand createCommand, CancellationToken cancellationToken)
    {
        return await this.CreateSimcardAsync(createCommand);
    }

    public async Task<BusinessResult<SimcardCreationUiModel>> CreateSimcardAsync(CreateSimcardCommand createCommand)
    {
        var bc = new BusinessResult<SimcardCreationUiModel>(new SimcardCreationUiModel());

        if (createCommand.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_CREATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }

        try
        {
            var simcardToBeCreated = new Simcard();

            simcardToBeCreated.InjectWithInitialAttributes(createCommand.Parameters.Number);
            simcardToBeCreated.InjectWithAudit(createCommand.CreatedById);

            this.ThrowExcIfSimcardCannotBeCreated(simcardToBeCreated);
            this.ThrowExcIfThisSimcardAlreadyExist(simcardToBeCreated);

            Log.Information(
              $"Create Simcard: {createCommand.Parameters.Number}" +
              "--CreateSimcard--  @NotComplete@ [CreateSimcardProcessor]. " +
              "Message: Just Before MakeItPersistence");

            MakeSimcardPersistent(simcardToBeCreated);

            Log.Information(
              $"Create Simcard: {createCommand.Parameters.Number}" +
              "--CreateSimcard--  @NotComplete@ [CreateSimcardProcessor]. " +
              "Message: Just After MakeItPersistence");
            bc.Model = ThrowExcIfSimcardWasNotBeMadePersistent(simcardToBeCreated);
            bc.Model.Message = "SUCCESS_SIMCARD_CREATION";
        }
        catch (InvalidSimcardException e)
        {
            string errorMessage = "ERROR_INVALID_SIMCARD_MODEL";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Simcard: {createCommand.Parameters.Number}" +
              $"Error Message:{errorMessage}" +
              "--CreateSimcard--  @NotComplete@ [CreateSimcardProcessor]. " +
              $"Broken rules: {e.BrokenRules}");
        }
        catch (SimcardAlreadyExistsException ex)
        {
            string errorMessage = "ERROR_SIMCARD_ALREADY_EXISTS";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Simcard: {createCommand.Parameters.Number}" +
              $"Error Message:{errorMessage}" +
              "--CreateSimcard--  @fail@ [CreateSimcardProcessor]. " +
              $"@innerfault:{ex?.Message} and {ex?.InnerException}");
        }
        catch (SimcardDoesNotExistAfterMadePersistentException exx)
        {
            string errorMessage = "ERROR_SIMCARD_NOT_MADE_PERSISTENT";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Simcard: {createCommand.Parameters.Number}" +
              $"Error Message:{errorMessage}" +
              "--CreateSimcard--  @fail@ [CreateSimcardProcessor]." +
              $" @innerfault:{exx?.Message} and {exx?.InnerException}");
        }
        catch (Exception exxx)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Simcard: {createCommand.Parameters.Number}" +
              $"Error Message:{errorMessage}" +
              $"--CreateSimcard--  @fail@ [CreateSimcardProcessor]. " +
              $"@innerfault:{exxx.Message} and {exxx.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    private void ThrowExcIfThisSimcardAlreadyExist(Simcard simcardToBeCreated)
    {
        var simcardRetrieved = this._simcardRepository.FindOneByNumber(simcardToBeCreated.Number);
        if (simcardRetrieved != null)
        {
            throw new SimcardAlreadyExistsException(simcardToBeCreated.Number,
              simcardToBeCreated.GetBrokenRulesAsString());
        }
    }

    private SimcardCreationUiModel ThrowExcIfSimcardWasNotBeMadePersistent(Simcard simcardToBeCreated)
    {
        var retrievedSimcard = this._simcardRepository.FindOneByNumber(simcardToBeCreated.Number);
        if (retrievedSimcard != null)
        {
            return this._autoMapper.Map<SimcardCreationUiModel>(retrievedSimcard);
        }

        throw new SimcardDoesNotExistAfterMadePersistentException(simcardToBeCreated.Number);
    }

    private void ThrowExcIfSimcardCannotBeCreated(Simcard simcardToBeCreated)
    {
        bool canBeCreated = !simcardToBeCreated.GetBrokenRules().Any();
        if (!canBeCreated)
        {
            throw new InvalidSimcardException(simcardToBeCreated.GetBrokenRulesAsString());
        }
    }

    private void MakeSimcardPersistent(Simcard simcard)
    {
        this._simcardRepository.Save(simcard);
        this._uOf.Commit();
    }

}//Class : CreateSimcardProcessor