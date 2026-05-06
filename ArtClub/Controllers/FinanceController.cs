using ArtClub.Models.Entities;
using ArtClub.Models.Enums;
using ArtClub.Models.ViewModels;
using ArtClub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ArtClub.Controllers
{
    public class FinanceController : Controller
    {
        private readonly IFinanceService _financeService;

        public FinanceController(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Dashboard));
        }

        public async Task<IActionResult> Dashboard()
        {
            var income = await _financeService.GetTotalIncomeAsync();
            var expenses = await _financeService.GetTotalExpensesAsync();
            var balance = income - expenses;
            var payments = await _financeService.GetAllPaymentsAsync();

            var model = new FinanceDashboardViewModel
            {
                TotalIncome = income,
                TotalExpenses = expenses,
                NetBalance = balance,
                HasTheClubEnoughMoney = balance >= 0,
                RecentTransactions = payments
                    .Take(5)
                    .Select(p => $"{(p.IsIncome ? "Income" : "Expense")} - {p.Amount} lei - {p.Date:dd.MM.yyyy}")
                    .ToList()
            };

            return View("Index", model);
        }

        public async Task<IActionResult> Payments()
        {
            var payments = await _financeService.GetAllPaymentsAsync();
            return View(payments);
        }

        public IActionResult Create()
        {
            return View(new Payment
            {
                Date = DateTime.Now,
                IsIncome = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(decimal amount, DateTime date, bool isIncome)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                ModelState.AddModelError("", "You must be logged in to create a payment.");
                return View(new Payment
                {
                    Amount = amount,
                    Date = date,
                    IsIncome = isIncome
                });
            }

            if (amount <= 0)
            {
                ModelState.AddModelError("Amount", "Amount must be greater than 0.");
                return View(new Payment
                {
                    Amount = amount,
                    Date = date,
                    IsIncome = isIncome
                });
            }

            var payment = new Payment
            {
                UserId = userId.Value,
                Amount = amount,
                Date = date,
                IsIncome = isIncome,
                Type = isIncome ? PaymentType.Subscription : PaymentType.Expense
            };

            await _financeService.CreatePaymentAsync(payment);

            TempData["StatusMessage"] = "Payment created successfully.";
            return RedirectToAction(nameof(Payments));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var payment = await _financeService.GetPaymentByIdAsync(id);

            if (payment == null)
                return NotFound();

            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, decimal amount, DateTime date, bool isIncome)
        {
            var existingPayment = await _financeService.GetPaymentByIdAsync(id);

            if (existingPayment == null)
                return NotFound();

            if (amount <= 0)
            {
                ModelState.AddModelError("Amount", "Amount must be greater than 0.");
                existingPayment.Amount = amount;
                existingPayment.Date = date;
                existingPayment.IsIncome = isIncome;
                return View(existingPayment);
            }

            existingPayment.Amount = amount;
            existingPayment.Date = date;
            existingPayment.IsIncome = isIncome;
            existingPayment.Type = isIncome ? PaymentType.Subscription : PaymentType.Expense;

            var success = await _financeService.UpdatePaymentAsync(existingPayment);

            if (!success)
                return NotFound();

            TempData["StatusMessage"] = "Payment updated successfully.";
            return RedirectToAction(nameof(Payments));
        }

        public async Task<IActionResult> Details(int id)
        {
            var payment = await _financeService.GetPaymentByIdAsync(id);

            if (payment == null)
                return NotFound();

            return View(payment);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _financeService.GetPaymentByIdAsync(id);

            if (payment == null)
                return NotFound();

            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _financeService.DeletePaymentAsync(id);

            if (!success)
                return NotFound();

            TempData["StatusMessage"] = "Payment deleted successfully.";
            return RedirectToAction(nameof(Payments));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(int eventId, decimal amount)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "You must be logged in to pay for an event.";
                return RedirectToAction("Index", "Event");
            }

            if (amount <= 0)
            {
                TempData["ErrorMessage"] = "Invalid payment amount.";
                return RedirectToAction("Details", "Event", new { id = eventId });
            }

            var payment = new Payment
            {
                UserId = userId.Value,
                Amount = amount,
                Date = DateTime.Now,
                IsIncome = true,
                Type = PaymentType.Booking,
                Description = $"Plată eveniment ID: {eventId}"
            };

            await _financeService.CreatePaymentAsync(payment);
            await _financeService.RegisterEventExpensesAsync(eventId, amount);

            TempData["StatusMessage"] = "Payment completed successfully.";
            return RedirectToAction("Details", "Event", new { id = eventId });
        }

        public async Task<IActionResult> GenerateReport(int month, int year)
        {
            var report = await _financeService.GenerateMonthlyReportAsync(month, year);

            return File(report, "application/pdf", $"Raport-{month:D2}-{year}.pdf");
        }
    }
}