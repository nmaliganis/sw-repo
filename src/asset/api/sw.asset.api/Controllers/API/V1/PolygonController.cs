using sw.asset.api.Helpers.Models;
using sw.asset.api.Validators;
using sw.infrastructure.Controlles.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace sw.asset.api.Controllers.API.V1;

/// <summary>
/// Polygon Controller
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class PolygonController : BaseController
{
    /// <summary>
    /// ctor : PolygonController
    /// </summary>
    public PolygonController()
    {
    }

    [HttpGet(Name = "GetPolygonRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetPolygonAsync()
    {
        var zone = new Zone()
        {
            Name = null,
            Positions = new List<List<double>>()
        };

        //try
        //{
        //    var limits = await GetLimits();
        //    SharpKml.Base.Parser kmlParser = new SharpKml.Base.Parser();
        //    kmlParser.ParseString(limits, false);
        //    if (kmlParser.Root is SharpKml.Dom.Placemark placemark)
        //    {
        //        var name = placemark.Name;
        //        zone.Name = name;
        //        if (placemark.Geometry is Polygon)
        //        {
        //            var poly = placemark.Geometry;
        //            foreach (var o in poly.Flatten().OfType<OuterBoundary>())
        //            {
        //                foreach (var lin in o.Flatten().OfType<LinearRing>())
        //                {
        //                    foreach (var col in lin.Flatten().OfType<CoordinateCollection>())
        //                    {
        //                        var coorList = new List<List<double>>();
        //                        var coordinates = col.GetEnumerator;
        //                        foreach (var coord in col)
        //                        {
        //                            coorList.Add(new List<double>
        //        {
        //          coord.Latitude, coord.Longitude
        //        });
        //                        }
        //                        zone.Positions = coorList;
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

        return Ok(zone);
    }


    private async Task<string> GetLimits()
    {
        XNamespace ns = "http://www.opengis.net/kml/2.2";
        var xdoc = XDocument.Load("./wwwroot/Docs/thermi.kml");
        
        // the base of the returned xdoc will be the kml 
        var placemarks = xdoc.Element(ns + "kml")
          .Element(ns + "Document")
          .Element(ns + "Folder")
          .Element(ns + "Placemark"); //for now the root passed is Placemark
        return placemarks.ToString();
    }
}