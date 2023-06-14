using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeventCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Manager")]

    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
