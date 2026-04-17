using ArtClub.Models.ViewModels;
using ArtClub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ArtClub.Controllers
{
    public class ArtPieceController : Controller
    {
        private readonly IArtPieceService _artPieceService;

        public ArtPieceController(IArtPieceService artPieceService)
        {
            _artPieceService = artPieceService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index() => View();

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id) => View();

        [Authorize(Roles = "Artist,Admin")]
        public async Task<IActionResult> Popular()
        {
            var pieces = await _artPieceService.GetPopularPiecesAsync();
            // Mapare către List<ArtPieceListViewModel>
            return View();
        }
    }
}