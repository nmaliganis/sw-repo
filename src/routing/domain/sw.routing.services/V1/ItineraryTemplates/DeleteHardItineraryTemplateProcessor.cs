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

public class DeleteHardItineraryTemplateProcessor : IDeleteHardItineraryTemplateProcessor, IRequestHandler<DeleteHardItineraryTemplateCommand,
    BusinessResult<ItineraryTemplateDeletionUiModel>>
{
    private readonly IUnitOfWork _uOf;
    private readonly IItineraryTemplateRepository _itineraryTemplateRepository;

    public DeleteHardItineraryTemplateProcessor(IUnitOfWork uOf, IItineraryTemplateRepository itineraryTemplateRepository)
    {
        this._uOf = uOf;
        this._itineraryTemplateRepository = itineraryTemplateRepository;
    }

    public async Task<BusinessResult<ItineraryTemplateDeletionUiModel>> Handle(DeleteHardItineraryTemplateCommand deleteCommand, CancellationToken cancellationToken)
    {
        return await this.DeleteHardItineraryTemplateAsync(deleteCommand);
    }

    public async Task<BusinessResult<ItineraryTemplateDeletionUiModel>> DeleteHardItineraryTemplateAsync(DeleteHardItineraryTemplateCommand deleteCommand)
    {
        var bc = new BusinessResult<ItineraryTemplateDeletionUiModel>(new ItineraryTemplateDeletionUiModel());

        if (deleteCommand is null)
        {
            bc.AddBrokenRule(new BusinessError(null));
        }

        var itineraryTemplate = this._itineraryTemplateRepository.FindBy(deleteCommand.Id);
        if (itineraryTemplate is null)
        {
            bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "ItineraryTemplate Id does not exist"));
            return bc;
        }

        this.Persist(itineraryTemplate);

        //bc.Model.Id = deleteCommand.Id;
        //bc.Model.Successful = true;
        //bc.Model.Hard = true;
        bc.Model.Message = $"ItineraryTemplate with id: {deleteCommand.Id} has been hard deleted successfully.";

        return await Task.FromResult(bc);
    }

    private void Persist(ItineraryTemplate itineraryTemplate)
    {
        this._itineraryTemplateRepository.Remove(itineraryTemplate);
        this._uOf.Commit();
    }

}//Class : DeleteHardItineraryTemplateProcessor