namespace ArtClub.Models.Enums
{
    /// <summary>
    /// Represents the type of resource available in the art club.
    /// Simplified categories for resource management.
    /// </summary>
    public enum ResourceType
    {
        /// <summary>Hall / venue (sala)</summary>
        Hall = 0,

        /// <summary>Equipment (projectors, audio, tools)</summary>
        Equipment = 1,

        /// <summary>Art piece (paintings, sculptures)</summary>
        ArtPiece = 2,

        /// <summary>Other resources</summary>
        Other = 3
    }
}
