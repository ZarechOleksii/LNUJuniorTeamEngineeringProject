using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Services.Interfaces;

namespace WebApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly IMovieService _movieService;
        private readonly UserManager<User> _userManager;
        private readonly IFavouriteService _favouriteService;
        private readonly ICommentService _commentService;

        public MoviesController(
            ILogger<MoviesController> logger,
            IMovieService movieService,
            UserManager<User> userManager,
            IFavouriteService favouriteService,
            ICommentService commentService)
        {
            _logger = logger;
            _movieService = movieService;
            _userManager = userManager;
            _favouriteService = favouriteService;
            _commentService = commentService;
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

            await _movieService.AddMovieAsync(movie);
            return RedirectToAction(nameof(GetAsync), new { id = movie.Id });
        }

        [HttpGet]
        public async Task<ActionResult<Movie>> GetAsync(Guid id)
        {
            var movie = await _movieService.GetMovieAsync(id);

            if (movie is null)
            {
                return View("Error", "Sorry, this movie was not found.");
            }

            return View(movie);
        }

        [HttpPost]

        [Authorize]
        public async Task<IActionResult> AddToFavourite(Guid id_movie)
        {
            var movie = await _movieService.GetMovieAsync(id_movie);

            if (movie is null)
            {
                return View("Error", "Failed to add to favourites.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return View("Error", "Failed to add to favourites.");
            }

            var result = await _favouriteService.AddToFavouriteAsync(user.Id, movie.Id);

            if (result)
            {
                return Ok();
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteFromFavourite(Guid id_movie)
        {
            var movie = await _movieService.GetMovieAsync(id_movie);

            if (movie is null)
            {
                return View("Error", "Failed to remove from favourites.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return View("Error", "Failed to remove from favourites.");
            }

            var result = await _favouriteService.DeleteFromFavouriteAsync(user.Id, movie.Id);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return View("Error", "Failed to add comment.");
            }

            comment.UserId = userId;

            var result = await _commentService.AddCommentAsync(comment);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
