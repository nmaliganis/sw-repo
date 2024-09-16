using System;
using System.Threading;
using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.Locations;
using sw.routing.common.dtos.Vms.Locations;
using sw.routing.contracts.ContractRepositories;
using sw.routing.contracts.V1.Locations;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using MediatR;
using Serilog;

namespace sw.routing.services.V1.Locations;

public class GetLocationByIdProcessor : IGetLocationByIdProcessor, IRequestHandler<GetLocationByIdQuery, BusinessResult<LocationUiModel>>
{
    private readonly IAutoMapper _autoMapper;
    private readonly ILocationRepository _locationRepository;
    public GetLocationByIdProcessor(ILocationRepository locationRepository, IAutoMapper autoMapper)
    {
        _locationRepository = locationRepository;
        _autoMapper = autoMapper;
    }

    public async Task<BusinessResult<LocationUiModel>> GetLocationByIdAsync(long id)
    {
        var bc = new BusinessResult<LocationUiModel>(new LocationUiModel());

        var location = this._locationRepository.FindBy(id);
        if (location.IsNull())
        {
            Log.Information(
              $"--Method:GetLocationByIdAsync -- Message:Location_FETCH" +
              $" -- Datetime:{DateTime.Now} -- Just After : _LocationRepository.FindBy");
            bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "Location Id does not exist"));
            return bc;
        }

        var response = this._autoMapper.Map<LocationUiModel>(location);
        response.Message = $"Location id: {response.Id} fetched successfully";

        bc.Model = response;

        return await Task.FromResult(bc);
    }

    public async Task<BusinessResult<LocationUiModel>> Handle(GetLocationByIdQuery qry, CancellationToken cancellationToken)
    {
        return await GetLocationByIdAsync(qry.Id);
    }
}