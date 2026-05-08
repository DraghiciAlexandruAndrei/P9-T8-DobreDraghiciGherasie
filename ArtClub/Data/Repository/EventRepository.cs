using ArtClub.DataAccess.Interfaces;
using ArtClub.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArtClub.DataAccess.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;
        public EventRepository(ApplicationDbContext context) => _context = context;

        public async Task<List<Event>> GetAllWithDetailsAsync() =>
            await _context.Events.Include(e => e.Resource).Include(e => e.Reservation)
                                 .Include(e => e.Invitations).Include(e => e.Organizer)
                                 .OrderBy(e => e.Reservation.StartTime).ToListAsync();

        public async Task<Event?> GetByTitleWithDetailsAsync(string title) =>
            await _context.Events.Include(e => e.Resource).Include(e => e.Reservation)
                                 .Include(e => e.Invitations).Include(e => e.Organizer)
                                 .FirstOrDefaultAsync(e => e.Title == title);

        public async Task<Event?> GetByIdWithReservationAsync(int id) =>
            await _context.Events.Include(e => e.Reservation).FirstOrDefaultAsync(e => e.Id == id);

        public async Task<Resource?> GetResourceByNameAsync(string name) =>
            await _context.Resources.FirstOrDefaultAsync(r => r.Name == name);

        public async Task<int?> GetFirstUserIdAsync() =>
            (await _context.Users.OrderBy(u => u.Id).FirstOrDefaultAsync())?.Id;

        public async Task AddAsync(Event artEvent)
        {
            // Verifică manual dacă obiectul este null înainte de adăugare
            if (artEvent == null) return;

            // Folosim direct contextul pentru a asigura track-uirea întregului graf de obiecte (Event + Reservation)
            await _context.AddAsync(artEvent);
        }

        public void Remove(Event artEvent) => _context.Events.Remove(artEvent);

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;

        public async Task AddInvitationAsync(Invitation invitation) => await _context.Invitations.AddAsync(invitation);

        public async Task<Invitation?> GetInvitationByIdAsync(int id) => await _context.Invitations.FindAsync(id);

        public async Task<List<Resource>> GetAllResourcesAsync()
        {
            return await _context.Resources
                .OrderBy(r => r.Name)
                .ToListAsync();
        }
    }
}