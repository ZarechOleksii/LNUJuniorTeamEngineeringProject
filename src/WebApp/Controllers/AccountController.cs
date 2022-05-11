using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Services.Implementations;
using Services.Interfaces;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMailService _mailService;
        private readonly ICommentService _commentService;

        public AccountController(
            ILogger<AccountController> logger,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IMailService mailService,
            ICommentService commentService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
            _commentService = commentService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new ()
                {
                    Email = model.Email,
                    UserName = model.Username
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var callbackUrl = Url.Action(
                        nameof(ConfirmEmail),
                        "Account",
                        new { userId = user.Id, code },
                        protocol: HttpContext.Request.Scheme);

                    await _mailService.SendHtmlEmailAsync(
                        model.Email,
                        "Confirm your email",
                        Constants.ConfirmEmailTemplate,
                        callbackUrl);

                    return RedirectToAction(nameof(Login));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is null)
                {
                    ModelState.AddModelError(string.Empty, "Email is not registered");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }

                ModelState.AddModelError(string.Empty, "Incorrect Email or password");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View("Error", "Failed to confirm your email.");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                    {
                        _logger.LogInformation("Unable to find user with email {Email}", model.Email);
                        return View("ForgotPasswordConfirmation");
                    }

                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code }, protocol: HttpContext.Request.Scheme);
                    await _mailService.SendMailAsync(
                        model.Email,
                        "OnlyMovies password reset",
                        $"Click this link to reset your password: <a href='{callbackUrl}'>link</a>");

                    return View("ForgotPasswordConfirmation");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Got error in ForgotPassword method in PasswordResetController");
                    return View("Error", "Unable to start reset password process. Please try again or contact administrator.");
                }
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string? code = null)
        {
            if (code is null)
            {
                return View("Error");
            }

            var model = new ResetPasswordViewModel()
            {
                Token = code
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                _logger.LogInformation("User {Email} passwords missmatch", model.Email);
                return View("Error", "Unable to reset your password. Passwords missmatch.");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                _logger.LogInformation("Unable to find user with email {Email}", model.Email);
                return View("Error", $"Unable to reset password for user with email {model.Email}. User not found.");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return View("Login");
            }

            return View("Error", "Unable to reset your password. Please try again or contact administrator.");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MeAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdWithRelationsAsync(userId);

            if (user is null)
            {
                return BadRequest();
            }

            bool admin = await _userManager.IsInRoleAsync(user, "Admin");
            bool banned = await _userManager.IsInRoleAsync(user, "Banned");

            var model = new ProfileViewModel
            {
                Email = user.Email,
                UserName = user.UserName,
                IsAdmin = admin,
                Favourites = user.Favourites,
                Banned = banned
            };

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
            {
                return View("Error", "User with this id does not exist");
            }

            bool admin = await _userManager.IsInRoleAsync(user, "Admin");
            bool banned = await _userManager.IsInRoleAsync(user, "Banned");

            var model = new ProfileViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                IsAdmin = admin,
                Favourites = user.Favourites,
                Banned = banned
            };

            return View("Me", model);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BanAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
            {
                return View("Error", "User with this id does not exist");
            }

            bool admin = await _userManager.IsInRoleAsync(user, "Admin");

            if (admin)
            {
                return View("Error", "You cannot ban administrator");
            }

            bool banned = await _userManager.IsInRoleAsync(user, "Banned");

            IdentityResult result;

            if (banned)
            {
                result = await _userManager.RemoveFromRoleAsync(user, "Banned");
            }
            else
            {
                result = await _userManager.AddToRoleAsync(user, "Banned");
            }

            if (result.Succeeded)
            {
                await _commentService.DeleteUserCommentsAsync(user);
                return Ok();
            }

            return BadRequest();
        }
    }
}
