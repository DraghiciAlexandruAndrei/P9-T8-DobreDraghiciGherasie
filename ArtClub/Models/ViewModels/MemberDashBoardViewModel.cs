using System;
using System.Collections.Generic;

namespace ArtClub.Models.ViewModels
{
    public class MemberDashboardViewModel
    {
        public required string UserName { get; set; }
        public bool IsMembershipActive { get; set; }
        public int EventsOrganizedCount { get; set; }
        public int PendingInvitationsCount { get; set; }
        public int RemainingEventLimit { get; set; }
        public List<RecentInvitationItem> RecentInvitations { get; set; } = new();
        public List<UpcomingEventItem> UpcomingEvents { get; set; } = new();
    }

    public class RecentInvitationItem
    {
        public required string EventTitle { get; set; }
        public required string OrganizerName { get; set; }
        public int InvitationId { get; set; }
    }

    public class UpcomingEventItem
    {
        public required string Title { get; set; }
        public required string ResourceName { get; set; }
        public DateTime StartDate { get; set; }
        public int EventId { get; set; }
    }
}