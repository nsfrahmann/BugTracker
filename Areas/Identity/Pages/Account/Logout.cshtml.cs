using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using BugTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using BugTracker.Data;

namespace BugTracker.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<BTUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly UserManager<BTUser> _userManager;
        private readonly ApplicationDbContext _context;

        public LogoutModel(SignInManager<BTUser> signInManager, ILogger<LogoutModel> logger, UserManager<BTUser> userManager, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            var userId = _userManager.GetUserId(User);
            if (User.IsInRole("Demo User"))
            {
                List<Project> pr = _context.Projects.Where(pr => pr.OwnerUserId == userId).ToList();
                List<Ticket> t = _context.Tickets.Where(t => t.OwnerUserId == userId).ToList();
                List<TicketComment> c = _context.TicketComments.Where(c => c.AuthorId == userId).ToList();
                List<TicketSubComment> sc = _context.TicketSubComments.Where(sc => sc.AuthorId == userId).ToList();
                List<TicketHistory> h = _context.TicketHistories.Where(h => h.UserId == userId).ToList();
                List<TicketAttachment> a = _context.TicketAttachments.Where(a => a.UserId == userId).ToList();
                Profile pf = _context.Profiles.FirstOrDefault(pf => pf.UserId == userId);
                _context.RemoveRange(pr);
                _context.RemoveRange(t);
                _context.RemoveRange(c);
                _context.RemoveRange(sc);
                _context.RemoveRange(h);
                _context.RemoveRange(a);
                _context.Remove(pf);
                await _context.SaveChangesAsync();
            }
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
