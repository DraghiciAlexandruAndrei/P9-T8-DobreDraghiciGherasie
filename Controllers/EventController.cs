using ArtClub.Models.Entities;
using ArtClub.Models.ViewModels;
using ArtClub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ArtClub.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IArtPieceService _artPieceService;

        public EventController(
            IEventService eventService,
            IArtPieceService artPieceService)
        {
            _eventService = eventService;
            _artPieceService = artPieceService;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _eventService.GetAllEventsAsync();

            var model = events.Select(e => new EventSummaryViewModel
            {
                Title = e.Title,
                OrganizerName = e.Organizer != null ? e.Organizer.UserName : "Unknown organizer",
                ResourceName = e.Resource != null ? e.Resource.Name : "No resource",
                Status = e.Reservation != null && e.Reservation.StartTime > DateTime.Now
                    ? "Scheduled"
                    : "Completed",
                StartDate = e.Reservation != null ? e.Reservation.StartTime : DateTime.Now,
                InviteCount = e.Invitations != null ? e.Invitations.Count : 0
            }).ToList();

            return View(model);
        }

        public IActionResult Create()
        {
            var start = DateTime.Now.AddDays(1);
            start = new DateTime(start.Year, start.Month, start.Day, 10, 0, 0);

            return View(new EventCreateViewModel
            {
                StartDate = start,
                EndDate = start.AddHours(2)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var organizerId = HttpContext.Session.GetInt32("UserId");

            if (organizerId == null)
            {
                ModelState.AddModelError("", "You must be logged in to create an event.");
                return View(model);
            }

            var resource = await _eventService.GetResourceByNameAsync(model.ResourceName);

            if (resource == null)
            {
                ModelState.AddModelError("ResourceName", "Resource not found.");
                return View(model);
            }

            var ev = new Event
            {
                Title = model.Title,
                Description = model.Description,
                ResourceId = resource.Id,
                OrganizerId = organizerId.Value,
                Budget = 0,

                Reservation = new Reservation
                {
                    ResourceId = resource.Id,
                    StartTime = model.StartDate,
                    EndTime = model.EndDate
                }
            };

            var success = await _eventService.CreateEventAsync(ev);

            if (!success)
            {
                ModelState.AddModelError("", "Event could not be created. The resource may be unavailable or the club may not have enough funds.");
                return View(model);
            }

            TempData["StatusMessage"] = "Event created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return NotFound();

            var ev = await _eventService.GetEventByTitleAsync(title);

            if (ev == null)
                return NotFound();

            var model = new EventDetailsViewModel
            {
                EventId = ev.Id,
                Title = ev.Title,
                OrganizerName = ev.Organizer != null ? ev.Organizer.UserName : "Unknown organizer",
                ResourceName = ev.Resource != null ? ev.Resource.Name : "No resource",
                Date = ev.Reservation != null ? ev.Reservation.StartTime : DateTime.Now,
                AttendingCount = ev.Invitations != null ? ev.Invitations.Count : 0,
                ArtPieceNames = new List<string>()
            };

            return View(model);
        }

        public async Task<IActionResult> Edit(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return NotFound();

            var ev = await _eventService.GetEventByTitleAsync(title);

            if (ev == null)
                return NotFound();

            ViewBag.OriginalTitle = ev.Title;

            var model = new EventCreateViewModel
            {
                Title = ev.Title,
                Description = ev.Description,
                StartDate = ev.Reservation != null ? ev.Reservation.StartTime : DateTime.Now,
                EndDate = ev.Reservation != null ? ev.Reservation.EndTime : DateTime.Now.AddHours(1),
                SelectedResourceId = ev.ResourceId,
                ResourceName = ev.Resource != null ? ev.Resource.Name : ""
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string originalTitle, EventCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.OriginalTitle = originalTitle;
                return View(model);
            }

            var resource = await _eventService.GetResourceByNameAsync(model.ResourceName);

            if (resource == null)
            {
                ViewBag.OriginalTitle = originalTitle;
                ModelState.AddModelError("ResourceName", "Resource not found.");
                return View(model);
            }

            var ev = new Event
            {
                Title = model.Title,
                Description = model.Description,
                ResourceId = resource.Id,

                Reservation = new Reservation
                {
                    ResourceId = resource.Id,
                    StartTime = model.StartDate,
                    EndTime = model.EndDate
                }
            };

            var success = await _eventService.UpdateEventAsync(originalTitle, ev);

            if (!success)
                return NotFound();

            TempData["StatusMessage"] = "Event updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return NotFound();

            var ev = await _eventService.GetEventByTitleAsync(title);

            if (ev == null)
                return NotFound();

            var model = new EventDetailsViewModel
            {
                EventId = ev.Id,
                Title = ev.Title,
                OrganizerName = ev.Organizer != null ? ev.Organizer.UserName : "Unknown organizer",
                ResourceName = ev.Resource != null ? ev.Resource.Name : "No resource",
                Date = ev.Reservation != null ? ev.Reservation.StartTime : DateTime.Now,
                AttendingCount = ev.Invitations != null ? ev.Invitations.Count : 0
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string title)
        {
            var success = await _eventService.DeleteEventByTitleAsync(title);

            if (!success)
                return NotFound();

            TempData["StatusMessage"] = "Event deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Inbox()
        {
            var model = new List<InvitationInboxViewModel>();
            return View(model);
        }

        public async Task<IActionResult> Cancel(int id)
        {
            await _eventService.CancelEventAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}