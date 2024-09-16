using Microsoft.AspNetCore.Mvc;

namespace sw.landmark.api.Controllers
{
  /// <summary>
  /// HomeController
  /// </summary>
  public class HomeController : Controller
  {

    // GET: /<controller>/
    /// <summary>
    /// Index
    /// </summary>
    /// <returns></returns>
    public IActionResult Index()
    {
      return new RedirectResult("~/swagger");
    }
  }
}