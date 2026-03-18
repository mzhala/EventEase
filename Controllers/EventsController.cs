using Microsoft.AspNetCore.Mvc;

namespace EventEase.Controllers
{
    public class EventsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
