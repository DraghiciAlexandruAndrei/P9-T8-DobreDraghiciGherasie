using ArtClub.Models.Entities;

namespace ArtClub.Services.Interfaces
{
    public interface IFinanceService
    {
        Task<decimal> CalculateMonthlyBalanceAsync();
        Task<bool> HasClubSufficientFundsAsync(decimal projectedCost);
        Task<byte[]> GenerateMonthlyReportAsync(int month, int year);

        Task<decimal> GetTotalIncomeAsync();
        Task<decimal> GetTotalExpensesAsync();
        Task<List<Payment>> GetAllPaymentsAsync();
        Task<Payment?> GetPaymentByIdAsync(int id);
        Task CreatePaymentAsync(Payment payment);
        Task<bool> UpdatePaymentAsync(Payment payment);
        Task<bool> DeletePaymentAsync(int id);
    }
}