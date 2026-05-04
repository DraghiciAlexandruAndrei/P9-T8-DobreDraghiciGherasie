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

        public async Task<List<ArtPiece>> GetAllArtPiecesAsync()
        {
            return await _context.ArtPieces
                .OrderBy(ap => ap.Title)
                .ToListAsync();
        }

        public async Task<List<ArtPiece>> GetPopularPiecesAsync()
        {
            return await _context.ArtPieces
                .Where(ap => ap.IsPopular)
                .OrderBy(ap => ap.Title)
                .ToListAsync();
        }

        public async Task<ArtPiece?> GetArtPieceByIdAsync(int id)
        {
            return await _context.ArtPieces
                .FirstOrDefaultAsync(ap => ap.Id == id);
        }

        public async Task CreateArtPieceAsync(ArtPiece artPiece)
        {
            _context.ArtPieces.Add(artPiece);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateArtPieceAsync(ArtPiece artPiece)
        {
            var existingArtPiece = await _context.ArtPieces
                .FirstOrDefaultAsync(ap => ap.Id == artPiece.Id);

            if (existingArtPiece == null)
                return false;

            existingArtPiece.Title = artPiece.Title;
            existingArtPiece.Creator = artPiece.Creator;
            existingArtPiece.ImageUrl = artPiece.ImageUrl;
            existingArtPiece.IsPopular = artPiece.IsPopular;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteArtPieceAsync(int id)
        {
            var artPiece = await _context.ArtPieces
                .FirstOrDefaultAsync(ap => ap.Id == id);

            if (artPiece == null)
                return false;

            _context.ArtPieces.Remove(artPiece);
            await _context.SaveChangesAsync();

            return true;
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