using ArtClub.Models.Entities;
using ArtClub.Models.Enums;
using ArtClub.Models.ViewModels;
using ArtClub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArtClub.Controllers
{
    public class ResourceController : Controller
    {
        private readonly IReservationService _reservationService;

        public ResourceController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public async Task<IActionResult> Index()
        {
            var resources = await _reservationService.GetAllResourcesAsync();

            var model = resources.Select(r => new ResourceOverviewViewModel
            {
                Name = r.Name,
                Type = GetResourceTypeDisplay(r.Type),
                Capacity = r.Capacity,
                Location = r.Location ?? (IsVenueType(r.Type) ? "Club venue" : "Equipment"),
                Status = r.Reservations.Any(res =>
                    res.StartTime.AddDays(-1) <= DateTime.Now &&
                    res.EndTime.AddDays(1) >= DateTime.Now)
                    ? "Reserved"
                    : "Available",
                QuantityAvailable = r.QuantityAvailable,
                IsActive = r.IsActive,
                ImageUrl = r.ImageUrl
            }).ToList();

            return View(model);
        }

        // Separate view for venues/rooms only
        public async Task<IActionResult> Venues()
        {
            var resources = await _reservationService.GetAllResourcesAsync();
            var venues = resources.Where(r => IsVenueType(r.Type)).ToList();

            var model = venues.Select(r => new ResourceOverviewViewModel
            {
                Name = r.Name,
                Type = GetResourceTypeDisplay(r.Type),
                Capacity = r.Capacity,
                Location = r.Location ?? "Club venue",
                Status = r.Reservations.Any(res =>
                    res.StartTime.AddDays(-1) <= DateTime.Now &&
                    res.EndTime.AddDays(1) >= DateTime.Now)
                    ? "Reserved"
                    : "Available",
                IsActive = r.IsActive,
                ImageUrl = r.ImageUrl
            }).ToList();

            return View(model);
        }

        // Separate view for equipment/art pieces
        public async Task<IActionResult> Equipment()
        {
            var resources = await _reservationService.GetAllResourcesAsync();
            var equipment = resources.Where(r => !IsVenueType(r.Type)).ToList();

            var model = equipment.Select(r => new ResourceOverviewViewModel
            {
                Name = r.Name,
                Type = GetResourceTypeDisplay(r.Type),
                Capacity = r.Capacity,
                Location = r.Location ?? "Club equipment",
                Status = r.Reservations.Any(res =>
                    res.StartTime.AddDays(-1) <= DateTime.Now &&
                    res.EndTime.AddDays(1) >= DateTime.Now)
                    ? "Reserved"
                    : "Available",
                QuantityAvailable = r.QuantityAvailable,
                IsActive = r.IsActive,
                ImageUrl = r.ImageUrl
            }).ToList();

            return View(model);
        }

        public async Task<IActionResult> Details(string name)
        {
            var resource = await _reservationService.GetResourceByNameAsync(name);

            if (resource == null)
                return NotFound();

            return View(resource);
        }

        public IActionResult Create()
        {
            return View(new ResourceCreateViewModel { ResourceTypes = GetResourceTypeSelectList() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResourceCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ResourceTypes = GetResourceTypeSelectList();
                return View(model);
            }

            var resource = new Resource
            {
                Name = model.Name,
                Description = model.Type,
                Capacity = IsVenueType((ResourceType)model.ResourceTypeId) ? model.Capacity : null,
                BasePrice = 0,
                Type = (ResourceType)model.ResourceTypeId,
                QuantityAvailable = model.QuantityAvailable > 0 ? model.QuantityAvailable : 1,
                Location = model.Location,
                IsAffiliatedVenue = model.IsAffiliatedVenue,
                ImageUrl = model.ImageUrl,
                IsActive = true
            };

            await _reservationService.CreateResourceAsync(resource);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string name)
        {
            var resource = await _reservationService.GetResourceByNameAsync(name);

            if (resource == null)
                return NotFound();

            var viewModel = new ResourceCreateViewModel
            {
                Name = resource.Name,
                Type = resource.Description,
                Capacity = resource.Capacity ?? 0,
                ResourceTypeId = (int)resource.Type,
                QuantityAvailable = resource.QuantityAvailable,
                Location = resource.Location,
                IsAffiliatedVenue = resource.IsAffiliatedVenue,
                ImageUrl = resource.ImageUrl,
                ResourceTypes = GetResourceTypeSelectList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string originalName, ResourceCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ResourceTypes = GetResourceTypeSelectList();
                return View(model);
            }

            var resource = await _reservationService.GetResourceByNameAsync(originalName);
            if (resource == null)
                return NotFound();

            resource.Name = model.Name;
            resource.Description = model.Type;
            resource.Capacity = IsVenueType((ResourceType)model.ResourceTypeId) ? model.Capacity : null;
            resource.Type = (ResourceType)model.ResourceTypeId;
            resource.QuantityAvailable = model.QuantityAvailable > 0 ? model.QuantityAvailable : 1;
            resource.Location = model.Location;
            resource.IsAffiliatedVenue = model.IsAffiliatedVenue;
            resource.ImageUrl = model.ImageUrl;

            var success = await _reservationService.UpdateResourceAsync(originalName, resource);

            if (!success)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string name)
        {
            var resource = await _reservationService.GetResourceByNameAsync(name);

            if (resource == null)
                return NotFound();

            return View(resource);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string name)
        {
            var success = await _reservationService.DeleteResourceAsync(name);

            if (!success)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Calendar()
        {
            var reservations = await _reservationService.GetReservationCalendarAsync();

            return View(reservations);
        }

        // Helper methods

        private bool IsVenueType(ResourceType type)
        {
            return type == ResourceType.ConferenceRoom 
                || type == ResourceType.ExhibitionHall 
                || type == ResourceType.OutdoorLocation 
                || type == ResourceType.AffiliatedVenue;
        }

        private string GetResourceTypeDisplay(ResourceType type)
        {
            return type switch
            {
                // Venues
                ResourceType.ConferenceRoom => "Conference Room",
                ResourceType.ExhibitionHall => "Exhibition Hall",
                ResourceType.OutdoorLocation => "Outdoor Location",
                ResourceType.AffiliatedVenue => "Affiliated Venue",

                // Equipment & Supplies
                ResourceType.ArtEquipment => "Art Equipment",
                ResourceType.AudioVisualEquipment => "Audio-Visual Equipment",
                ResourceType.Furniture => "Furniture",
                ResourceType.PhotographyEquipment => "Photography Equipment",
                ResourceType.DecorationMaterials => "Decoration Materials",
                ResourceType.ArtPiece => "Art Piece",
                ResourceType.Other => "Other",

                _ => "Unknown"
            };
        }

        private List<SelectListItem> GetResourceTypeSelectList()
        {
            return new List<SelectListItem>
            {
                // Venues group
                new SelectListItem { Value = "0", Text = "🏢 Conference Room (Venue)" },
                new SelectListItem { Value = "1", Text = "🖼️ Exhibition Hall (Venue)" },
                new SelectListItem { Value = "10", Text = "🌳 Outdoor Location (Venue)" },
                new SelectListItem { Value = "11", Text = "📍 Affiliated Venue (External)" },

                // Equipment & Supplies group
                new SelectListItem { Value = "2", Text = "🎨 Art Equipment (Multiple units)" },
                new SelectListItem { Value = "3", Text = "🎬 Audio-Visual Equipment (Multiple units)" },
                new SelectListItem { Value = "4", Text = "🪑 Furniture (Multiple units)" },
                new SelectListItem { Value = "5", Text = "📷 Photography Equipment (Multiple units)" },
                new SelectListItem { Value = "6", Text = "✨ Decoration Materials (Multiple units)" },
                new SelectListItem { Value = "7", Text = "🎭 Art Piece (Multiple units)" },
                new SelectListItem { Value = "99", Text = "📦 Other (Multiple units)" }
            };
        }
    }
}