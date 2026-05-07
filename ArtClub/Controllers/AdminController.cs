using ArtClub.DataAccess;
using ArtClub.Models.Entities;
using ArtClub.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtClub.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET /Admin
        public async Task<IActionResult> Index()
        {
            var vm = new AdminDashboardViewModel
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalResources = await _context.Resources.CountAsync(),
                TotalEvents = await _context.Events.CountAsync(),
                TotalPayments = await _context.Payments.CountAsync(),
                MonthlyIncome = await _context.Payments
                    .Where(p => p.IsIncome && p.Date.Month == DateTime.Now.Month && p.Date.Year == DateTime.Now.Year)
                    .SumAsync(p => (decimal?)p.Amount) ?? 0,
                MonthlyExpenses = await _context.Payments
                    .Where(p => !p.IsIncome && p.Date.Month == DateTime.Now.Month && p.Date.Year == DateTime.Now.Year)
                    .SumAsync(p => (decimal?)p.Amount) ?? 0
            };
            return View(vm);
        }

        // GET /Admin/Users?search=...
        public async Task<IActionResult> Users(string? search)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower();
                query = query.Where(u =>
                    u.UserName.ToLower().Contains(s) ||
                    u.Email.ToLower().Contains(s));
            }

            var users = await query.ToListAsync();
            ViewBag.Search = search;
            return View(users);
        }

        // POST /Admin/BanUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BanUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.IsBanned = true;
            user.IsActive = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Users));
        }

        // POST /Admin/UnbanUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnbanUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.IsBanned = false;
            user.IsActive = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Users));
        }

        // GET /Admin/EditRole/5
        [HttpGet]
        public async Task<IActionResult> EditRole(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var vm = new EditUserRoleViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                CurrentRole = user.Role
            };
            return View(vm);
        }

        // POST /Admin/EditRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(EditUserRoleViewModel vm)
        {
            var user = await _context.Users.FindAsync(vm.UserId);
            if (user == null) return NotFound();

            user.Role = vm.NewRole;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Users));
        }

        // GET /Admin/Payments?userId=&from=&to=&isIncome=
        public async Task<IActionResult> Payments(int? userId, DateTime? from, DateTime? to, bool? isIncome)
        {
            var query = _context.Payments.Include(p => p.User).AsQueryable();

            if (userId.HasValue)
                query = query.Where(p => p.UserId == userId.Value);
            if (from.HasValue)
                query = query.Where(p => p.Date >= from.Value);
            if (to.HasValue)
                query = query.Where(p => p.Date <= to.Value);
            if (isIncome.HasValue)
                query = query.Where(p => p.IsIncome == isIncome.Value);

            var payments = await query.OrderByDescending(p => p.Date).ToListAsync();
            return View(payments);
        }

        // POST /Admin/RecordPayment  (REQ-43)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecordPayment(Payment payment)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Payments));

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Payments));
        }

        // GET /Admin/Resources
        public async Task<IActionResult> Resources()
        {
            var resources = await _context.Resources.ToListAsync();
            return View(resources);
        }

        // GET /Admin/CreateResource
        [HttpGet]
        public IActionResult CreateResource() => View();

        // POST /Admin/CreateResource
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateResource(Resource resource)
        {
            if (!ModelState.IsValid) return View(resource);
            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Resources));
        }

        // GET /Admin/EditResource/5
        [HttpGet]
        public async Task<IActionResult> EditResource(int id)
        {
            var r = await _context.Resources.FindAsync(id);
            if (r == null) return NotFound();
            return View(r);
        }

        // POST /Admin/EditResource
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditResource(Resource resource)
        {
            if (!ModelState.IsValid) return View(resource);
            _context.Resources.Update(resource);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Resources));
        }

        // POST /Admin/DeleteResource/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteResource(int id)
        {
            var r = await _context.Resources.FindAsync(id);
            if (r != null)
            {
                _context.Resources.Remove(r);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Resources));
        }

        // GET /Admin/Reports  (REQ-49, REQ-50)
        public async Task<IActionResult> Reports(int? year, int? month)
        {
            year ??= DateTime.Now.Year;
            month ??= DateTime.Now.Month;

            var income = await _context.Payments
                .Where(p => p.IsIncome && p.Date.Year == year && p.Date.Month == month)
                .SumAsync(p => (decimal?)p.Amount) ?? 0;

            var expenses = await _context.Payments
                .Where(p => !p.IsIncome && p.Date.Year == year && p.Date.Month == month)
                .SumAsync(p => (decimal?)p.Amount) ?? 0;

            var vm = new MonthlyReportViewModel
            {
                Year = year.Value,
                Month = month.Value,
                TotalIncome = income,
                TotalExpenses = expenses,
                Balance = income - expenses,
                MembersBlocked = income < expenses  // REQ-47
            };

            return View(vm);
        }
    }
}