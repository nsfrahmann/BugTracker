using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class TicketComment
    {
        #region Keys
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string AuthorId { get; set; }
        #endregion

        #region Comment Properties
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public virtual ICollection<TicketSubComment> TicketSubComments { get; set; }
        #endregion

        public TicketComment()
        {
            TicketSubComments = new HashSet<TicketSubComment>();
        }

        #region Navigation
        public virtual Ticket Ticket { get; set; }
        public virtual BTUser Author { get; set; }
        #endregion
    }
}
