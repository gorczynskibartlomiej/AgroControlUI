using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroControlUI.Controllers
{
    public class CalculatorController : Controller
    {
        [Authorize(Policy = "OwnerOrWorker")]
        public IActionResult CornDrying()
        {
            return View();
        }
        [Authorize(Policy = "OwnerOrWorker")]
        public IActionResult PlantPopulation()
        {
            return View();
        }
        [Authorize(Policy = "OwnerOrWorker")]
        public IActionResult SeedRate()
        {
            return View();
        }
        [Authorize(Policy = "OwnerOrWorker")]
        public IActionResult Spraying()
        {
            return View();
        }
    }
}
