using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class TicketHistory
    {
        //public int Id { get; set; }
        //public int TicketId { get; set; }
        //public string Property { get; set; }
        //public string OldValue { get; set; }
        //public string NewValue { get; set; }
        //public DateTimeOffset Created { get; set; }
        //public string UserId { get; set; }
        //public virtual Ticket Ticket { get; set; }
        //public virtual BTUser User { get; set; }
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string DeveloperAssigned { get; set; }
        public string OldDeveloperValue { get; set; }
        public string NewDeveloperValue { get; set; }
        public string TicketTitle { get; set; }
        public string OldTitleValue { get; set; }
        public string NewTitleValue { get; set; }
        public string TicketDescription { get; set; }
        public string OldDescriptionValue { get; set; }
        public string NewDescriptionValue { get; set; }
        public string TicketType { get; set; }
        public string OldTypeValue { get; set; }
        public string NewTypeValue { get; set; }
        public string TicketPriority { get; set; }
        public string OldPriorityValue { get; set; }
        public string NewPriorityValue { get; set; }
        public string TicketStatus { get; set; }
        public string OldStatusValue { get; set; }
        public string NewStatusValue { get; set; }
        public DateTimeOffset Created { get; set; }
        public string UserId { get; set; }
        public virtual Ticket Ticket { get; set; }
        public virtual BTUser User { get; set; }
    }
}
