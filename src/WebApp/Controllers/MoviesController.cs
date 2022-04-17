using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Services.Interfaces;

namespace WebApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly IMovieService _service;

        public MoviesController(ILogger<MoviesController> logger, IMovieService service)
        {
            _logger = logger;
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddAsync(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return View(movie);
            }

            await _service.AddMovieAsync(movie);

            return View("~/Views/Home/Index.cshtml");
        }
    }
}
