using System;

namespace ArtClub.Models.Entities
{
    /// <summary>
    /// Represents a monthly aggregated financial summary for the art club.
    /// Tracks total income and expenses per month to enforce financial rules.
    /// Implements REQ-47: Check member monthly balance to block reservations if negative.
    /// Implements REQ-50: Generate monthly financial reports.
    /// </summary>
    public class MonthlyFinancialSummary
    {
        public int Id { get; set; }

        /// <summary>Year of the financial summary (e.g., 2026)</summary>
        public int Year { get; set; }

        /// <summary>Month of the financial summary (1-12)</summary>
        public int Month { get; set; }

        /// <summary>Total income received during the month</summary>
        public decimal TotalIncome { get; set; }

        /// <summary>Total expenses recorded during the month</summary>
        public decimal TotalExpenses { get; set; }

        /// <summary>
        /// Computed: Balance = Income - Expenses
        /// Used to determine if member reservations should be blocked (REQ-47)
        /// </summary>
        public decimal Balance => TotalIncome - TotalExpenses;

        /// <summary>
        /// Computed: True if balance is negative, blocking new member reservations (REQ-47)
        /// </summary>
        public bool AreReservationsBlocked => Balance < 0;

        /// <summary>
        /// Date when this summary was calculated or last updated
        /// </summary>
        public DateTime CalculatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Optional notes or commentary about the month's finances
        /// </summary>
        public string Notes { get; set; }
    }
}
