﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
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

        public AccountController(
            ILogger<AccountController> logger,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IMailService mailService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
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
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                    {
                        _logger.LogInformation($"Unable to find user with email {model.Email}");
                        return View();  // should return success view
                    }

                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // for Sergiy
                    // url is created like "{base url}/{second argument}/{first argument}?userId={user id}&code={code}"
                    var callbackUrl = Url.Action("ResetPassword", "ResetPassword", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    await _mailService.SendMailAsync(
                        model.Email,
                        "OnlyMovies password reset",
                        $"Click this link to reset your password: <a href='{callbackUrl}'>link</a>");

                    return View();  // should return success view
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Got error in ForgotPassword method in PasswordResetController");
                    return View("Error", "Unable to start reset password process. Please try again or contact administrator.");
                }
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                _logger.LogInformation($"User {model.UserEmail} passwords missmatch");
                return View("Error", "Unable to reset your password. Passwords missmatch.");
            }

            var user = await _userManager.FindByEmailAsync(model.UserEmail);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                _logger.LogInformation($"Unable to find user with email {model.UserEmail}");
                return View("Error", $"Unable to reset password for user with email {model.UserEmail}. User not found.");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return View();    // should return success view
            }

            return View("Error", "Unable to reset your password. Please try again or contact administrator.");
        }
    }
}
