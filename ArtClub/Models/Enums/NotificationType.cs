namespace ArtClub.Models.Enums
{
    /// <summary>
    /// Represents the type of notification sent to users.
    /// Different notification types trigger different message content and delivery methods.
    /// Implements REQ-31 to REQ-35: Notification system with email and in-app support.
    /// </summary>
    public enum NotificationType
    {
        /// <summary>User has been invited to an event (REQ-31, REQ-32)</summary>
        Invitation = 0,

        /// <summary>Payment was confirmed and processed successfully (REQ-43)</summary>
        PaymentConfirmed = 1,

        /// <summary>Payment failed or was declined (REQ-43)</summary>
        PaymentFailed = 2,

        /// <summary>Reservation was confirmed by admin or automatically</summary>
        ReservationConfirmed = 3,

        /// <summary>Reservation was cancelled</summary>
        ReservationCancelled = 4,

        /// <summary>Event organizer updated event details (REQ-30)</summary>
        EventUpdate = 5,

        /// <summary>System notification, maintenance, or general message</summary>
        SystemNotification = 6
    }
}
