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

        // ==========================================
        // 1. LISTARE UTILIZATORI (INDEX)
        // ==========================================
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        // ==========================================
        // 2. LOGICĂ DE LOGIN
        // ==========================================
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Verificare credențiale prin serviciu
            var success = await _userService.AuthenticateAsync(model.Email, model.Password);
            if (!success)
            {
                ModelState.AddModelError("", "Email sau parolă incorectă.");
                return View(model);
            }

            // Preluare date utilizator pentru sesiune
            var user = await _userService.GetUserByEmailAsync(model.Email);
            if (user == null || !user.IsActive)
            {
                ModelState.AddModelError("", "Contul este inactiv sau nu a fost găsit.");
                return View(model);
            }

            // Setare date în sesiune
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.UserName ?? "User");
            HttpContext.Session.SetString("UserRole", user.Role.ToString());

            TempData["StatusMessage"] = $"Bine ai revenit, {user.UserName}!";
            return RedirectToAction("Index", "Home");
        }

        // ==========================================
        // 3. LOGICĂ DE ÎNREGISTRARE (REGISTER)
        // ==========================================
        [HttpGet]
        public IActionResult Register()
        {
            // NOTĂ: Dacă fișierul tău se numește Create.cshtml, folosim "Create"
            // Dacă l-ai redenumit în Register.cshtml, poți folosi return View();
            return View("Create", new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Create", model);

            // Creare entitate Member din ViewModel
            var member = new Member
            {
                UserName = $"{model.FirstName} {model.LastName}".Trim(),
                Email = model.Email,
                Role = UserRole.Member,
                IsActive = true,
                MembershipDate = DateTime.Now,
                EventCreationLimit = 5
            };

            var created = await _userService.RegisterUserAsync(member, model.Password);
            if (!created)
            {
                ModelState.AddModelError("Email", "Această adresă de email este deja înregistrată.");
                return View("Create", model);
            }

            TempData["StatusMessage"] = "Înregistrare reușită! Te poți loga acum.";
            return RedirectToAction(nameof(Login));
        }

        // ==========================================
        // 4. LOGICĂ DE LOGOUT
        // ==========================================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["StatusMessage"] = "Te-ai deconectat cu succes.";
            return RedirectToAction("Index", "Home");
        }

        // ==========================================
        // 5. DETALII UTILIZATOR
        // ==========================================
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }
    }
}