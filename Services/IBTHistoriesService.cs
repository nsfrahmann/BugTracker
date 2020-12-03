using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BugTracker.Models;

namespace BugTracker.Services
{
    public interface IBTHistoriesService
    {
        public Task AddHistory(Ticket oldTicket, Ticket newTicket, string userId);
        public Task<List<TicketHistory>> GetHistories(int ticketId);
    }
}
