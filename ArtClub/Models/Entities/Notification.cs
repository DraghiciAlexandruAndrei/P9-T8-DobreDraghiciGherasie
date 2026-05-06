using System;
using ArtClub.Models.Enums;

namespace ArtClub.Models.Entities
{
    /// <summary>
    /// Represents a notification sent to a user.
    /// Tracks all notifications (email and in-app) to maintain history and delivery status.
    /// Implements REQ-31 to REQ-35: Email and in-app notification system.
    /// Implements REQ-14: Track notification preferences.
    /// </summary>
    public class Notification
    {
        public int Id { get; set; }

        /// <summary>User who will receive this notification</summary>
        public string UserId { get; set; }
        public virtual User User { get; set; }

        /// <summary>
        /// Related invitation if this is an invitation notification.
        /// Can be null for other notification types.
        /// </summary>
        public int? InvitationId { get; set; }
        public virtual Invitation Invitation { get; set; }

        /// <summary>
        /// Related payment if this is a payment notification.
        /// Can be null for other notification types.
        /// </summary>
        public int? PaymentId { get; set; }
        public virtual Payment Payment { get; set; }

        /// <summary>
        /// Type of notification (Invitation, PaymentConfirmed, ReservationConfirmed, etc.)
        /// </summary>
        public NotificationType Type { get; set; }

        /// <summary>
        /// Message text to display to the user
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Indicates if the user has read/viewed this in-app notification
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// Indicates if the notification was sent via email (REQ-34)
        /// </summary>
        public bool EmailSent { get; set; }

        /// <summary>
        /// Date and time when the notification was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Date and time when email was sent (if EmailSent is true)
        /// </summary>
        public DateTime? EmailSentDate { get; set; }
    }
}
