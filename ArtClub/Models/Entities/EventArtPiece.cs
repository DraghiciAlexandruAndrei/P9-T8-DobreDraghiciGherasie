namespace ArtClub.Models.Entities
{
    public class EventArtPiece
    {
        public int EventId { get; set; }
        public virtual Event Event { get; set; }

        public int ResourceId { get; set; }
        public virtual Resource Resource { get; set; }
    }
}

