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
                Location = r.Location ?? (IsVenueType(r.Type) ? "Hall" : "Equipment"),
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
            var equipment = resources.Where(r => r.Type == ResourceType.Equipment || r.Type == ResourceType.ArtPiece).ToList();

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
                BasePrice = model.BasePrice,
                Type = (ResourceType)model.ResourceTypeId,
                QuantityAvailable = model.QuantityAvailable > 0 ? model.QuantityAvailable : 1,
                Location = model.Location,
                IsAffiliatedVenue = model.IsAffiliatedVenue,
                ImageUrl = model.ImageUrl,
                IsActive = model.IsActive
            };

            await _reservationService.CreateResourceAsync(resource);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RefreshTypes()
        {
            var resources = await _reservationService.GetAllResourcesAsync();

            foreach (var resource in resources)
            {
                if (resource.Type == ResourceType.Other && !string.IsNullOrWhiteSpace(resource.Description))
                {
                    var description = resource.Description.ToLowerInvariant();
                    if (description.Contains("hall") || description.Contains("sala") || description.Contains("room"))
                    {
                        resource.Type = ResourceType.Hall;
                    }
                    else if (description.Contains("equipment") || description.Contains("proiector") || description.Contains("audio"))
                    {
                        resource.Type = ResourceType.Equipment;
                    }
                }
            }

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
                BasePrice = resource.BasePrice,
                IsActive = resource.IsActive,
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
            resource.BasePrice = model.BasePrice;
            resource.IsActive = model.IsActive;

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
            return type == ResourceType.Hall;
        }

        private string GetResourceTypeDisplay(ResourceType type)
        {
            return type switch
            {
                ResourceType.Hall => "Hall",
                ResourceType.Equipment => "Equipment",
                ResourceType.ArtPiece => "Art Piece",
                ResourceType.Other => "Other",
                _ => "Unknown"
            };
        }

        private List<SelectListItem> GetResourceTypeSelectList()
        {
            return new List<SelectListItem>
        {
            new SelectListItem { Value = ((int)ResourceType.Hall).ToString(), Text = "🏢 Hall" },
            new SelectListItem { Value = ((int)ResourceType.Equipment).ToString(), Text = "🎨 Equipment" },
            new SelectListItem { Value = ((int)ResourceType.ArtPiece).ToString(), Text = "🖼️ Art Piece" },
            new SelectListItem { Value = ((int)ResourceType.Other).ToString(), Text = "📦 Other" }
            };
        }
    }
}