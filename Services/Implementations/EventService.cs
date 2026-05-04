using ArtClub.DataAccess;
using ArtClub.Models.Entities;
using ArtClub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArtClub.Services.Implementations
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;
        private readonly IReservationService _reservationService;
        private readonly IFinanceService _financeService;
        private readonly INotificationService _notificationService;

        public EventService(
            ApplicationDbContext context,
            IReservationService reservationService,
            IFinanceService financeService,
            INotificationService notificationService)
        {
            _context = context;
            _reservationService = reservationService;
            _financeService = financeService;
            _notificationService = notificationService;
        }

        public async Task<bool> CreateEventAsync(Event model)
        {
            // 1. Verificăm disponibilitatea resursei (SALA) cu tot cu BUFFER
            // model.Reservation conține StartTime și EndTime
            var isAvailable = await _reservationService.CheckAvailabilityAsync(
                model.ResourceId,
                model.Reservation.StartTime,
                model.Reservation.EndTime);

            if (!isAvailable) return false; // Sala e ocupată sau în perioada de buffer

            // 2. Verificăm dacă avem bani pentru acest eveniment (buget proiectat)
            var hasFunds = await _financeService.HasClubSufficientFundsAsync(model.Budget);
            if (!hasFunds) return false; // Clubul e pe minus, nu ne permitem evenimentul

            // 3. Dacă totul e OK, salvăm în baza de date
            // EF Core va salva automat și obiectul Reservation legat de Event
            _context.Events.Add(model);
            var success = await _context.SaveChangesAsync() > 0;

            // 4. Trimitem o notificare automată organizatorului
            if (success)
            {
                var organizer = await _context.Users.FindAsync(model.OrganizerId);

                if (organizer != null)
                {
                    await _notificationService.SendEmailAsync(
                        organizer.Email,
                        "Eveniment Creat",
                        $"Evenimentul '{model.Title}' a fost aprobat și programat.");
                }
            }

            return success;
        }

        public async Task<bool> CancelEventAsync(int eventId)
        {
            var eventToDelete = await _context.Events
                .Include(e => e.Reservation) // Ștergem și rezervarea din calendar
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventToDelete == null) return false;

            _context.Events.Remove(eventToDelete);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task SendInvitationAsync(int eventId, int inviteeId)
        {
            var user = await _context.Users.FindAsync(inviteeId);
            var ev = await _context.Events.FindAsync(eventId);

            if (user != null && ev != null)
            {
                // Creăm invitația în DB
                var invitation = new Invitation { EventId = eventId, InviteeId = inviteeId };
                _context.Invitations.Add(invitation);
                await _context.SaveChangesAsync();

                // Notificăm user-ul
                await _notificationService.SendEmailAsync(
                    user.Email,
                    "Invitație Eveniment Artă",
                    $"Ai fost invitat la {ev.Title}!");
            }
        }

        public async Task RespondToInvitationAsync(int invitationId, bool accept)
        {
            var inv = await _context.Invitations.FindAsync(invitationId);

            if (inv != null)
            {
                // În loc de inv.IsAccepted = accept, folosim logica ta din entitate:
                if (accept)
                {
                    inv.Accept(); // Setează statusul pe Accepted
                }
                else
                {
                    inv.Decline(); // Setează statusul pe Declined
                }

                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await _context.Events
                .Include(e => e.Resource)
                .Include(e => e.Reservation)
                .Include(e => e.Invitations)
                .Include(e => e.Organizer)
                .OrderBy(e => e.Reservation.StartTime)
                .ToListAsync();
        }

        public async Task<Event?> GetEventByTitleAsync(string title)
        {
            return await _context.Events
                .Include(e => e.Resource)
                .Include(e => e.Reservation)
                .Include(e => e.Invitations)
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(e => e.Title == title);
        }

        public async Task<bool> UpdateEventAsync(string originalTitle, Event model)
        {
            var ev = await _context.Events
                .Include(e => e.Reservation)
                .FirstOrDefaultAsync(e => e.Title == originalTitle);

            if (ev == null)
                return false;

            ev.Title = model.Title;
            ev.Description = model.Description;
            ev.ResourceId = model.ResourceId;

            if (ev.Reservation != null && model.Reservation != null)
            {
                ev.Reservation.ResourceId = model.Reservation.ResourceId;
                ev.Reservation.StartTime = model.Reservation.StartTime;
                ev.Reservation.EndTime = model.Reservation.EndTime;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEventByTitleAsync(string title)
        {
            var ev = await _context.Events
                .FirstOrDefaultAsync(e => e.Title == title);

            if (ev == null)
                return false;

            return await CancelEventAsync(ev.Id);
        }
        public async Task<Resource?> GetResourceByNameAsync(string resourceName)
        {
            return await _context.Resources
                .FirstOrDefaultAsync(r => r.Name == resourceName);
        }

        public async Task<int?> GetDefaultOrganizerIdAsync()
        {
            var user = await _context.Users
                .OrderBy(u => u.Id)
                .FirstOrDefaultAsync();

            return user?.Id;
        }
    }
}