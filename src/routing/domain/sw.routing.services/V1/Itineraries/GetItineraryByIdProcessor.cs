using System;
using System.Threading;
using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.Itineraries;
using sw.routing.common.dtos.Vms.Itineraries;
using sw.routing.contracts.ContractRepositories;
using sw.routing.contracts.V1.Itineraries;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using MediatR;
using Serilog;

namespace sw.routing.services.V1.Itineraries;

public class GetItineraryByIdProcessor : IGetItineraryByIdProcessor, IRequestHandler<GetItineraryByIdQuery, BusinessResult<ItineraryUiModel>>
{
    private readonly IAutoMapper _autoMapper;
    private readonly IItineraryRepository _itineraryRepository;
    public GetItineraryByIdProcessor(IItineraryRepository itineraryRepository, IAutoMapper autoMapper)
    {
        _itineraryRepository = itineraryRepository;
        _autoMapper = autoMapper;
    }

    public async Task<BusinessResult<ItineraryUiModel>> GetItineraryByIdAsync(long id)
    {
        var bc = new BusinessResult<ItineraryUiModel>(new ItineraryUiModel());

        var itinerary = this._itineraryRepository.FindBy(id);
        if (itinerary.IsNull())
        {
            Log.Information(
              $"--Method:GetItineraryByIdAsync -- Message:Itinerary_FETCH" +
              $" -- Datetime:{DateTime.Now} -- Just After : _ItineraryRepository.FindBy");
            bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "Itinerary Id does not exist"));
            return bc;
        }

        var response = this._autoMapper.Map<ItineraryUiModel>(itinerary);
        response.Message = $"Itinerary id: {response.Id} fetched successfully";

        bc.Model = response;

        return await Task.FromResult(bc);
    }

    public async Task<BusinessResult<ItineraryUiModel>> Handle(GetItineraryByIdQuery qry, CancellationToken cancellationToken)
    {
        return await GetItineraryByIdAsync(qry.Id);
    }
}