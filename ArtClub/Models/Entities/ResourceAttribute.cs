namespace ArtClub.Models.Entities
{
    /// <summary>
    /// Represents an extensible attribute for a resource (e.g., equipment specifications).
    /// Supports category-specific technical specifications like projectors, WiFi availability, etc.
    /// Implements REQ-26: Support extensible resource attributes.
    /// </summary>
    public class ResourceAttribute
    {
        public int Id { get; set; }

        /// <summary>The resource this attribute belongs to</summary>
        public int ResourceId { get; set; }
        public virtual Resource Resource { get; set; }

        /// <summary>
        /// Name of the attribute (e.g., "Projector", "WiFi", "Capacity", "Audio System")
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value of the attribute (e.g., "4K", "Yes", "50 people", "Surround Sound")
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Optional description or notes about this attribute
        /// </summary>
        public string Description { get; set; }
    }
}
