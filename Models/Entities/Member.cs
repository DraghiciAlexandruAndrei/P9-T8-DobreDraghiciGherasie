using ArtClub.Models.Entities;

namespace ArtClub.Models.Entities
{
    public class Member : User
    {
        public DateTime MembershipDate { get; set; }
        public int EventCreationLimit { get; set; } = 5; // Exemplu limită

        public bool CheckEventLimit() => OrganizedEvents.Count < EventCreationLimit;

        public virtual ICollection<Event> OrganizedEvents { get; set; } = new List<Event>();
    }
}
