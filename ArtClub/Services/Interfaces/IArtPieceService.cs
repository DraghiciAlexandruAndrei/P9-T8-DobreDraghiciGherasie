using ArtClub.Models.Entities;

namespace ArtClub.Services.Interfaces
{
    public interface IArtPieceService
    {
        Task<List<ArtPiece>> GetPopularPiecesAsync();
        Task AddArtPieceToEventAsync(int eventId, int artPieceId);
        Task<List<ArtPiece>> GetAvailablePiecesForEventAsync(int eventId);
    }
}