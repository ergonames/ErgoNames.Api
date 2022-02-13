using Microsoft.AspNetCore.Mvc;

namespace ErgoNames.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
