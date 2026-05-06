namespace ArtClub.Models.Enums
{
    /// <summary>
    /// Represents the type of resource available in the art club.
    /// Different types have different usage rules and pricing models.
    /// Organized into two categories: VENUES (singular, booked per time slot)
    /// and EQUIPMENT (can have multiple units available).
    /// </summary>
    public enum ResourceType
    {
        // VENUES - Physical spaces (typically one per type, booked per time slot)

        /// <summary>Conference room - for meetings, lectures, workshops</summary>
        ConferenceRoom = 0,

        /// <summary>Exhibition hall - for art exhibitions and displays</summary>
        ExhibitionHall = 1,

        /// <summary>Outdoor location - garden, patio, exterior space</summary>
        OutdoorLocation = 10,

        /// <summary>Affiliated external venue - partner location for events</summary>
        AffiliatedVenue = 11,

        // EQUIPMENT & SUPPLIES - Items that can have multiple units

        /// <summary>Art equipment - easels, canvases, brushes, paint, etc.</summary>
        ArtEquipment = 2,

        /// <summary>Audio-visual equipment - projectors, speakers, microphones, screens</summary>
        AudioVisualEquipment = 3,

        /// <summary>Furniture - tables, chairs, stands, display racks</summary>
        Furniture = 4,

        /// <summary>Photography equipment - cameras, tripods, lighting, reflectors</summary>
        PhotographyEquipment = 5,

        /// <summary>Decoration materials - banners, lights, backdrop, drapes</summary>
        DecorationMaterials = 6,

        /// <summary>Art pieces - paintings, sculptures, installations available for display</summary>
        ArtPiece = 7,

        /// <summary>Miscellaneous supplies and equipment</summary>
        Other = 99
    }
}
