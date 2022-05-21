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
        private readonly IMovieService _movieService;
        private readonly UserManager<User> _userManager;
        private readonly IFavouriteService _favouriteService;
        private readonly ICommentService _commentService;
        private readonly IRatingService _ratingService;

        public MoviesController(
            IMovieService movieService,
            UserManager<User> userManager,
            IFavouriteService favouriteService,
            ICommentService commentService,
            IRatingService ratingService)
        {
            _movieService = movieService;
            _userManager = userManager;
            _favouriteService = favouriteService;
            _commentService = commentService;
            _ratingService = ratingService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAsync(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return View(movie);
            }

            await _movieService.AddMovieAsync(movie);
            return RedirectToAction("Get", new { id = movie.Id });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditAsync(Guid id)
        {
            var movie = await _movieService.GetMovieAsync(id);

            if (movie is null)
            {
                return View("Error", "Sorry, this movie was not found.");
            }

            return View(movie);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditAsync(Movie model)
        {
            if (ModelState.IsValid)
            {
                var movie = await _movieService.GetMovieAsync(model.Id);

                if (movie is null)
                {
                    return View("Error", "Sorry, this movie was not found.");
                }

                var result = await _movieService.EditMovieAsync(movie, model);

                if (result)
                {
                    return RedirectToAction("Get", new { id = movie.Id });
                }

                return View("Error", "Sorry, unexpected error occurred.");
            }

            return View(model);
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

        [HttpGet]

        [Authorize]
        public async Task<IActionResult> CheckIfFavourite(Guid id_movie)
        {
            var movie = await _movieService.GetMovieAsync(id_movie);

            if (movie is null)
            {
                return BadRequest();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return BadRequest();
            }

            var result = await _favouriteService.IsAlreadyFavouriteAsync(user.Id, movie.Id);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
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
        public async Task<IActionResult> AddComment(Guid movieId, string content)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var movie = await _movieService.GetMovieAsync(movieId);

            if (user is null || movie is null)
            {
                return View("Error", "Failed to add comment.");
            }

            if (await _userManager.IsInRoleAsync(user, "Banned"))
            {
                return View("Error", "You are banned and not allowed to publish comments.");
            }

            var comment = new Comment()
            {
                UserId = userId,
                Content = content,
                MovieId = movieId,
                Movie = movie
            };

            var result = await _commentService.AddCommentAsync(comment);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);

            if (user is null || comment is null)
            {
                return View("Error", "Failed to delete comment.");
            }

            if (comment.UserId == userId || userRoles.Contains("Admin"))
            {
                var result = await _commentService.DeleteCommentAsync(comment);

                if (result)
                {
                    return Ok();
                }
            }
            else
            {
                return View("Error", "Failed to delete comment.");
            }

            return BadRequest();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddRate(Guid movieId, int rate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return View("Error", "Failed to add rate for movie.");
            }

            var movieRate = new MovieRate
            {
                UserId = userId,
                Rate = Convert.ToByte(rate),
                MovieId = movieId
            };

            var result = await _ratingService.AddRateAsync(movieRate);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<JsonResult> GetRate(Guid movieId)
        {
            var rate = await _ratingService.GetRateAsync(movieId);
            return Json(new { rateValue = rate });
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMovieAdmin(Guid movieId)
        {
            var movie = await _movieService.GetMovieAsync(movieId);

            if (movie is null)
            {
                return View("Error", "Failed to remove movie.");
            }

            var result = await _movieService.DeleteMovieAsync(movie);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
