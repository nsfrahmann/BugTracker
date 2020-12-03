using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTracker.Data;
using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using BugTracker.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace BugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTHistoriesService _historiesService;
        private readonly IBTAttachmentsService _attachmentsService;
        private readonly IBTTicketsService _ticketsService;
        private readonly IBTProjectsService _projectsService;
        private readonly IBTRolesService _rolesService;
        private readonly UserManager<BTUser> _userManager;

        public TicketsController(ApplicationDbContext context,
            IBTHistoriesService historiesService,
            IBTAttachmentsService attachmentsService,
            IBTTicketsService ticketsService,
            IBTProjectsService projectsService,
            IBTRolesService rolesService,
            UserManager<BTUser> userManager)
        {
            _context = context;
            _historiesService = historiesService;
            _attachmentsService = attachmentsService;
            _ticketsService = ticketsService;
            _projectsService = projectsService;
            _rolesService = rolesService;
            _userManager = userManager;
        }

        // GET: Tickets
        [Authorize]
        public async Task<IActionResult> Index(string all, string assignedProjectTickets, string assignedTickets, string ownedTickets)
        {

            var userId = _userManager.GetUserId(User);
            if (assignedProjectTickets == "From My Projects")
            {
                ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name");
                ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name");
                ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name");
                ViewData["CurrentFilter"] = "From My Projects";
                return View (await _ticketsService.AssignedProjectTickets(userId));
            }
            if (assignedTickets == "Tickets Assigned")
            {
                ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name");
                ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name");
                ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name");
                ViewData["CurrentFilter"] = "Tickets Assigned";
                return View(await _ticketsService.AssignedTickets(userId));
            }
            if (ownedTickets == "My Submits")
            {
                ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name");
                ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name");
                ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name");
                ViewData["CurrentFilter"] = "My Submits";
                return View(await _ticketsService.OwnedTickets(userId));
            }
            if (all == "All Tickets")
            {
                ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name");
                ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name");
                ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name");
                ViewData["CurrentFilter"] = "All Tickets";
                return View(await _ticketsService.All());
            }

            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name");
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name");
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name");
            ViewData["CurrentFilter"] = "All Tickets";
            return View(await _ticketsService.All());
        }

        public IActionResult Analytics()
        {
            return View();
        }        

        [Authorize]
        public async Task<IActionResult> History(int? id)
        {
            var ticketId = id;
            if (id == null)
            {
                return NotFound();
            }
            var ticket = await _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.UsersOnProject)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.Attachments)
                .Include(t => t.Histories)
                .ThenInclude(h => h.User)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .FirstOrDefaultAsync(m => m.Id == id);

            ticket.UsersOnProject = await _projectsService.UsersOnProject(ticket.ProjectId);
            var ticketDev = new List<SelectListItem>();
            foreach (var user in (await _projectsService.UsersOnProject(ticket.ProjectId)))
            {
                if (!await _userManager.IsInRoleAsync(user, "Demo"))
                {
                    ticketDev.Add(new SelectListItem() { Text = user.FullName, Value = user.Id });
                }
            }

            return View(ticket);

        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,ImageData,ProjectId,TicketPriorityId,TicketStatusId,TicketTypeId")] Ticket ticket, List<IFormFile> FormFile)
        {
                bool foo = Convert.ToBoolean(TempData["FromPDetails"]);
                if (ModelState.IsValid)
                {
                    ticket.OwnerUser = await _userManager.GetUserAsync(User);
                    ticket.OwnerUserId = ticket.OwnerUser.Id;
                    ticket.Created = DateTime.Now;

                    _context.Add(ticket);
                    await _context.SaveChangesAsync();

                    if (FormFile != null)
                    {
                        
                        foreach (var file in FormFile)
                        {
                            var attachment = new TicketAttachment
                            {
                                TicketId = ticket.Id,
                                UserId = _userManager.GetUserId(User),
                                Created = DateTime.Now,
                                ContentType = file.ContentType,
                                FileData = await _attachmentsService.EncodeAttachmentAsync(file),
                                FileName = file.FileName,
                            };
                            await _context.AddAsync(attachment);
                        }
                    }
                    await _context.SaveChangesAsync();
         

                    if (foo == false)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        var myId = ticket.ProjectId;
                        return RedirectToAction("Details", "Projects", new { Id = myId });
                    }
                }
                return View(ticket);
        }


        // GET: Tickets/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {         
            if (id == null)
            {
                return NotFound();
            }

                var ticket = await _context.Tickets.FindAsync(id);
                if (ticket == null)
                {
                    return NotFound();
                }
                ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.DeveloperUserId);
                ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.OwnerUserId);
                ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
                ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
                ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
                ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
                return View(ticket);

        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ImageData,Created,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,DeveloperUserId")] Ticket ticket, IFormFile NewFormFile, string selectedUsers)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

                if (ModelState.IsValid)
                {
                    try
                    {
                        Ticket oldTicket = await _context.Tickets
                            .Include(t => t.DeveloperUser)
                            .Include(t => t.TicketPriority)
                            .Include(t => t.TicketStatus)
                            .Include(t => t.TicketType)
                            .Include(t => t.TicketComments)
                            .AsNoTracking().FirstOrDefaultAsync(t => t.Id == ticket.Id);

                        if (selectedUsers != null)
                        {
                            ticket.DeveloperUserId = selectedUsers;
                        }
                        if (NewFormFile != null)
                        {
                            var attachment = new TicketAttachment
                            {
                                TicketId = ticket.Id,
                                UserId = _userManager.GetUserId(User),
                                Created = DateTime.Now,
                                ContentType = NewFormFile.ContentType,
                                FileData = await _attachmentsService.EncodeAttachmentAsync(NewFormFile),
                                FileName = NewFormFile.FileName,
                            };
                            _context.Add(attachment);
                            await _context.SaveChangesAsync();
                        }
                        string userId = _userManager.GetUserId(User);
                        ticket.Updated = DateTime.Now;
                        _context.Update(ticket);
                        await _context.SaveChangesAsync();
                        Ticket newTicket = await _context.Tickets
                            .Include(t => t.DeveloperUser)
                            .Include(t => t.TicketPriority)
                            .Include(t => t.TicketStatus)
                            .Include(t => t.TicketType)
                            .Include(t => t.TicketComments)
                            .AsNoTracking().FirstOrDefaultAsync(t => t.Id == ticket.Id);
                        await _historiesService.AddHistory(oldTicket, newTicket, userId);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TicketExists(ticket.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    var myId = ticket.Id;
                    return RedirectToAction("Details", "Tickets", new { Id = myId });
                }
            ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.DeveloperUserId);
            ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.OwnerUserId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Id", ticket.TicketPriorityId);
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticket.TicketStatusId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Id", ticket.TicketTypeId);
            return RedirectToAction("Index", "Tickets");
        }


        // POST: Tickets/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }

        [Authorize]
        [HttpPost, ActionName("AttachmentDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AttachmentDeleteConfirmed(int id)
        {
            var ticketAttachment = await _context.TicketAttachments.FindAsync(id);
            _context.TicketAttachments.Remove(ticketAttachment);
            await _context.SaveChangesAsync();
            var myId = ticketAttachment.TicketId;
            return RedirectToAction("Details", "Tickets", new { Id = myId });
        }

        [Authorize]
        [HttpPost, ActionName("NotificationDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NotificationDeleteConfirmed(List<int> ids)
        {
            var userId = _userManager.GetUserId(User);
            var userNotifications = await _context.Notifications.Where(n => n.RecipientId == userId).ToListAsync();

            foreach (var notification in userNotifications)
            {
                _context.Notifications.Remove(notification);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Home", "Dashboard");
        }

        public async Task<FileResult> DownloadFile(int? id)
        {
            if (id == null)
            {
                return null;
            }
            TicketAttachment attachment = await _context.TicketAttachments.FindAsync(id);
            if (attachment == null)
            {
                return null;
            }
            return File(attachment.FileData, attachment.ContentType);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id, int? tikId, bool click)
        {
            var ticketId = id;
            if (id == null)
            {
                return NotFound();
            }
            var ticket = await _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.UsersOnProject)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.Attachments)
                .Include(t => t.Histories)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .Include(t => t.TicketComments)
                .ThenInclude(tc => tc.Author)
                .Include(t => t.TicketComments)
                .ThenInclude(tc => tc.TicketSubComments)
                .ThenInclude(tsc => tsc.Author)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (click && tikId != null)
            {
                var relatedNotifications = await _context.Notifications.Where(n => n.TicketId == tikId).ToListAsync();
                foreach (var notification in relatedNotifications)
                {
                    _context.Remove(notification);
                }
                await _context.SaveChangesAsync();
            }
            ticket.UsersOnProject = await _projectsService.UsersOnProject(ticket.ProjectId);
            var ticketDev = new List<SelectListItem>();
            foreach (var user in (await _projectsService.UsersOnProject(ticket.ProjectId)))
            {
                if (!await _userManager.IsInRoleAsync(user, "Demo"))
                {
                    ticketDev.Add(new SelectListItem() { Text = user.FullName, Value = user.Id });
                }
            }
            ViewData["TicketDevs"] = new SelectList(ticketDev, "Value", "Text");
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name", ticket.TicketStatusId);

            return View(ticket);

        }
    }
}