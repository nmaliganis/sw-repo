using sw.asset.api.Validators;
using sw.asset.common.dtos.Cqrs.Geofences;
using sw.asset.common.dtos.Cqrs.Persons;
using sw.asset.common.dtos.ResourceParameters.Geofences;
using sw.asset.common.dtos.Vms.Geofence;
using sw.asset.common.infrastructure.Exceptions.Geofence;
using sw.asset.contracts.ContractRepositories.IGeofenceRepository;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Controlles.Base;
using sw.infrastructure.Extensions;
using Geocoding;
using Geocoding.Google;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace sw.asset.api.Controllers.API.V1;

/// <summary>
/// Controller : GeofenceController
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize(Roles = "SU, ADMIN")]
public class GeofenceController : BaseController
{
    public IConfiguration Configuration { get; }
    private readonly IMediator _mediator;
    private readonly IGeofenceRedisRepository _geofenceRepository;
    private readonly IHostingEnvironment _environment;

    public GeofenceController(IHostingEnvironment environment,
        IMediator mediator,
        IConfiguration configuration,
      IGeofenceRedisRepository geofenceRedisRepository)
    {
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        Configuration = configuration;
        _mediator = mediator;
        _geofenceRepository = geofenceRedisRepository;
    }

    /// <summary>
    /// Post : Method for PostMunicipalityAsync
    /// </summary>
    /// <param name="municipalityName"></param>
    /// <param name="municipalityId"></param>
    /// <remarks>Post Municipality </remarks>
    /// <response code="200">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns></returns>
    [HttpPost("municipalities/{municipalityName}/{municipalityId}", Name = "PostMunicipalityRoot")]
    public async Task<IActionResult> PostMunicipalityAsync([Required] string municipalityName,
      [Required] int municipalityId)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var municipalityCreation =
          await _geofenceRepository.AddMunicipalityAsync("municipalities", municipalityId, municipalityName);

        return Ok(municipalityCreation);

    }

    /// <summary>
    /// Get - Get the Address of a Point from Latitude and Longitude 
    /// </summary>
    /// <param name="latitude">Latitude</param>
    /// <param name="longitude">Longitude</param>
    /// <remarks>Get Point Address </remarks>
    /// <response code="200">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("address/{latitude}/{longitude}", Name = "GetPointAddressRoot")]
    public async Task<IActionResult> GetPointAddressAsync(double latitude, double longitude)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        var address = GetLocationByCoordinates(latitude, longitude).Result;
        if (address.IsNull())
        {
            return this.NotFound("ADDRESS_NOT_FOUND");
        };
        var geoEntry = new GeoEntryUiModel()
        {
            Lat = latitude,
            Lon = longitude,
            Address = address
        };
        BusinessResult<GeoEntryUiModel> br = new BusinessResult<GeoEntryUiModel>(geoEntry);
        br.Model.Message = "SUCCESSFUL_REVERSE_GEOCODING";
        return Ok(br);
    }

    /// <summary>
    /// Get - Get the Geofence Points from the Geofence Key 
    /// </summary>
    /// <param name="geofenceKey"> Geofence Key</param>
    /// <remarks>Get Geofence Points </remarks>
    /// <response code="200">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("geofence/{geofenceKey}", Name = "GetGeofencePointsRoot")]
    public async Task<IActionResult> GetGeofencePointsAsync(string geofenceKey)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        BusinessResult<LandmarkUiModel> fetchedGeofence = await this._mediator.Send(new GetGeofenceByKeyQuery(geofenceKey));

        // It is ok to return no values at all.

        return Ok(fetchedGeofence);
    }


    /// <summary>
    /// Get - Get the Geo Point from key, if it is stored as Entry
    /// </summary>
    /// <param name="pointKey"> Geofence Key</param>
    /// <remarks>Get Geo Point Entry </remarks>
    /// <response code="200">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("point/{pointKey}", Name = "GetPointRoot")]
    public async Task<IActionResult> GetPointAsync(string pointKey)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        BusinessResult<GeoEntryUiModel> fetchedGeoEntry = await this._mediator.Send(new GetGeoEntryByKeyQuery(pointKey));

        if (fetchedGeoEntry.IsNull())
        {
            return BadRequest("RETRIEVE_POINT_FAILED");
        }

        return Ok(fetchedGeoEntry);
    }


    /// <summary>
    /// Post - Create a Point in Redis
    /// </summary>
    /// <param name="geoPointForCreation">CreateGeoEntryResourceParameters for creation</param>
    /// <remarks>Create Point </remarks>
    /// <response code="200">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost("point", Name = "PostPointRoot")]
    [ValidateModel]
    public async Task<IActionResult> PostPointAsync([FromBody] CreateGeoEntryResourceParameters geoPointForCreation)
    {

        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        string physicalAddr = String.Empty;
        try
        {
            physicalAddr =
              GetLocationByCoordinates(geoPointForCreation.GeoPoint.Lat, geoPointForCreation.GeoPoint.Lon).Result;
        }
        catch (GeolocationNotFound ex)
        {
            return BadRequest("CREATION_POINT_FAILED");
        }
        catch (Exception e)
        {
            return BadRequest("CREATION_POINT_FAILED");
        }

        BusinessResult<GeoEntryUiModel> createdPoint = await this._mediator.Send(new CreateGeoEntryCommand(userAudit.Model.Id, physicalAddr, geoPointForCreation));

        if (createdPoint.IsNull())
        {
            Log.Error(
              $"--Method:PostPointRoot -- Message:POINT_CREATION" +
              $" -- Datetime:{DateTime.Now} -- Action: PostPointAsync");
            return this.NotFound("ERROR_POINT_CREATED_NOT_FOUND");
        }
        if (!createdPoint.IsSuccess())
        {
            return this.OkOrBadRequest(createdPoint.BrokenRules);
        }
        if (createdPoint.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(createdPoint.BrokenRules);
        }
        return this.CreatedOrNoResult(createdPoint);
    }

    /// <summary>
    /// Post - Create a Geofence (multiple Points) in Redis
    /// </summary>
    /// <param name="geofenceForCreation">CreateGeofenceResourceParameters for creation</param>
    /// <remarks>Create Geofence </remarks>
    /// <response code="200">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost("geofence", Name = "PostGeofencePointsRoot")]
    [ValidateModel]
    public async Task<IActionResult> PostGeofencePointsAsync([FromBody] CreateGeofenceResourceParameters geofenceForCreation)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        BusinessResult<GeofenceUiModel> createdPoint = await this._mediator.Send(new CreateGeofenceCommand(userAudit.Model.Id, geofenceForCreation));

        if (createdPoint.IsNull())
        {
            Log.Error(
              $"--Method:PostGeofencePointsRoot -- Message:GEOFENCE_CREATION" +
              $" -- Datetime:{DateTime.Now} -- Action: PostGeofencePointsAsync");
            return this.NotFound("ERROR_GEOFENCE_CREATED_NOT_FOUND");
        }
        if (!createdPoint.IsSuccess())
        {
            return this.OkOrBadRequest(createdPoint.BrokenRules);
        }
        if (createdPoint.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(createdPoint.BrokenRules);
        }
        return this.CreatedOrNoResult(createdPoint);
    }

    /// <summary>
    /// Post - Create a Geofence (multiple Points) from KML file in Redis
    /// </summary>
    /// <param name="geofenceName">CreateGeoEntryResourceParameters for creation</param>
    /// <remarks>Create Geofence From KML </remarks>
    /// <response code="200">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost("geofence/generateFromKML/{geofenceName}", Name = "PostGeofenceGenerateFromKMLRoot")]
    public async Task<IActionResult> PostGeofenceGenerateFromKMLAsync(string geofenceName)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
        {
            _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            _environment.WebRootPath = Directory.GetCurrentDirectory();
        }

        var geoFenceKMLPath = _environment.WebRootPath + "/geofence/";

        var geofencePoints = new List<GeoEntryUiModel>();

        //try
        //{
        //    var limits = await GetLimits($"{geoFenceKMLPath}{geofenceName}.kml");
        //    SharpKml.Base.Parser kmlParser = new SharpKml.Base.Parser();
        //    kmlParser.ParseString(limits, false);
        //    if (kmlParser.Root is Placemark placemark)
        //    {
        //        var name = placemark.Name;
        //        if (placemark.Geometry is Polygon poly)
        //        {
        //            foreach (var o in poly.Flatten().OfType<OuterBoundary>())
        //            {
        //                foreach (var lin in o.Flatten().OfType<LinearRing>())
        //                {
        //                    foreach (var col in lin.Flatten().OfType<CoordinateCollection>())
        //                    {
        //                        var coordinates = col.GetEnumerator;
        //                        foreach (var coord in col)
        //                        {
        //                            var lat = coord.Latitude;
        //                            var lon = coord.Longitude;
        //                            GeoEntryUiModel itemGeoEntryToBeAdded = new GeoEntryUiModel()
        //                            {
        //                                Lat = lat,
        //                                Lon = lon,
        //                                Address = GetLocationByCoordinates(lat, lon).Result
        //                            };
        //                            geofencePoints.Add(itemGeoEntryToBeAdded);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}
        //catch (Exception e)
        //{
        //    return BadRequest(e.Message);
        //}

        CreateGeofenceResourceParameters geoFenceForCreation = new CreateGeofenceResourceParameters()
        {
            PointId = geofenceName,
            GeoPoints = geofencePoints
        };

        // TODO: check if key (geofenceName) already exists.
        // If so, either return BadRequest with key name already exists, or update values with current file

        BusinessResult<GeofenceUiModel> createdPoint = await this._mediator.Send(new CreateGeofenceCommand(userAudit.Model.Id, geoFenceForCreation));

        if (createdPoint.IsNull())
        {
            Log.Error(
              $"--Method:PostGeofencePointsRoot -- Message:GEOFENCE_CREATION" +
              $" -- Datetime:{DateTime.Now} -- Action: PostGeofencePointsAsync");
            return this.NotFound("ERROR_GEOFENCE_CREATED_NOT_FOUND");
        }
        if (!createdPoint.IsSuccess())
        {
            return this.OkOrBadRequest(createdPoint.BrokenRules);
        }
        if (createdPoint.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(createdPoint.BrokenRules);
        }
        return this.CreatedOrNoResult(createdPoint);
    }

    //[HttpPut("geofence/{geofenceKey}", Name = "PutGeofencePointsRoot")]
    ////[ValidateModel]
    //public async Task<IActionResult> PutGeofencePointsAsync(string geofenceKey,
    //  [FromBody] UpdateGeofenceResourceParameters geofenceForModification)
    //{

    //    var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

    //    if (userAudit.IsNull())
    //    {
    //        return this.NotFound("USER_AUDIT_NOT_FOUND");
    //    }

    //    var geofencePoints = new List<GeoEntry>();

    //    foreach (var mapPoint in geofenceForModification.GeoFencePoints)
    //    {
    //        GeoEntry itemGeoEntryToBeAdded = new GeoEntry(mapPoint.Longitude, mapPoint.Latitude,
    //          GetLocationByCoordinates(mapPoint.Latitude, mapPoint.Longitude).Result);
    //        geofencePoints.Add(itemGeoEntryToBeAdded);
    //    }

    //    var result = await this._mediator.Send(new UpdateGeofenceCommand(geofenceKey, userAudit.Model.Id, geofenceForModification, geofencePoints));


    //    try
    //    {
    //        var geoPointCreation = await _geofenceRepository.AddGeofencePointAsync(geofenceKey, geofencePoints);
    //        var geoMapPointStored = await _geofenceRepository.AddMapPointsAsync($"{geofenceKey}-s",
    //          geofenceForModification.GeoFencePoints);

    //        if (geoMapPointStored)
    //            Ok(new { geoPointCreation, geofencePoints });

    //        BadRequest("MODIFICATION_GEOFENCE_FAILED");
    //    }
    //    catch (Exception e)
    //    {
    //        BadRequest("MODIFICATION_GEOFENCE_FAILED");
    //    }

    //    return Ok(result);
    //}

    private async Task<string> GetLocationByCoordinates(double lat, double lon)
    {
        // The location to reverse geocode.
        IGeocoder geocoder = new GoogleGeocoder(Configuration["GCPAPIKey"]);
        IEnumerable<Address> addresses = await geocoder.ReverseGeocodeAsync(lat, lon);
        //var address = "Nikitara kai Psarwn";
        return addresses.First().ToString();
    }

    private async Task<string> GetLimits(string path)
    {
        XNamespace ns = "http://www.opengis.net/kml/2.2";
        var xdoc = XDocument.Load(path);

        // the base of the returned xdoc will be the kml 
        var placemarks = xdoc.Element(ns + "kml")
            .Element(ns + "Document")
            .Element(ns + "Folder")
            .Element(ns + "Placemark"); //for now the root passed is Placemark
        return placemarks.ToString();
    }
}// Class: GeofenceController