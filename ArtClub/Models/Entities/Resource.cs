using System.Collections.Generic;
using ArtClub.Models.Enums;

namespace ArtClub.Models.Entities
{
    public class Resource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Capacity for venues (null for equipment)
        public int? Capacity { get; set; }

        public decimal BasePrice { get; set; }

        // Resource type: categorizes what this resource is (REQ-15, REQ-22, REQ-26)
        public ResourceType Type { get; set; }

        // For equipment: quantity available (can have multiple)
        // For venues: usually 1 (single room/hall)
        public int QuantityAvailable { get; set; } = 1;

        // Location/Address information (REQ-36)
        public string? Location { get; set; }

        // Is this an affiliated/external venue? (REQ-36)
        public bool IsAffiliatedVenue { get; set; } = false;

        // When was this resource added to the system
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Image/thumbnail for display in listings
        public string? ImageUrl { get; set; }

        // Whether resource is currently available for booking
        public bool IsActive { get; set; } = true;

        // Proprietăți de navigare (Relațiile din diagrama ta)

        // O resursă poate avea mai multe rezervări în timp
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        // O resursă poate fi asociată cu mai multe evenimente (istoric)
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }
}