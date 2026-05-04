using ArtClub.Models.Enums;

namespace ArtClub.Models.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public EventStatus Status { get; set; } = EventStatus.Draft;

        //Buget eveniment
        public decimal Budget { get; set; }

        public int OrganizerId { get; set; }
        public virtual Member Organizer { get; set; }

        public int ResourceId { get; set; }
        public virtual Resource Resource { get; set; }


        public virtual Reservation Reservation { get; set; }


        // Relații Many-to-Many și invitații
        public virtual ICollection<EventArtPiece> EventArtPieces { get; set; } = new List<EventArtPiece>();
        public virtual ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();

        public void Activate() => Status = EventStatus.Active;
        public void Cancel() => Status = EventStatus.Cancelled;
    }
}