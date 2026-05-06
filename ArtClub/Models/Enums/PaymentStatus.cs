namespace ArtClub.Models.Enums
{
    /// <summary>
    /// Represents the status of a payment transaction.
    /// Tracks whether a payment is pending confirmation, confirmed, cancelled, or refunded.
    /// Implements REQ-43 to REQ-51: Payment system tracking and financial rules.
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>Payment initiated but not yet confirmed</summary>
        Pending = 0,

        /// <summary>Payment confirmed and successfully processed</summary>
        Confirmed = 1,

        /// <summary>Payment was cancelled by user or system</summary>
        Cancelled = 2,

        /// <summary>Payment was refunded to user</summary>
        Refunded = 3
    }
}
