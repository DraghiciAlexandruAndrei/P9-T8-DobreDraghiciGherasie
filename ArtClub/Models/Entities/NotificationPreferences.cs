using System;

namespace ArtClub.Models.Entities
{
    /// <summary>
    /// Represents user preferences for receiving notifications.
    /// Allows users to opt-in or opt-out of different notification types via email and in-app.
    /// Implements REQ-14: Support user notification preferences.
    /// </summary>
    public class NotificationPreferences
    {
        public int Id { get; set; }

        /// <summary>The user whose preferences these are</summary>
        public string UserId { get; set; }
        public virtual User User { get; set; }

        /// <summary>Whether to send email notifications for event invitations (REQ-34)</summary>
        public bool EmailOnInvitation { get; set; } = true;

        /// <summary>Whether to send email notifications for payment confirmations</summary>
        public bool EmailOnPayment { get; set; } = true;

        /// <summary>Whether to send email notifications for reservations</summary>
        public bool EmailOnReservation { get; set; } = true;

        /// <summary>Whether to show in-app notifications (REQ-35)</summary>
        public bool InAppNotifications { get; set; } = true;

        /// <summary>Whether to show in-app notifications for invitations</summary>
        public bool InAppOnInvitation { get; set; } = true;

        /// <summary>Whether to show in-app notifications for payments</summary>
        public bool InAppOnPayment { get; set; } = true;

        /// <summary>Whether to show in-app notifications for reservations</summary>
        public bool InAppOnReservation { get; set; } = true;

        /// <summary>Date when preferences were last updated</summary>
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
