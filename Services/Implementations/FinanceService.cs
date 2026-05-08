using ArtClub.DataAccess;
using ArtClub.Models.Entities;
using ArtClub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace ArtClub.Services.Implementations
{
    public class FinanceService : IFinanceService
    {
        private readonly ApplicationDbContext _context;

        public FinanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> CalculateMonthlyBalanceAsync()
        {
            var now = DateTime.Now;

            var income = await _context.Payments
                .Where(p => p.Date.Month == now.Month && p.Date.Year == now.Year && p.IsIncome)
                .SumAsync(p => p.Amount);

            var expenses = await _context.Payments
                .Where(p => p.Date.Month == now.Month && p.Date.Year == now.Year && !p.IsIncome)
                .SumAsync(p => p.Amount);

            return income - expenses;
        }

        public async Task<bool> HasClubSufficientFundsAsync(decimal projectedCost)
        {
            var totalIncome = await GetTotalIncomeAsync();
            var totalExpenses = await GetTotalExpensesAsync();

            var currentLiquidity = totalIncome - totalExpenses;

            return currentLiquidity >= projectedCost;
        }

        public async Task<byte[]> GenerateMonthlyReportAsync(int month, int year)
        {
            var income = await _context.Payments
                .Where(p => p.Date.Month == month && p.Date.Year == year && p.IsIncome)
                .SumAsync(p => p.Amount);

            var expenses = await _context.Payments
                .Where(p => p.Date.Month == month && p.Date.Year == year && !p.IsIncome)
                .SumAsync(p => p.Amount);

            var balance = income - expenses;

            using var document = new PdfDocument();
            document.Info.Title = $"ArtClub Finance Report {month:D2}/{year}";

            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            var titleFont = new XFont("Arial", 18, XFontStyle.Bold);
            var headerFont = new XFont("Arial", 12, XFontStyle.Bold);
            var normalFont = new XFont("Arial", 12, XFontStyle.Regular);

            double y = 60;

            gfx.DrawString("ArtClub Finance Report", titleFont, XBrushes.Black,
                new XRect(40, y, page.Width - 80, 30), XStringFormats.TopLeft);

            y += 45;

            gfx.DrawString($"Month: {month:D2}/{year}", normalFont, XBrushes.Black, 40, y);
            y += 30;

            gfx.DrawString("Summary", headerFont, XBrushes.Black, 40, y);
            y += 25;

            gfx.DrawString($"Income: {income} lei", normalFont, XBrushes.Black, 60, y);
            y += 22;

            gfx.DrawString($"Expenses: {expenses} lei", normalFont, XBrushes.Black, 60, y);
            y += 22;

            gfx.DrawString($"Net balance: {balance} lei", normalFont, XBrushes.Black, 60, y);
            y += 35;

            var payments = await _context.Payments
                  .Where(p => p.Date.Month == month && p.Date.Year == year)
                  .OrderBy(p => p.Date)
                  .ToListAsync();

            gfx.DrawString("Transactions:", headerFont, XBrushes.Black, 40, y);
            y += 25;

            if (payments.Any())
            {
                foreach (var p in payments)
                {
                    var text = $"{p.Date:dd.MM.yyyy} - {(p.IsIncome ? "Income" : "Expense")} - {p.Amount:F2} lei";
                    gfx.DrawString(text, normalFont, XBrushes.Black, 60, y);
                    y += 20;
                }
            }
            else
            {
                gfx.DrawString("No transactions for this month.", normalFont, XBrushes.Black, 60, y);
                y += 20;
            }

            y += 20;
            gfx.DrawString($"Generated at: {DateTime.Now:dd.MM.yyyy HH:mm}", normalFont, XBrushes.Black, 40, y);

            using var stream = new MemoryStream();
            document.Save(stream, false);

            return stream.ToArray();
        }
        public async Task<decimal> GetTotalIncomeAsync()
        {
            return await _context.Payments
                .Where(p => p.IsIncome)
                .SumAsync(p => p.Amount);
        }

        public async Task<decimal> GetTotalExpensesAsync()
        {
            return await _context.Payments
                .Where(p => !p.IsIncome)
                .SumAsync(p => p.Amount);
        }

        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Payments
                .OrderByDescending(p => p.Date)
                .ToListAsync();
        }

        public async Task<Payment?> GetPaymentByIdAsync(int id)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreatePaymentAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePaymentAsync(Payment payment)
        {
            var existingPayment = await _context.Payments
                .FirstOrDefaultAsync(p => p.Id == payment.Id);

            if (existingPayment == null)
                return false;

            existingPayment.Amount = payment.Amount;
            existingPayment.Date = payment.Date;
            existingPayment.IsIncome = payment.IsIncome;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePaymentAsync(int id)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
                return false;

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}