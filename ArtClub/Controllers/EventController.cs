using ArtClub.Models.ViewModels;
using ArtClub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ArtClub.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IArtPieceService _artPieceService;

        public EventController(IEventService eventService, IArtPieceService artPieceService)
        {
            _eventService = eventService;
            _artPieceService = artPieceService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index() => View();

        [Authorize]
        public IActionResult Create() => View();

        [HttpPost, Authorize]
        public async Task<IActionResult> Create(EventCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            // Logica de mapare din ViewModel în Entitate și salvare
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            // Exemplu de populare EventDetailsViewModel
            var model = new EventDetailsViewModel { EventId = id, Title = "Exemplu" };
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Cancel(int id)
        {
            await _eventService.CancelEventAsync(id);
            return RedirectToAction("Index");
        }
    }
}