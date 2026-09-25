using System.Security.Claims;
using RODRIGUEZ_MESSAGEBOXE.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RODRIGUEZ_MESSAGEBOXE.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Dashboard");
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return IsAjax() ? ValidationFailedJson() : View(model);

            if (UserStore.FindByUsername(model.Username) != null)
            {
                ModelState.AddModelError(nameof(model.Username), "This username is already taken.");
                return IsAjax() ? ValidationFailedJson() : View(model);
            }

            if (UserStore.FindByEmail(model.Email) != null)
            {
                ModelState.AddModelError(nameof(model.Email), "An account with this email already exists.");
                return IsAjax() ? ValidationFailedJson() : View(model);
            }

            var user = new Models.User
            {
                Name         = model.Name,
                Email        = model.Email,
                Gender       = model.Gender,
                Age          = model.Age,
                Address      = model.Address,
                Username     = model.Username,
                Password     = model.Password,
                CreatedAtUtc = DateTime.UtcNow
            };

            UserStore.Add(user);
            _logger.LogInformation("Registered: {Username}", user.Username);

            if (IsAjax())
                return Json(new { success = true, username = model.Username, message = $"Account \"{model.Username}\" created! You can now log in." });

            TempData["SuccessMessage"] = "Account created. Please sign in.";
            return View(new RegisterViewModel());
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Dashboard");
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(model);

            var user = UserStore.FindByUsername(model.Username);

            if (user == null || user.Password != model.Password)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name,      user.Username),
                new(ClaimTypes.GivenName, user.Name),
                new(ClaimTypes.Email,     user.Email)
            };

            var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc   = DateTimeOffset.UtcNow.AddDays(model.RememberMe ? 14 : 1)
                });

            _logger.LogInformation("Login: {Username}", user.Username);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        private bool IsAjax() =>
            Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        private IActionResult ValidationFailedJson()
        {
            var errors = ModelState
                .Where(kvp => kvp.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            return Json(new { success = false, errors });
        }
    }
}
