using ArtClub.Models.Entities;
using ArtClub.Models.Enums;
using ArtClub.Models.ViewModels;
using ArtClub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ArtClub.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        public IActionResult Login()
        {
            return View("Login", new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Login", model);

            var success = await _userService.AuthenticateAsync(model.Email, model.Password);

            if (!success)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View("Login", model);
            }

            var user = await _userService.GetUserByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "User account could not be found.");
                return View("Login", model);
            }

            if (!user.IsActive)
            {
                ModelState.AddModelError("", "This account is inactive.");
                return View("Login", model);
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.UserName ?? "");
            HttpContext.Session.SetString("UserRole", user.Role.ToString());

            TempData["StatusMessage"] = "Login completed successfully.";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View("Create", new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Create", model);

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Password and confirmation password do not match.");
                return View("Create", model);
            }

            var member = new Member
            {
                UserName = $"{model.FirstName} {model.LastName}".Trim(),
                Email = model.Email,
                Role = UserRole.Member,
                IsActive = true,
                MembershipDate = DateTime.Now
            };

            var created = await _userService.RegisterUserAsync(member, model.Password);

            if (!created)
            {
                ModelState.AddModelError("", "Email already exists.");
                return View("Create", model);
            }

            TempData["StatusMessage"] = "Registration completed successfully. You can now log in.";
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            string userName,
            string email,
            UserRole role,
            bool isActive)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound();

            user.UserName = userName;
            user.Email = email;
            user.Role = role;
            user.IsActive = isActive;

            var success = await _userService.UpdateUserAsync(user);

            if (!success)
                return NotFound();

            TempData["StatusMessage"] = "User updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _userService.DeleteUserAsync(id);

            if (!success)
                return NotFound();

            TempData["StatusMessage"] = "User deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            TempData["StatusMessage"] = "You have been signed out.";
            return RedirectToAction("Index", "Home");
        }
    }
}