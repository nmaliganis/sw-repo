using sw.asset.common.dtos.Cqrs.Geofences;
using sw.asset.common.dtos.Vms.Geofence;
using sw.asset.contracts.ContractRepositories.IGeofenceRepository;
using sw.asset.contracts.V1.GeofenceProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using MediatR;
using Serilog;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace sw.asset.services.V1.GeofenceService;

public class CreateGeofenceProcessor :
  ICreateGeofenceProcessor,
  IRequestHandler<CreateGeofenceCommand, BusinessResult<GeofenceUiModel>>
{
    private readonly IGeofenceRedisRepository _geofenceRepository;

    public CreateGeofenceProcessor(IGeofenceRedisRepository geofenceRepository)
    {
        this._geofenceRepository = geofenceRepository;

    }
    public async Task<BusinessResult<GeofenceUiModel>> Handle(CreateGeofenceCommand createCommand, CancellationToken cancellationToken)
    {
        return await this.CreateGeofenceAsync(createCommand);
    }

    public async Task<BusinessResult<GeofenceUiModel>> CreateGeofenceAsync(CreateGeofenceCommand createCommand)
    {
        var bc = new BusinessResult<GeofenceUiModel>(new GeofenceUiModel());

        if (createCommand.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_CREATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }

        try
        {
            var geofencePoints = new List<GeoEntry>();

            foreach (var geoEntryUiModel in createCommand.Parameters.GeoPoints)
            {
                geofencePoints.Add(new GeoEntry(geoEntryUiModel.Lon, geoEntryUiModel.Lat, geoEntryUiModel.Address));
            }

            var geoPointCreation = await _geofenceRepository.AddGeofencePointAsync(createCommand.Parameters.PointId, geofencePoints);

            Log.Debug(
              $"Create Geofence: {createCommand.Parameters.PointId}" +
              "--CreateGeofence--  @NotComplete@ [CreateGeofenceProcessor]. ");

            Log.Debug(
              $"Create Geofence: {createCommand.Parameters.PointId}" +
              "--CreateGeofence--  @NotComplete@ [CreateGeofenceProcessor]. ");
            bc.Model.Message = $"SUCCESS_GEOFENCE_CREATION_WITH_{geoPointCreation}_POINTS";
            bc.Model.PointId = createCommand.Parameters.PointId;
        }
        catch (Exception exxx)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Geofence: {createCommand.Parameters.PointId}" +
              $"Error Message:{errorMessage}" +
              $"--CreateGeofence--  @fail@ [CreateGeofenceProcessor]. " +
              $"@innerfault:{exxx.Message} and {exxx.InnerException}");
        }
        return await Task.FromResult(bc);
    }
    public async Task<BusinessResult<GeoEntryUiModel>> CreateGeoEntryAsync(CreateGeoEntryCommand createCommand)
    {
        var bc = new BusinessResult<GeoEntryUiModel>(new GeoEntryUiModel());

        if (createCommand.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_CREATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }
        try
        {
            var geoPointCreation = await _geofenceRepository.AddGeoPointAsync(createCommand.Parameters.PointId, new GeoEntry(
              createCommand.Parameters.GeoPoint.Lon,
              createCommand.Parameters.GeoPoint.Lat, createCommand.PhysAddress));

            Log.Debug(
              $"Create Geofence: {createCommand.Parameters.PointId}" +
              "--CreateGeofence--  @NotComplete@ [CreateGeofenceProcessor]. ");

            Log.Debug(
              $"Create Geofence: {createCommand.Parameters.PointId}" +
              "--CreateGeofence--  @NotComplete@ [CreateGeofenceProcessor]. ");
            bc.Model.Message = "SUCCESS_GEOFENCE_POINT_CREATION";
            bc.Model.Address = createCommand.PhysAddress;
            bc.Model.Lat = createCommand.Parameters.GeoPoint.Lat;
            bc.Model.Lon = createCommand.Parameters.GeoPoint.Lon;

        }
        catch (Exception exxx)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Geofence: {createCommand.Parameters.PointId}" +
              $"Error Message:{errorMessage}" +
              $"--CreateGeofence--  @fail@ [CreateGeofenceProcessor]. " +
              $"@innerfault:{exxx.Message} and {exxx.InnerException}");
        }
        return await Task.FromResult(bc);
    }

    public async Task<BusinessResult<MunicipalityUiModel>> CreateMunicipalityAsync(CreateMunicipalityCommand createCommand)
    {
        var bc = new BusinessResult<MunicipalityUiModel>(new MunicipalityUiModel());

        if (createCommand.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_CREATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }
        try
        {
            var municipalityForCreation = await _geofenceRepository.AddMunicipalityAsync("municipalities", createCommand.MunicipalityId, createCommand.MunicipalityName);

            Log.Debug(
              $"Create Municipality: {createCommand.MunicipalityName}" +
              "--CreateMunicipality--  @NotComplete@ [CreateMunicipalityProcessor]. ");

            Log.Debug(
              $"Create Municipality: {createCommand.MunicipalityName}" +
              "--CreateMunicipality--  @NotComplete@ [CreateMunicipalityProcessor]. ");
            bc.Model.Message = "SUCCESS_MUNICIPALITY_POINT_CREATION";

        }
        catch (Exception exxx)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Municipality: {createCommand.MunicipalityName}" +
              $"Error Message:{errorMessage}" +
              $"--CreateMunicipality--  @fail@ [CreateMunicipalityProcessor]. " +
              $"@innerfault:{exxx.Message} and {exxx.InnerException}");
        }
        return await Task.FromResult(bc);
    }
}