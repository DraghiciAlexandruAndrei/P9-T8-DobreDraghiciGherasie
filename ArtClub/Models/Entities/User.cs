using ArtClub.Models.Enums;

namespace ArtClub.Models.Entities
{
    public abstract class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
        public bool IsBanned { get; set; } = false;

        // Navigări
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<Invitation> ReceivedInvitations { get; set; } = new List<Invitation>();
    }
}