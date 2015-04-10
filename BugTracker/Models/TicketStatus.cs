﻿namespace BugTracker.Models
{
    using System;
    using System.Collections.Generic;

    public partial class TicketStatus
    {
        public TicketStatus()
        {
            this.Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
