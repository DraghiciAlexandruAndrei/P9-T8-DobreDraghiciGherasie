using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using ArtClub.Models.Entities;
using ArtClub.Models.Enums;
using ArtClub.Models.ViewModels;
using ArtClub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ArtClub.DataAccess;
using ArtClub.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ArtClub.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;

        public AccountController(IUserService userService, ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        [Authorize(Policy = "AdminOnly")]
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

            if (user.IsBanned)
            {
                ModelState.AddModelError("", "This account has been banned.");
                return View("Login", model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("Role", user.Role.ToString())
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

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
                Role = UserRole.External,   // new users start as External
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

        // Profile page for any logged-in user
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return RedirectToAction(nameof(Login));

            var user = await _userService.GetUserByIdAsync(userId.Value);
            if (user == null) return NotFound();

            return View(user);
        }
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return RedirectToAction(nameof(Login));

            var user = await _userService.GetUserByIdAsync(userId.Value);
            if (user == null) return NotFound();

            // Admins go directly to the Admin panel
            if (user.Role == UserRole.Admin)
                return RedirectToAction("Index", "Admin");

            // Build member dashboard
            var recentInvitations = await _context.Invitations
                .Include(i => i.Event).ThenInclude(e => e.Organizer)
                .Where(i => i.InviteeId == userId.Value && i.Status == InvitationStatus.Pending)
                .OrderByDescending(i => i.Event.Date)
                .Take(5)
                .Select(i => new RecentInvitationItem
                {
                    InvitationId = i.Id,
                    EventTitle = i.Event.Title,
                    OrganizerName = i.Event.Organizer.UserName
                })
                .ToListAsync();

            var upcomingEvents = await _context.Events
                .Include(e => e.Resource)
                .Where(e => e.OrganizerId == userId.Value && e.Date >= DateTime.Now)
                .OrderBy(e => e.Date)
                .Take(5)
                .Select(e => new UpcomingEventItem
                {
                    EventId = e.Id,
                    Title = e.Title,
                    ResourceName = e.Resource.Name,
                    StartDate = e.Date
                })
                .ToListAsync();

            const int monthlyLimit = 3;
            var eventsThisMonth = await _context.Events
                .CountAsync(e => e.OrganizerId == userId.Value
                              && e.Date.Month == DateTime.Now.Month
                              && e.Date.Year == DateTime.Now.Year);

            var vm = new MemberDashboardViewModel
            {
                UserName = user.UserName,
                IsMembershipActive = user.Role == UserRole.Member,
                EventsOrganizedCount = await _context.Events.CountAsync(e => e.OrganizerId == userId.Value),
                PendingInvitationsCount = recentInvitations.Count,
                RemainingEventLimit = Math.Max(0, monthlyLimit - eventsThisMonth),
                RecentInvitations = recentInvitations,
                UpcomingEvents = upcomingEvents
            };

            return View("MemberDashboard", vm);
        }

        // POST: External user upgrades to Member
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BecomeMember()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return RedirectToAction(nameof(Login));

            var user = await _userService.GetUserByIdAsync(userId.Value);
            if (user == null) return NotFound();

            if (user.Role == UserRole.External)
            {
                user.Role = UserRole.Member;
                user.IsActive = true;
                await _userService.UpdateUserAsync(user);
                TempData["StatusMessage"] = "Membership activated! Welcome, Member!";
            }

            return RedirectToAction(nameof(Dashboard));
        }
        // External users can apply for membership
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyMembership()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return RedirectToAction(nameof(Login));

            var user = await _userService.GetUserByIdAsync(userId.Value);
            if (user == null) return NotFound();

            if (user.Role == UserRole.External)
            {
                user.Role = UserRole.Member;
                user.IsActive = true;
                await _userService.UpdateUserAsync(user);
                TempData["StatusMessage"] = "Membership approved! You are now a Member.";
            }
            else
            {
                TempData["StatusMessage"] = "You already have an active membership.";
            }

            return RedirectToAction(nameof(Profile));
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
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
            if (user == null) return NotFound();

            user.UserName = userName;
            user.Email = email;
            user.Role = role;
            user.IsActive = isActive;

            var success = await _userService.UpdateUserAsync(user);
            if (!success) return NotFound();

            TempData["StatusMessage"] = "User updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success) return NotFound();

            TempData["StatusMessage"] = "User deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["StatusMessage"] = "You have been signed out.";
            return RedirectToAction("Index", "Home");
        }
    }
}