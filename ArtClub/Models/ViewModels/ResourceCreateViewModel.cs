using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArtClub.Models.ViewModels
{
    public class ResourceCreateViewModel
    {
        public string Name { get; set; }
        public string Type { get; set; }

        // Capacity for venues only
        public int? Capacity { get; set; }

        // Resource type selection
        public int ResourceTypeId { get; set; }

        // For equipment: how many units are available
        public int QuantityAvailable { get; set; } = 1;

        // Location/Address information (REQ-36)
        public string Location { get; set; }

        // Is this an affiliated external venue?
        public bool IsAffiliatedVenue { get; set; } = false;

        // Image/thumbnail URL
        public string ImageUrl { get; set; }

        // For dropdown selection in view
        public List<SelectListItem> ResourceTypes { get; set; } = new List<SelectListItem>();
    }
}