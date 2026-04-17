using ArtClub.Models.ViewModels;
using ArtClub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ArtClub.Controllers
{
    [Authorize(Roles = "Admin,Contabil")]
    public class FinanceController : Controller
    {
        private readonly IFinanceService _financeService;

        public FinanceController(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var model = new FinanceDashboardViewModel
            {
                NetBalance = await _financeService.CalculateMonthlyBalanceAsync()
            };
            return View(model);
        }

        public async Task<IActionResult> GenerateReport(int month, int year)
        {
            var report = await _financeService.GenerateMonthlyReportAsync(month, year);
            return File(report, "text/plain", "Raport.txt");
        }
    }
}