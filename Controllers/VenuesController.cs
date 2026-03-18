using Microsoft.AspNetCore.Mvc;

namespace EventEase.Controllers
{
    public class VenuesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
