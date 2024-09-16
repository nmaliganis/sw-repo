using System;
using System.Threading;
using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.ItineraryTemplates;
using sw.routing.common.dtos.Vms.ItineraryTemplates;
using sw.routing.contracts.ContractRepositories;
using sw.routing.contracts.V1.ItineraryTemplates;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using MediatR;
using Serilog;

namespace sw.routing.services.V1.ItineraryTemplates;

public class GetItineraryTemplateByIdProcessor : IGetItineraryTemplateByIdProcessor, IRequestHandler<GetItineraryTemplateByIdQuery, BusinessResult<ItineraryTemplateUiModel>>
{
    private readonly IAutoMapper _autoMapper;
    private readonly IItineraryTemplateRepository _itineraryTemplateRepository;
    public GetItineraryTemplateByIdProcessor(IItineraryTemplateRepository itineraryTemplateRepository, IAutoMapper autoMapper)
    {
        _itineraryTemplateRepository = itineraryTemplateRepository;
        _autoMapper = autoMapper;
    }

    public async Task<BusinessResult<ItineraryTemplateUiModel>> GetItineraryTemplateByIdAsync(long id)
    {
        var bc = new BusinessResult<ItineraryTemplateUiModel>(new ItineraryTemplateUiModel());

        var itineraryTemplate = this._itineraryTemplateRepository.FindBy(id);
        if (itineraryTemplate.IsNull())
        {
            Log.Information(
              $"--Method:GetItineraryTemplateByIdAsync -- Message:ItineraryTemplate_FETCH" +
              $" -- Datetime:{DateTime.Now} -- Just After : _ItineraryTemplateRepository.FindBy");
            bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "ItineraryTemplate Id does not exist"));
            return bc;
        }

        var response = this._autoMapper.Map<ItineraryTemplateUiModel>(itineraryTemplate);
        response.Message = $"ItineraryTemplate id: {response.Id} fetched successfully";

        bc.Model = response;

        return await Task.FromResult(bc);
    }

    public async Task<BusinessResult<ItineraryTemplateUiModel>> Handle(GetItineraryTemplateByIdQuery qry, CancellationToken cancellationToken)
    {
        return await GetItineraryTemplateByIdAsync(qry.Id);
    }
}