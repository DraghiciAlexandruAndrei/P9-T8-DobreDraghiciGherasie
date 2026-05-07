using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArtClub.Controllers
{
    public class BaseController : Controller
    {
        protected int? GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null && int.TryParse(claim.Value, out var id))
                return id;
            return HttpContext.Session.GetInt32("UserId");
        }

        protected string? GetCurrentUserRole()
        {
            return User.FindFirst("Role")?.Value
                ?? HttpContext.Session.GetString("UserRole");
        }

        protected bool IsAdmin() =>
            GetCurrentUserRole() == "Admin";
    }
}