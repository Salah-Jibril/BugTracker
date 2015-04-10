namespace BugTracker.Models
{
    using System;
    using System.Collections.Generic;

    public partial class TicketNotification
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }
        public string Notice { get; set; }
        public bool Read { get; set; }

        
        public virtual Ticket Ticket { get; set; }
    }
}