using ArtClub.DataAccess;
using ArtClub.Models.Entities;
using ArtClub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArtClub.Services.Implementations
{
    public class ArtPieceService : IArtPieceService
    {
        private readonly ApplicationDbContext _context;

        public ArtPieceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ArtPiece>> GetPopularPiecesAsync()
        {
            return await _context.ArtPieces
                .Where(ap => ap.IsPopular)
                .ToListAsync();
        }

        public async Task AddArtPieceToEventAsync(int eventId, int artPieceId)
        {
            var eventArtPiece = new EventArtPiece
            {
                EventId = eventId,
                ArtPieceId = artPieceId
            };

            _context.EventArtPieces.Add(eventArtPiece);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ArtPiece>> GetAvailablePiecesForEventAsync(int eventId)
        {
            // Returnăm piesele care NU sunt deja atribuite acestui eveniment
            var assignedIds = await _context.EventArtPieces
                .Where(eap => eap.EventId == eventId)
                .Select(eap => eap.ArtPieceId)
                .ToListAsync();

            return await _context.ArtPieces
                .Where(ap => !assignedIds.Contains(ap.Id))
                .ToListAsync();
        }
    }
}