using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugTracker.Data;
using BugTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    public class ChartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChartsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public JsonResult PriorityChart()
        {
            List<ChartData> result = new List<ChartData>();

            var ticketPriorities = _context.TicketPriorities.ToList();
            
            var pendingTicket = _context.Tickets.Where(t => t.TicketPriorityId == null).ToList();
            var pending = _context.Tickets.Where(t => t.TicketPriorityId == null).ToList().Count;
            var total = _context.Tickets.ToList().Count;

            if (pending != 0)
            {                
                foreach (var ticket in pendingTicket)
                {
                    result.Add(new ChartData
                    {
                        Name = "Pending Priority",
                        Count = pending,
                        Percentage = (double)pending / total * 100,
                        Color = "gold",
                    });
                }
            }

            foreach (var priority in ticketPriorities)
            {
                var prioritiesWith = _context.Tickets.Where(t => t.TicketPriorityId == priority.Id).ToList().Count;
                
                if (prioritiesWith != 0)
                {
                    result.Add(new ChartData
                    {
                        Name = priority.Name,
                        Count = prioritiesWith,
                        Percentage = (double)prioritiesWith / total * 100,
                        Color = "gold",
                    });
                }                
            }
            return Json(result);
        }

        public JsonResult TypeChart()
        {
            List<ChartData> result = new List<ChartData>();

            var ticketTypes = _context.TicketTypes.ToList();

            foreach (var type in ticketTypes)
            {
                var typesWith = _context.Tickets.Where(t => t.TicketTypeId == type.Id).ToList().Count;
                var typesWO = _context.Tickets.Where(t => t.TicketTypeId != type.Id).ToList().Count;
                var total = typesWO + typesWith;
                if (typesWith != 0)
                {
                    result.Add(new ChartData
                    {
                        Name = type.Name,
                        Count = typesWith,
                        Percentage = (double)typesWith / total * 100,
                        Color = "gold",
                    });
                }

            }
            return Json(result);
        }

        public JsonResult StatusChart()
        {
            List<ChartData> result = new List<ChartData>();

            var ticketStatuses = _context.TicketStatuses.ToList();

            var pendingTicket = _context.Tickets.Where(t => t.TicketStatusId == null).ToList();
            var pending = _context.Tickets.Where(t => t.TicketStatusId == null).ToList().Count;
            var total = _context.Tickets.ToList().Count;

            if (pending != 0)
            {
                foreach (var ticket in pendingTicket)
                {
                    result.Add(new ChartData
                    {
                        Name = "Pending Status",
                        Count = pending,
                        Percentage = (double)pending / total * 100,
                        Color = "gold",
                    });
                }
            }

            foreach (var status in ticketStatuses)
            {
                var statusWith = _context.Tickets.Where(t => t.TicketStatusId == status.Id).ToList().Count;
                if (statusWith != 0)
                {
                    result.Add(new ChartData
                    {
                        Name = status.Name,
                        Count = statusWith,
                        Percentage = (double)statusWith / total * 100,
                        Color = "gold",
                    });
                }

            }
            return Json(result);
        }
    }
}
