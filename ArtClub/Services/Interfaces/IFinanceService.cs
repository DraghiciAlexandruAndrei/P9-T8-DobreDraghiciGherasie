namespace ArtClub.Services.Interfaces
{
    public interface IFinanceService
    {
        Task<decimal> CalculateMonthlyBalanceAsync();
        Task<bool> HasClubSufficientFundsAsync(decimal projectedCost);
        Task<byte[]> GenerateMonthlyReportAsync(int month, int year);
    }
}