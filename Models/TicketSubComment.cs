using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class TicketSubComment
    {
        #region Keys
        public int Id { get; set; }
        public int TicketCommentId { get; set; }
        public string AuthorId { get; set; }
        #endregion

        #region Comment Properties
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        #endregion


        #region Navigation
        public virtual BTUser Author { get; set; }
        public virtual TicketComment TicketComment { get; set; }
        #endregion
    }
}
