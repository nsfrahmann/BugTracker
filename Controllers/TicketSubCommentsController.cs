using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTracker.Data;
using BugTracker.Models;

namespace BugTracker.Controllers
{
    public class TicketSubCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketSubCommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SubComments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TicketSubComments.Include(s => s.Author);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SubComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subComment = await _context.TicketSubComments
                .Include(s => s.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subComment == null)
            {
                return NotFound();
            }

            return View(subComment);
        }

        // GET: SubComments/Create
        //public IActionResult Create()
        //{
        //    ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id");
        //    ViewData["CommentId"] = new SelectList(_context.Comment, "Id", "Id");
        //    return View();
        //}

        // POST: SubComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // GET: SubComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subComment = await _context.TicketSubComments.FindAsync(id);
            if (subComment == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", subComment.AuthorId);
            ViewData["CommentId"] = new SelectList(_context.TicketComments, "Id", "Id", subComment.TicketCommentId);
            return View(subComment);
        }

        // POST: SubComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CommentId,TicketId,AuthorId,Content,Created")] TicketSubComment subComment)
        {
            if (id != subComment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", subComment.AuthorId);
            ViewData["CommentId"] = new SelectList(_context.TicketComments, "Id", "Id", subComment.TicketCommentId);
            return View(subComment);
        }

        // GET: SubComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subComment = await _context.TicketSubComments
                .Include(s => s.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subComment == null)
            {
                return NotFound();
            }

            return View(subComment);
        }

        // POST: SubComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subComment = await _context.TicketSubComments.FindAsync(id);
            _context.TicketSubComments.Remove(subComment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubCommentExists(int id)
        {
            return _context.TicketSubComments.Any(e => e.Id == id);
        }
    }
}
