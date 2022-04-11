using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WebApp.Controllers
{
    [Route("Health/[action]")]
    public class HealthController : Controller
    {
        private readonly ILogger<HealthController> _logger;
        private readonly IBaseEntityService _service;

        public HealthController(
            ILogger<HealthController> logger,
            IBaseEntityService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching Information.");
            var entities = await _service.GetBaseEntitiesAsync();
            _logger.LogInformation("Fetched. Returning view.");
            return View(entities);
        }
    }
}
