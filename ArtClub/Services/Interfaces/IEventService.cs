using ArtClub.Models.Entities;

namespace ArtClub.Services.Interfaces
{
    public interface IEventService
    {
        Task<bool> CreateEventAsync(Event model);
        Task<bool> CancelEventAsync(int eventId);
        Task SendInvitationAsync(int eventId, int inviteeId);
        Task RespondToInvitationAsync(int invitationId, bool accept);
    }
}