using ArtClub.DataAccess;
using ArtClub.Models.Enums;
using ArtClub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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

            // Calculăm toate veniturile din luna curentă
            var income = await _context.Payments
                .Where(p => p.Date.Month == now.Month && p.Date.Year == now.Year && p.IsIncome)
                .SumAsync(p => p.Amount);

            // Calculăm toate cheltuielile din luna curentă
            var expenses = await _context.Payments
                .Where(p => p.Date.Month == now.Month && p.Date.Year == now.Year && !p.IsIncome)
                .SumAsync(p => p.Amount);

            return income - expenses;
        }

        public async Task<bool> HasClubSufficientFundsAsync(decimal projectedCost)
        {
            // Verificăm dacă balanța totală (istorică) acoperă costul noului eveniment
            var totalIncome = await _context.Payments.Where(p => p.IsIncome).SumAsync(p => p.Amount);
            var totalExpenses = await _context.Payments.Where(p => !p.IsIncome).SumAsync(p => p.Amount);

            var currentLiquidity = totalIncome - totalExpenses;

            return currentLiquidity >= projectedCost;
        }

        public async Task<byte[]> GenerateMonthlyReportAsync(int month, int year)
        {
            // Aici, într-o aplicație reală, s-ar folosi o librărie precum iTextSharp sau QuestPDF.
            // Pentru acest stadiu, returnăm un array de bytes care simulează un fișier text.

            var balance = await CalculateMonthlyBalanceAsync();
            string reportContent = $"Raport Financiar ArtClub - Luna {month}/{year}\n" +
                                   $"Balanța totală: {balance} RON\n" +
                                   $"Generat la: {DateTime.Now}";

            return System.Text.Encoding.UTF8.GetBytes(reportContent);
        }
    }
}