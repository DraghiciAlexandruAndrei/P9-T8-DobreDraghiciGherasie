using ArtClub.Models.Entities;

namespace ArtClub.Services.Interfaces
{
    public interface IReservationService
    {
        Task<bool> CheckAvailabilityAsync(int resourceId, DateTime start, DateTime end);
        Task CreateReservationAsync(Reservation reservation);
    }
}