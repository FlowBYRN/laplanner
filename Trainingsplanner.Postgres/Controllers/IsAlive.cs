using Microsoft.AspNetCore.Mvc;

namespace Trainingsplanner.Postgres.Controllers
{
    public class IsAlive : Controller
    {
        public IActionResult Index()
        {
            return Ok("IsAlive: true");
        }
    }
}
