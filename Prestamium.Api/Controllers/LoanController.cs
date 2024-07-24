using Microsoft.AspNetCore.Mvc;

namespace Prestamium.Api.Controllers
{
    public class LoanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
