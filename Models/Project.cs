using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using BugTracker.Models.ViewModels;

namespace BugTracker.Models
{
    public class Project
    {
        public Project()
        {
            Tickets = new HashSet<Ticket>();

        }
        public int Id { get; set; }
        public string OwnerUserId { get; set; }
        

        [Required]
        [StringLength(50)]
        [Display(Name = "Project Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "Project Summary")]
        public string Summary { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public List<string> ProjectUserIds { get; set; }
        public virtual BTUser OwnerUser { get; set; }
        public virtual List<ProjectUser> ProjectUsers { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
