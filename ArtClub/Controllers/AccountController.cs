using ArtClub.Models.ViewModels;
using ArtClub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ArtClub.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        public IActionResult Login() => View();

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var success = await _userService.AuthenticateAsync(model.Email, model.Password);
            return success ? RedirectToAction("Index", "Home") : View(model);
        }

        [AllowAnonymous]
        public IActionResult Register() => View();

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            // Aici s-ar apela serviciul de creare user
            return RedirectToAction("Login");
        }

        [Authorize]
        public async Task<IActionResult> Logout() => RedirectToAction("Login");
    }
}