using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTracker.Data;
using BugTracker.Models;
using BugTracker.Models.ViewModels;
using BugTracker.Services;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTProjectsService _projectsService;
        private readonly UserManager<BTUser> _userManager;

        public ProjectsController(ApplicationDbContext context, IBTProjectsService projectsService, UserManager<BTUser> userManager)
        {
            _context = context;
            _projectsService = projectsService;
            _userManager = userManager;
        }

        // GET: Projects
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var projects = await _projectsService.All();

            return View(projects);
        }

        // GET: Projects/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var projectUsers = new List<SelectListItem>();
            foreach (var user in (await _projectsService.UsersNotOnProject(id)))
            {
                if (!await _userManager.IsInRoleAsync(user, "Demo User"))
                {
                    projectUsers.Add(new SelectListItem() { Text = user.FullName, Value = user.Id });
                }
            }
            ViewData["projectUsers"] = new MultiSelectList(projectUsers, "Value", "Text");

            var project = await _context.Projects
                .Include(p => p.OwnerUser)
                .Include(p => p.ProjectUsers)
                .ThenInclude(p => p.User)
                .Include(p => p.Tickets)
                .ThenInclude(t => t.TicketPriority)
                .Include(p => p.Tickets)
                .ThenInclude(t => t.TicketStatus)
                .Include(p => p.Tickets)
                .ThenInclude(t => t.TicketType)
                .Include(p => p.Tickets)
                .ThenInclude(t => t.OwnerUser)
                .Include(p => p.Tickets)
                .ThenInclude(t => t.DeveloperUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            return View(project);
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Summary,OwnerUser,OwnerUserId,ImagePath,ImageData")] Project project)
        {            
            if (ModelState.IsValid)
            {
                project.OwnerUser = await _userManager.GetUserAsync(User);
                project.OwnerUserId = project.OwnerUser.Id;
                project.Created = DateTime.Now;
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Summary,OwnerUser,OwnerUserId,ImagePath,ImageData,Created")] Project project, List<string> selectedUsers)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    project.Updated = DateTime.Now;
                    project.ProjectUserIds = selectedUsers;
                    foreach (var user in selectedUsers)
                    {
                        await _projectsService.AddUserToProject(user, id);
                    }
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
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
            return View(project);
        }

        // POST: Projects/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }

        //[Authorize]
        //[HttpGet]
        //public async Task<IActionResult> AssignUsers(int id)
        //{
        //    var model = new ManageProjectUsersViewModel();
        //    var project = await _context.Projects
        //        .Include(p => p.ProjectUsers)
        //        .ThenInclude(p => p.User)
        //        .FirstAsync(p => p.Id == id);

        //    model.Project = project;
        //    List<BTUser> users = await _context.Users.ToListAsync();
        //    List<BTUser> members = (List<BTUser>)await _projectsService.UsersOnProject(id);
        //    model.Users = new MultiSelectList(users, "Id", "FullName", members);
        //    return View(model);
        //}

        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AssignUsers(Project project)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (project.ProjectUsers != null)
        //        {
        //            var currentMembers = await _context.Projects
        //                .Include(p => p.ProjectUsers)
        //                .FirstOrDefaultAsync(p => p.Id == project.Id);
        //            List<string> memberIds = currentMembers.ProjectUsers.Select(u => u.UserId).ToList();

        //            foreach (string id in memberIds)
        //            {
        //                await _projectsService.RemoveUserFromProject(id, model.Project.Id);
        //            }

        //            foreach (string id in model.SelectedUsers)
        //            {
        //                await _projectsService.AddUserToProject(id, model.Project.Id);
        //            }
        //            var myId = model.Project.Id;
        //            return RedirectToAction("Details", new { Id = myId });
        //        }
        //        else
        //        {
        //        }
        //    }
        //    return View(model);
        //}
    }
}
