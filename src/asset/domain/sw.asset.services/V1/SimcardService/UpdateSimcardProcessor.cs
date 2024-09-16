using sw.asset.common.dtos.Cqrs.Simcards;
using sw.asset.common.dtos.Vms.Simcards;
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
using System.Threading;
using System.Threading.Tasks;

namespace sw.asset.services.V1.SimcardService;

public class UpdateSimcardProcessor :
  IUpdateSimcardProcessor,
  IRequestHandler<UpdateSimcardCommand, BusinessResult<SimcardModificationUiModel>>
{
    private readonly IUnitOfWork _uOf;
    private readonly ISimcardRepository _simcardRepository;
    private readonly IAutoMapper _autoMapper;
    public UpdateSimcardProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
      ISimcardRepository simcardRepository)
    {
        _uOf = uOf;
        _autoMapper = autoMapper;
        _simcardRepository = simcardRepository;
    }

    public async Task<BusinessResult<SimcardModificationUiModel>> Handle(UpdateSimcardCommand updateCommand, CancellationToken cancellationToken)
    {
        return await UpdateSimcardAsync(updateCommand);
    }

    public async Task<BusinessResult<SimcardModificationUiModel>> UpdateSimcardAsync(UpdateSimcardCommand updateCommand)
    {
        var bc = new BusinessResult<SimcardModificationUiModel>(new SimcardModificationUiModel());

        if (updateCommand.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_UPDATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }

        try
        {
            var simcard = _simcardRepository.FindBy(updateCommand.Id);
            if (simcard is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "Simcard Id does not exist"));
                return bc;
            }

            simcard.ModifyWithAudit(updateCommand.ModifiedById);
            simcard.InjectWithInitialAttributes(updateCommand.Parameters.Number);

            Persist(simcard, updateCommand.Id);

            var response = _autoMapper.Map<SimcardModificationUiModel>(simcard);
            response.Message = $"Simcard id: {response.Id} updated successfully";

            bc.Model = response;
        }
        catch (Exception e)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Update Simcard: {updateCommand.Parameters.Number}" +
              $"Error Message:{errorMessage}" +
              $"--UpdateSimcard--  @fail@ [UpdateSimcardProcessor]. " +
              $"@innerfault:{e.Message} and {e.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    private void Persist(Simcard simcard, long id)
    {
        _simcardRepository.Save(simcard, id);
        _uOf.Commit();
    }

}//Class : UpdateSimcardProcessor