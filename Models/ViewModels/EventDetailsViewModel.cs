using System;
using System.Collections.Generic;

namespace ArtClub.Models.ViewModels
{
    public class EventDetailsViewModel
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string OrganizerName { get; set; }
        public string ResourceName { get; set; }
        public DateTime Date { get; set; }
        public List<string> ArtPieceNames { get; set; } = new List<string>();
        public int AttendingCount { get; set; }
    }
}