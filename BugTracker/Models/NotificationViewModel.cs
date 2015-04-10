using System.Collections.Generic;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class NotificationViewModel
    {
        public int Count { get; set; }
        public List<string> Notifications { get; set; }
        public List<int> TicketId { get; set; }
    }
}