using BugTracker.Data;
using BugTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Services
{
    public class BTTicketsService : IBTTicketsService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IHttpContextAccessor _httpContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public BTTicketsService(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<BTUser> userManager, IHttpContextAccessor httpContext)
        {
            _context = context;
            _userManager = userManager;
            _httpContext = httpContext;
            _roleManager = roleManager;
        }

        //public async Task AddUserToTicket(string userId, int ticketId)
        //{
        //    if (!await IsUserOnTicket(userId, ticketId))
        //    {
        //        try
        //        {
        //            await _context.TicketUsers.AddAsync(new TicketUser { TicketId = ticketId, UserId = userId });
        //            await _context.SaveChangesAsync();
        //            //Project project = _context.Projects.Find(projectId);
        //            //BTUser user = _context.Users.Find(userId);

        //            //ProjectUser projectUser = new ProjectUser()
        //            //{
        //            //    ProjectId = project.Id,
        //            //    UserId = user.Id
        //            //};
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.WriteLine($"Error adding user to project. --> {ex.Message}");
        //            throw;
        //        }
        //    }
        //}

        //public async Task<bool> IsUserOnTicket(string userId, int ticketId)
        //{
        //    //Project project = await _context.Projects
        //    //    .Include(u => u.ProjectUsers.Where(u => u.UserId == userId))
        //    //    .ThenInclude(u => u.User)
        //    //    .FirstOrDefaultAsync(u => u.Id == projectId);
        //    //bool result = project.ProjectUsers.Any(u => u.UserId == userId);
        //    //return result;

        //    return await _context.TicketUsers.Where(tu => tu.UserId == userId && tu.TicketId == ticketId).AnyAsync();
        //}
        //public async Task<ICollection<Ticket>> ListUserTickets(string userId)
        //{
        //    BTUser user = await _context.Users
        //        .Include(t => t.TicketUsers)
        //        .ThenInclude(t => t.Ticket)
        //        .FirstOrDefaultAsync(t => t.Id == userId);

        //    List<Ticket> tickets = user.TicketUsers.SelectMany(t => (IEnumerable<Ticket>)t.Ticket).ToList();
        //    return tickets;
        //}

        //public async Task RemoveUserFromTicket(string userId, int ticketId)
        //{
        //    if (await IsUserOnTicket(userId, ticketId))
        //    {
        //        try
        //        {
        //            TicketUser ticketUser = _context.TicketUsers.Where(m => m.UserId == userId && m.TicketId == ticketId).FirstOrDefault();
        //            _context.TicketUsers.Remove(ticketUser);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.WriteLine($"Error removing user from project. --> {ex.Message}");
        //            throw;
        //        }
        //    }
        //}

        //public async Task<ICollection<BTUser>> UsersNotOnTicket(int ticketId)
        //{
        //    return await _context.Users.Where(u => IsUserOnTicket(u.Id, ticketId).Result == false).ToListAsync();
        //}

        //public async Task<ICollection<BTUser>> UsersOnTicket(int ticketId)
        //{
        //    Ticket ticket = await _context.Tickets
        //        .Include(u => u.TicketUsers)
        //        .ThenInclude(u => u.User)
        //        .FirstOrDefaultAsync(u => u.Id == ticketId);

        //    List<BTUser> ticketUsers = ticket.TicketUsers.Select(t => t.User).ToList();
        //    return ticketUsers;
        //}

        public async Task<int> GetTicketCount(int projectId)
        {
            return await _context.Tickets.Where(t => t.ProjectId == projectId).CountAsync();
        }

        public async Task<int> GetAttachmentCount(int ticketId)
        {
            return await _context.TicketAttachments.Where(a => a.TicketId == ticketId).CountAsync();
        }

        public async Task<int> GetCommentCount(int ticketId)
        {
            var comments = await _context.TicketComments.Where(c => c.TicketId == ticketId).ToListAsync();
            var subcomments = new List<TicketSubComment>();
            foreach (var comment in comments)
            {
                subcomments = await _context.TicketSubComments.Where(sc => sc.TicketCommentId == comment.Id).ToListAsync();
            }
            var total = comments.Count + subcomments.Count;
            return total;
        }

        public async Task<int> GetUserTicketCount(string userId)
        {
            return await _context.Tickets.Where(t => t.DeveloperUserId == userId).CountAsync();
        }

        public async Task<List<Ticket>> All()
        {

            var model = await _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .ToListAsync();
            return model;
        }
        public async Task<List<Ticket>> AssignedProjectTickets(string userId)
        {
            var projectId = new List<int>();
            var model = new List<Ticket>();
            var userProjects = _context.ProjectUsers.Where(pu => pu.UserId == userId);
            foreach (var project in userProjects)
            {
                projectId.Add(project.ProjectId);
            }
            foreach (var id in projectId)
            {
                var tickets = await _context.Tickets
                    .Where(t => t.ProjectId == id)
                    .Include(t => t.DeveloperUser)
                    .Include(t => t.OwnerUser)
                    .Include(t => t.Project)
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType)
                    .ToListAsync();
                model.AddRange(tickets);
            }
            return model;
        }
        public async Task<List<Ticket>> AssignedTickets(string userId)
        {
            var model = new List<Ticket>();
            var tickets = await _context.Tickets
                .Where(t => t.DeveloperUserId == userId)
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .ToListAsync();

            model.AddRange(tickets);
            var modelList = model.Distinct().ToList();
            return modelList;

        }
        public async Task<List<Ticket>> OwnedTickets(string userId)
        {
            var model = new List<Ticket>();
            var tickets = await _context.Tickets
                .Where(t => t.OwnerUserId == userId)
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .ToListAsync();
            model.AddRange(tickets);
            return model;
        }

        public Ticket Ticket()
        {            
            var ticket = new Ticket();
            return ticket;
        }

        public SelectList GetProjectSelect()
        {
            var projects = new SelectList(_context.Projects, "Id", "Name");
            return projects;
        }

        public SelectList GetTicketTypeSelect()
        {
            var tiktypes = new SelectList(_context.TicketTypes, "Id", "Name");
            return tiktypes;
        }

        public async Task<bool> OwnsTicket(int ticketId)
        {
            var userLogged = _httpContext.HttpContext.User;
            var userId = _userManager.GetUserId(userLogged);
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId && t.OwnerUserId == userId);

            bool result = false;
            if (ticket != null)
            {
                result = true;
            }

            return result;
        }

        public async Task<bool> OwnsAttachment(int attachmentId)
        {
            var userLogged = _httpContext.HttpContext.User;
            var userId = _userManager.GetUserId(userLogged);
            var attachment = await _context.TicketAttachments.FirstOrDefaultAsync(a => a.Id == attachmentId && a.UserId == userId);

            bool result = false;
            if (attachment != null)
            {
                result = true;
            }

            return result;
        }

        public async Task<bool> OwnsComment(int commentId)
        {
            var userLogged = _httpContext.HttpContext.User;
            var userId = _userManager.GetUserId(userLogged);
            var comment = await _context.TicketComments.FirstOrDefaultAsync(c => c.Id == commentId && c.AuthorId == userId);

            bool result = false;
            if (comment != null)
            {
                result = true;
            }

            return result;
        }

        public async Task<bool> OwnsSub(int subCommentId)
        {
            var userLogged = _httpContext.HttpContext.User;
            var userId = _userManager.GetUserId(userLogged);
            var sub = await _context.TicketSubComments.FirstOrDefaultAsync(c => c.Id == subCommentId && c.AuthorId == userId);

            bool result = false;
            if (sub != null)
            {
                result = true;
            }

            return result;
        }
    }
}
