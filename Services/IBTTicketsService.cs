using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugTracker.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugTracker.Services
{
    public interface IBTTicketsService
    {
        //public Task<bool> IsUserOnTicket(string userId, int ticketId);
        //public Task<ICollection<Ticket>> ListUserTickets(string userId);
        //public Task AddUserToTicket(string userId, int ticketId);
        //public Task RemoveUserFromTicket(string userId, int ticketId);
        //public Task<ICollection<BTUser>> UsersOnTicket(int ticketId);
        //public Task<ICollection<BTUser>> UsersNotOnTicket(int ticketId);
        public Task<int> GetTicketCount(int projectId);
        public Task<int> GetAttachmentCount(int ticketId);
        public Task<int> GetCommentCount(int ticketId);
        public Task<int> GetUserTicketCount(string userId);
        public Task<List<Ticket>> All();
        public Task<List<Ticket>> AssignedProjectTickets(string userId);
        public Task<List<Ticket>> AssignedTickets(string userId);
        public Task<List<Ticket>> OwnedTickets(string userId);
        public Ticket Ticket();
        public SelectList GetProjectSelect();
        public SelectList GetTicketTypeSelect();
        public Task<bool> OwnsTicket(int ticketId);
        public Task<bool> OwnsAttachment(int attachmentId);
        public Task<bool> OwnsComment(int commentId);
        public Task<bool> OwnsSub(int subCommentId);
    }
}
