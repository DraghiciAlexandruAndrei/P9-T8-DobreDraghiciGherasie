using ArtClub.Models.ViewModels;
using ArtClub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

//Pentru sali de inchiriat 
namespace ArtClub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ResourceController : Controller
    {
        private readonly IReservationService _reservationService;

        public ResourceController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public async Task<IActionResult> Index() => View();

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(ResourceCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id) => RedirectToAction("Index");
    }
}