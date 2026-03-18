using Microsoft.AspNetCore.Mvc;

namespace EventEase.Controllers
{
    public class BookingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
