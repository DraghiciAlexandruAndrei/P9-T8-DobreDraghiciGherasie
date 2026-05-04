using ArtClub.DataAccess;
using ArtClub.Models.Entities;
using ArtClub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArtClub.Services.Implementations
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _context;

        public ReservationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckAvailabilityAsync(int resourceId, DateTime start, DateTime end)
        {
            // Calculăm intervalul extins cu buffer-ul de 1 zi cerut în diagramă
            var requestedBufferStart = start.AddDays(-1);
            var requestedBufferEnd = end.AddDays(1);

            // Verificăm dacă există vreo rezervare care se suprapune cu intervalul nostru
            // Logica de suprapunere: (StartA < EndB) AND (EndA > StartB)
            var isOverlapping = await _context.Reservations
                .AnyAsync(r => r.ResourceId == resourceId &&
                               requestedBufferStart < r.EndTime.AddDays(1) &&
                               requestedBufferEnd > r.StartTime.AddDays(-1));

            // Dacă este suprapunere, returnăm false (nu e disponibil)
            return !isOverlapping;
        }

        public async Task CreateReservationAsync(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
        }
    }
}