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
            var requestedBufferStart = start.AddDays(-1);
            var requestedBufferEnd = end.AddDays(1);

            var isOverlapping = await _context.Reservations
                .AnyAsync(r => r.ResourceId == resourceId &&
                               requestedBufferStart < r.EndTime.AddDays(1) &&
                               requestedBufferEnd > r.StartTime.AddDays(-1));

            return !isOverlapping;
        }

        public async Task CreateReservationAsync(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Resource>> GetAllResourcesAsync()
        {
            return await _context.Resources
                .Include(r => r.Reservations)
                .ToListAsync();
        }

        public async Task<Resource?> GetResourceByNameAsync(string name)
        {
            return await _context.Resources
                .Include(r => r.Reservations)
                .FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task CreateResourceAsync(Resource resource)
        {
            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateResourceAsync(string originalName, Resource model)
        {
            var resource = await _context.Resources
                .FirstOrDefaultAsync(r => r.Name == originalName);

            if (resource == null)
                return false;

            resource.Name = model.Name;
            resource.Description = model.Description;
            resource.Capacity = model.Capacity;
            resource.BasePrice = model.BasePrice;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteResourceAsync(string name)
        {
            var resource = await _context.Resources
                .FirstOrDefaultAsync(r => r.Name == name);

            if (resource == null)
                return false;

            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Reservation>> GetReservationCalendarAsync()
        {
            return await _context.Reservations
                .Include(r => r.Resource)
                .OrderBy(r => r.StartTime)
                .ToListAsync();
        }
    }
}