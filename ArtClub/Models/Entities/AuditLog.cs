using System;

namespace ArtClub.Models.Entities
{
    /// <summary>
    /// Represents a security audit log entry.
    /// Logs all security-related events for compliance and debugging purposes.
    /// Implements SEC-06: Log all security-related events (failed logins, privilege escalations, payment attempts)
    /// with timestamps, user IDs, and IP addresses for audit trails.
    /// </summary>
    public class AuditLog
    {
        public int Id { get; set; }

        /// <summary>User who performed the action (null for anonymous/system actions)</summary>
        public string UserId { get; set; }
        public virtual User User { get; set; }

        /// <summary>
        /// Action/event type (e.g., "LoginFailed", "LoginSuccess", "PaymentAttempt", 
        /// "PrivilegeEscalation", "EventCreated", "ResourceDeleted")
        /// </summary>
        public string Action { get; set; }

        /// <summary>Type of entity involved (e.g., "User", "Event", "Payment", "Reservation")</summary>
        public string EntityType { get; set; }

        /// <summary>ID of the entity that was affected (if applicable)</summary>
        public int? EntityId { get; set; }

        /// <summary>IP address from which the action was performed</summary>
        public string IpAddress { get; set; }

        /// <summary>Whether the action succeeded or failed</summary>
        public bool Success { get; set; }

        /// <summary>
        /// Additional details in JSON format if needed.
        /// E.g., {"reason": "invalid_password", "attempts": 3}
        /// </summary>
        public string Details { get; set; }

        /// <summary>Date and time when the action occurred</summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
