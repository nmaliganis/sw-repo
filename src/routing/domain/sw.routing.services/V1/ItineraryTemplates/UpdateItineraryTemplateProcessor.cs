using System;
using System.Threading;
using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.ItineraryTemplates;
using sw.routing.common.dtos.Vms.ItineraryTemplates;
using sw.routing.contracts.ContractRepositories;
using sw.routing.contracts.V1.ItineraryTemplates;
using sw.routing.model.ItineraryTemplates;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using Serilog;

namespace sw.routing.services.V1.ItineraryTemplates;

public class UpdateItineraryTemplateProcessor : IUpdateItineraryTemplateProcessor, IRequestHandler<UpdateItineraryTemplateCommand, 
    BusinessResult<ItineraryTemplateUiModel>>
{
    private readonly IUnitOfWork _uOf;
    private readonly IItineraryTemplateRepository _itineraryTemplateRepository;
    private readonly IAutoMapper _autoMapper;
    public UpdateItineraryTemplateProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
        IItineraryTemplateRepository itineraryTemplateRepository)
    {
        _uOf = uOf;
        _autoMapper = autoMapper;
        _itineraryTemplateRepository = itineraryTemplateRepository;
    }

    public async Task<BusinessResult<ItineraryTemplateUiModel>> Handle(UpdateItineraryTemplateCommand updateCommand, CancellationToken cancellationToken)
    {
        return await UpdateItineraryTemplate(updateCommand);
    }

    public async Task<BusinessResult<ItineraryTemplateUiModel>> UpdateItineraryTemplate(UpdateItineraryTemplateCommand updateCommand)
    {
        var bc = new BusinessResult<ItineraryTemplateUiModel>(new ItineraryTemplateUiModel());

        if (updateCommand.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_UPDATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }

        try
        {
            var itineraryTemplateToBeModified = _itineraryTemplateRepository.FindBy(updateCommand.Id);
            if (itineraryTemplateToBeModified.IsNull())
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "ItineraryTemplate Id does not exist"));
                return bc;
            }

            itineraryTemplateToBeModified.InjectWithModifiedAttributes(updateCommand);
            itineraryTemplateToBeModified.ModifyWithAudit(updateCommand.ModifiedById, updateCommand);

            Log.Information(
                $"Create ItineraryTemplate: {updateCommand.Parameters.Name}" +
                "--UpdateItineraryTemplate--  @NotComplete@ [UpdateItineraryTemplateProcessor]. " +
                "Message: Just Before MakeItPersistence");

            MakeItineraryTemplatePersistent(itineraryTemplateToBeModified, updateCommand.Id);

            Log.Information(
                $"Create ItineraryTemplate: {updateCommand.Parameters.Name}" +
                "--UpdateItineraryTemplate--  @NotComplete@ [UpdateItineraryTemplateProcessor]. " +
                "Message: Just After MakeItPersistence");

            var response = _autoMapper.Map<ItineraryTemplateUiModel>(itineraryTemplateToBeModified);

            response.Message = $"ItineraryTemplate id: {response.Id} updated successfully";

            bc.Model = response;
        }
        catch (Exception e)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
                $"Update ItineraryTemplate: {updateCommand.Id} - {updateCommand.Parameters.Name}" +
                $"Error Message:{errorMessage}" +
                $"--UpdateItineraryTemplate--  @fail@ [UpdateItineraryTemplateProcessor]. " +
                $"@innerfault:{e.Message} and {e.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    private void MakeItineraryTemplatePersistent(ItineraryTemplate itineraryTemplate, long id)
    {
        this._itineraryTemplateRepository.Save(itineraryTemplate, id);
        this._uOf.Commit();
    }
}