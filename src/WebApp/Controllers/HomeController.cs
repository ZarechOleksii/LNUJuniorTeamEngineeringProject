using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _homeService;

        public HomeController(
            ILogger<HomeController> logger,
            IHomeService homeService)
        {
            _logger = logger;
            _homeService = homeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> LoadMoreMovies(int page, int pageSize)
        {
            var movies = await _homeService.GetMoviesPartAsync(page, pageSize);

            if (movies is null || movies.Count < 0)
            {
                return Json(BadRequest());
            }

            return Json(movies);
        }
    }
}
