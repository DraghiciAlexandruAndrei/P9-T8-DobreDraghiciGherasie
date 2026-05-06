using ArtClub.Models.Entities;
using ArtClub.Models.Enums;
using ArtClub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ArtClub.Controllers
{
    [Authorize]
    public class InvitationsController : Controller
    {
        private readonly IInvitationService _invitationService;
        private readonly IEventService _eventService;
        private readonly UserManager<User> _userManager;

        public InvitationsController(IInvitationService invitationService, IEventService eventService, UserManager<User> userManager)
        {
            _invitationService = invitationService;
            _eventService = eventService;
            _userManager = userManager;
        }

        // ==========================================
        // LIST RECEIVED INVITATIONS (INBOX)
        // ==========================================
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            // Since User.Id is now string from IdentityUser, we pass user.Id directly
            var invitations = await _invitationService.GetUserInboxAsync(user.Id);
            return View(invitations ?? new List<Invitation>());
        }

        // ==========================================
        // INVITATION DETAILS
        // ==========================================
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return NotFound();

                return View();
            }
            catch
            {
                return NotFound();
            }
        }

        // ==========================================
        // ACCEPT INVITATION
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(int id)
        {
            var result = await _invitationService.AcceptInvitationAsync(id);
            if (result)
            {
                TempData["StatusMessage"] = "Invitație acceptată cu succes.";
            }
            return RedirectToAction(nameof(Index));
        }

        // ==========================================
        // DECLINE INVITATION
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Decline(int id)
        {
            var result = await _invitationService.DeclineInvitationAsync(id);
            if (result)
            {
                TempData["StatusMessage"] = "Invitație refuzată.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
