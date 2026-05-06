using ArtClub.DataAccess.Interfaces;
using ArtClub.Models.Entities;
using ArtClub.Services.Interfaces;
using PdfSharp.Drawing;
using PdfSharp.Pdf;


namespace ArtClub.Services.Implementations
{
    public class FinanceService : IFinanceService
    {
        private readonly IPaymentRepository _paymentRepo;

        public FinanceService(IPaymentRepository paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        // --- Metodele care lipseau ---

        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            // Apelăm repository-ul pentru a lua lista ordonată
            return await _paymentRepo.GetAllOrderedByDateAsync();
        }

        public async Task<decimal> GetTotalExpensesAsync()
        {
            // Calculăm totalul cheltuielilor (IsIncome = false)
            return await _paymentRepo.GetTotalSumAsync(false);
        }

        public async Task<decimal> GetTotalIncomeAsync()
        {
            // Calculăm totalul veniturilor (IsIncome = true)
            return await _paymentRepo.GetTotalSumAsync(true);
        }

        // --- Restul logicii de Business ---

        public async Task<decimal> CalculateMonthlyBalanceAsync()
        {
            var now = DateTime.Now;
            var income = await _paymentRepo.GetSumAsync(now.Month, now.Year, true);
            var expenses = await _paymentRepo.GetSumAsync(now.Month, now.Year, false);

            return income - expenses;
        }

        public async Task<bool> HasClubSufficientFundsAsync(decimal projectedCost)
        {
            var totalIncome = await GetTotalIncomeAsync();
            var totalExpenses = await GetTotalExpensesAsync();

            return (totalIncome - totalExpenses) >= projectedCost;
        }

        public async Task<byte[]> GenerateMonthlyReportAsync(int month, int year)
        {
            var income = await _paymentRepo.GetSumAsync(month, year, true);
            var expenses = await _paymentRepo.GetSumAsync(month, year, false);
            var payments = await _paymentRepo.GetPaymentsByPeriodAsync(month, year);

            // Creare document
            using (var document = new PdfDocument())
            {
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var fontTitle = new XFont("Verdana", 20, XFontStyleEx.Bold);
                var fontText = new XFont("Verdana", 12, XFontStyleEx.Regular);

                // Header
                gfx.DrawString($"Raport Financiar {month}/{year}", fontTitle, XBrushes.Black,
                    new XRect(0, 40, page.Width, 40), XStringFormats.Center);

                // Rezumat
                int yPos = 100;
                gfx.DrawString($"Total Venituri: {income} lei", fontText, XBrushes.Green, 50, yPos);
                gfx.DrawString($"Total Cheltuieli: {expenses} lei", fontText, XBrushes.Red, 50, yPos + 20);
                gfx.DrawString($"Bilanț: {income - expenses} lei", fontText, XBrushes.Black, 50, yPos + 40);

                // Tabel Tranzacții
                yPos += 80;
                gfx.DrawString("Data", fontText, XBrushes.Gray, 50, yPos);
                gfx.DrawString("Suma", fontText, XBrushes.Gray, 200, yPos);
                gfx.DrawString("Tip", fontText, XBrushes.Gray, 350, yPos);

                foreach (var p in payments)
                {
                    yPos += 20;
                    gfx.DrawString(p.Date.ToShortDateString(), fontText, XBrushes.Black, 50, yPos);
                    gfx.DrawString($"{p.Amount} lei", fontText, XBrushes.Black, 200, yPos);
                    gfx.DrawString(p.IsIncome ? "Venit" : "Cheltuială", fontText, XBrushes.Black, 350, yPos);

                    // Verificare limită pagină
                    if (yPos > page.Height - 50) break;
                }

                using (var stream = new MemoryStream())
                {
                    document.Save(stream, false);
                    return stream.ToArray();
                }
            }
        }

        public async Task<Payment?> GetPaymentByIdAsync(int id) => await _paymentRepo.GetByIdAsync(id);

        public async Task CreatePaymentAsync(Payment payment)
        {
            await _paymentRepo.AddAsync(payment);
            await _paymentRepo.SaveChangesAsync();
        }

        public async Task<bool> UpdatePaymentAsync(Payment payment)
        {
            var existing = await _paymentRepo.GetByIdAsync(payment.Id);
            if (existing == null) return false;

            existing.Amount = payment.Amount;
            existing.Date = payment.Date;
            existing.IsIncome = payment.IsIncome;

            return await _paymentRepo.SaveChangesAsync();
        }

        public async Task<bool> DeletePaymentAsync(int id)
        {
            var payment = await _paymentRepo.GetByIdAsync(id);
            if (payment == null) return false;

            _paymentRepo.Remove(payment);
            return await _paymentRepo.SaveChangesAsync();
        }
    }
}