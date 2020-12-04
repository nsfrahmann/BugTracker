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

namespace BugTracker.Controllers
{
    public class TicketCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTHistoriesService _historiesService;
        private readonly UserManager<BTUser> _userManager;

        public TicketCommentsController(ApplicationDbContext context, IBTHistoriesService historiesService, UserManager<BTUser> userManager)
        {
            _context = context;
            _historiesService = historiesService;
            _userManager = userManager;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TicketComments.Include(c => c.Author).Include(c => c.Ticket);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.TicketComments
                .Include(c => c.Author)
                .Include(c => c.Ticket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        //public IActionResult Create()
        //{
        //    ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id");
        //    ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
        //    ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description");
        //    return View();
        //}

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCom([Bind("TicketId,AuthorId")] TicketComment comment, string commentContent)
        {
            if (commentContent.ToCharArray().Length > 3000)
            {
                TempData["StopIt"] = "Either you're publishing a novel or trying to break the application. Either way, we don't have time for that. Sorry...";
                return RedirectToAction("Details", "Tickets", new { Id = comment.TicketId });
            }

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                comment.Created = DateTime.Now;
                comment.Content = commentContent;
                Ticket oldTicket = await _context.Tickets
                        .Include(t => t.DeveloperUser)
                        .Include(t => t.TicketPriority)
                        .Include(t => t.TicketStatus)
                        .Include(t => t.TicketType)
                        .Include(t => t.TicketComments)
                        .AsNoTracking().FirstOrDefaultAsync(t => t.Id == comment.TicketId);

                _context.Add(comment);

                Ticket newTicket = await _context.Tickets
                        .Include(t => t.DeveloperUser)
                        .Include(t => t.TicketPriority)
                        .Include(t => t.TicketStatus)
                        .Include(t => t.TicketType)
                        .Include(t => t.TicketComments)
                        .AsNoTracking().FirstOrDefaultAsync(t => t.Id == comment.TicketId);
                await _historiesService.AddHistory(oldTicket, newTicket, userId);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Tickets", new { Id = comment.TicketId });
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", comment.AuthorId);
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", comment.TicketId);
            return RedirectToAction("Details", "Tickets", new { Id = comment.TicketId });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSub([Bind("TicketId,AuthorId,Created")] TicketSubComment subComment, string subcommentContent, int foo)
        {
            if (subcommentContent.ToCharArray().Length > 3000)
            {
                TempData["StopIt"] = "Either you're publishing a novel or trying to break the application. Either way, we don't have time for that. Sorry...";
                return RedirectToAction("Details", "Tickets", new { Id = TempData["IdForSub"] });
            }

            if (ModelState.IsValid)
            {
                subComment.Content = subcommentContent;
                subComment.TicketCommentId = foo;
                var ticketId = (await _context.TicketComments.FindAsync(foo)).TicketId;

                _context.Add(subComment);

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Tickets", new { Id = ticketId });
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", subComment.AuthorId);
            ViewData["CommentId"] = new SelectList(_context.TicketComments, "Id", "Id", subComment.TicketCommentId);
            return View(subComment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCom(int id, [Bind("Id,TicketId,AuthorId,Content,Created")] TicketComment comment, string commentContent)
        {
            if (commentContent.ToCharArray().Length > 3000)
            {
                TempData["StopIt"] = "Either you're publishing a novel or trying to break the application. Either way, we don't have time for that. Sorry...";
                return RedirectToAction("Details", "Tickets", new { Id = comment.TicketId });
            }

            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    comment.Content = commentContent;
                    comment.Updated = DateTime.Now;
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Tickets", new { Id = comment.TicketId });
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", comment.AuthorId);
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", comment.TicketId);
            return RedirectToAction("Details", "Tickets", new { Id = comment.TicketId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSub(int id, [Bind("Id,CommentId,TicketId,AuthorId,Content,Created")] TicketSubComment subComment, string subcommentContent, int foo)
        {
            if (subcommentContent.ToCharArray().Length > 3000)
            {
                TempData["StopIt"] = "Either you're publishing a novel or trying to break the application. Either way, we don't have time for that. Sorry...";
                return RedirectToAction("Details", "Tickets", new { Id = TempData["IdForSub"] });
            }

            if (id != subComment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    subComment.Id = id;
                    subComment.Content = subcommentContent;
                    subComment.TicketCommentId = foo;
                    subComment.Updated = DateTime.Now;
                    _context.Update(subComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubCommentExists(subComment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var ticketId = (await _context.TicketComments.FindAsync(foo)).TicketId;
                return RedirectToAction("Details", "Tickets", new { Id = ticketId });
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", subComment.AuthorId);
            ViewData["CommentId"] = new SelectList(_context.TicketComments, "Id", "Id", subComment.TicketCommentId);
            return View(subComment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.TicketComments
                .Include(c => c.Author)
                .Include(c => c.Ticket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("DeleteCom")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComConfirmed(int id)
        {
            var comment = await _context.TicketComments.FindAsync(id);
            _context.TicketComments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Tickets", new { Id = comment.TicketId });
        }

        [HttpPost, ActionName("DeleteSub")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubConfirmed(int id, int foo)
        {
            var subComment = await _context.TicketSubComments.FindAsync(id);
            _context.TicketSubComments.Remove(subComment);
            await _context.SaveChangesAsync();
            var ticketId = (await _context.TicketComments.FindAsync(foo)).TicketId;
            return RedirectToAction("Details", "Tickets", new { Id = ticketId });
        }

        private bool CommentExists(int id)
        {
            return _context.TicketComments.Any(e => e.Id == id);
        }

        private bool SubCommentExists(int id)
        {
            return _context.TicketSubComments.Any(e => e.Id == id);
        }
    }
}
