using System.Threading;
using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.ItineraryTemplates;
using sw.routing.common.dtos.Vms.ItineraryTemplates;
using sw.routing.contracts.ContractRepositories;
using sw.routing.contracts.V1.ItineraryTemplates;
using sw.routing.model.ItineraryTemplates;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using MediatR;

namespace sw.routing.services.V1.ItineraryTemplates;

public class DeleteSoftItineraryTemplateProcessor 
    : IDeleteSoftItineraryTemplateProcessor, IRequestHandler<DeleteSoftItineraryTemplateCommand, BusinessResult<ItineraryTemplateDeletionUiModel>>
{
    private readonly IUnitOfWork _uOf;
    private readonly IItineraryTemplateRepository _itineraryTemplateRepository;

    public DeleteSoftItineraryTemplateProcessor(IUnitOfWork uOf, IItineraryTemplateRepository itineraryTemplateRepository)
    {
        this._uOf = uOf;
        this._itineraryTemplateRepository = itineraryTemplateRepository;
    }

    public async Task<BusinessResult<ItineraryTemplateDeletionUiModel>> Handle(DeleteSoftItineraryTemplateCommand deleteCommand,
        CancellationToken cancellationToken)
    {
        return await this.DeleteSoftItineraryTemplateAsync(deleteCommand);
    }

    public async Task<BusinessResult<ItineraryTemplateDeletionUiModel>> DeleteSoftItineraryTemplateAsync(
        DeleteSoftItineraryTemplateCommand deleteCommand)
    {
        var bc = new BusinessResult<ItineraryTemplateDeletionUiModel>(new ItineraryTemplateDeletionUiModel());

        if (deleteCommand is null)
        {
            bc.AddBrokenRule(new BusinessError(null));
        }

        var itineraryTemplate = this._itineraryTemplateRepository.FindBy(deleteCommand.Id);
        if (itineraryTemplate is null || !itineraryTemplate.Active)
        {
            bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "ItineraryTemplate Id does not exist"));
            return bc;
        }

        itineraryTemplate.DeleteWithAudit(deleteCommand.DeletedBy, deleteCommand.DeletedReason);

        this.Persist(itineraryTemplate, deleteCommand.Id);

        bc.Model.Id = deleteCommand.Id;
        bc.Model.Active = false;

        //bc.Model.Hard = false;
        bc.Model.Message = $"ItineraryTemplate with id: {deleteCommand.Id} has been deleted successfully.";

        return await Task.FromResult(bc);
    }

    private void Persist(ItineraryTemplate itineraryTemplate, long id)
    {
        this._itineraryTemplateRepository.Save(itineraryTemplate, id);
        this._uOf.Commit();
    }
} //Class : DeleteSoftItineraryTemplateProcessor