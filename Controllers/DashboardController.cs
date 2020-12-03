using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BugTracker.Models;
using Microsoft.AspNetCore.Authorization;
using BugTracker.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;

        public DashboardController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<BTUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Home()
        {
            var profileCheck = _context.Profiles.FirstOrDefault(p => _userManager.GetUserId(User) == p.UserId);

            if (profileCheck == null)
            {
                Profile profile = new Profile
                {
                    UserId = _userManager.GetUserId(User),
                    Created = DateTime.Now
                };
                _context.Add(profile);
                _context.SaveChanges();
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
