using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models.ViewModels
{
    public class TicketsDetailsViewModel
    {
        public Ticket Ticket { get; set; }
        public Project Project { get; set; }
        public List<BTUser> ProjectUsers { get; set; }
        public MultiSelectList Users { get; set; }
        public string[] SelectedUsers { get; set; }
    }
}
