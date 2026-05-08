namespace ArtClub.Models.Entities
{
    public class Admin : User
    {
        public int AdminLevel { get; set; }
        public bool CanOverrideReservations { get; set; } = true;
    }
}
