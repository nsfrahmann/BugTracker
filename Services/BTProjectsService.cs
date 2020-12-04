using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BugTracker.Data;
using BugTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Services
{    

    public class BTProjectsService : IBTProjectsService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IHttpContextAccessor _httpContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public BTProjectsService(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<BTUser> userManager, IHttpContextAccessor httpContext)
        {
            _context = context;
            _userManager = userManager;
            _httpContext = httpContext;
            _roleManager = roleManager;
        }

        public async Task AddUserToProject(string userId, int projectId)
        {
            if (!await IsUserOnProject(userId, projectId))
            {
                try
                {
                    await _context.ProjectUsers.AddAsync(new ProjectUser { ProjectId = projectId, UserId = userId });
                    await _context.SaveChangesAsync();
                    //Project project = _context.Projects.Find(projectId);
                    //BTUser user = _context.Users.Find(userId);

                    //ProjectUser projectUser = new ProjectUser()
                    //{
                    //    ProjectId = project.Id,
                    //    UserId = user.Id
                    //};
                }
                catch(Exception ex)
                {
                    Debug.WriteLine($"Error adding user to project. --> {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<bool> IsUserOnProject(string userId, int projectId)
        {
            //Project project = await _context.Projects
            //    .Include(u => u.ProjectUsers.Where(u => u.UserId == userId))
            //    .ThenInclude(u => u.User)
            //    .FirstOrDefaultAsync(u => u.Id == projectId);
            //bool result = project.ProjectUsers.Any(u => u.UserId == userId);
            //return result;

            return await _context.ProjectUsers.Where(pu => pu.UserId == userId && pu.ProjectId == projectId).AnyAsync();
        }
        public async Task<ICollection<Project>> ListUserProjects(string userId)
        {
            BTUser user = await _context.Users
                .Include(p => p.ProjectUsers)
                .ThenInclude(p => p.Project)
                .FirstOrDefaultAsync(p => p.Id == userId);

            List<Project> projects = user.ProjectUsers.SelectMany(p => (IEnumerable<Project>)p.Project).ToList();
            return projects;
        }

        public async Task RemoveUserFromProject(string userId, int projectId)
        {
            if (await IsUserOnProject(userId, projectId))
            {
                try
                {
                    ProjectUser projectUser = await _context.ProjectUsers.Where(m => m.UserId == userId && m.ProjectId == projectId).FirstOrDefaultAsync();
                    _context.ProjectUsers.Remove(projectUser);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error removing user from project. --> {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<ICollection<BTUser>> UsersNotOnProject(int projectId)
        {
            return await _context.Users.Where(u => u.ProjectUsers.All(p => p.ProjectId != projectId)).ToListAsync();
        }

        public async Task<ICollection<BTUser>> UsersOnProject(int projectId)
        {
            Project project = await _context.Projects
                .Include(u => u.ProjectUsers)
                .ThenInclude(u => u.User)
                .FirstOrDefaultAsync(u => u.Id == projectId);

            List<BTUser> projectUsers = project.ProjectUsers.Select(p => p.User).ToList();
            return projectUsers;
        }

        public async Task<List<Project>> All()
        {
            var model = await _context.Projects
                .Include(p => p.OwnerUser)
                .Include(p => p.ProjectUsers)
                .ToListAsync();
            return model;
        }

        public async Task<List<ProjectUser>> AssignedProjects(string userId)
        {
            var model = new List<ProjectUser>();
            var projects = await _context.ProjectUsers
                .Where(pu => pu.UserId == userId)
                .Include(pu => pu.Project)
                .ToListAsync();
            model.AddRange(projects);
            var modelList = model.Distinct().ToList();
            return modelList;
        }

        public Project Project()
        {
            var project = new Project();
            return project;
        }

        public async Task<int> GetUserProjectCount(string userId)
        {
            return await _context.ProjectUsers.Where(pu => pu.UserId == userId).CountAsync();
        }

        public async Task<bool> OwnsProject(int projectId)
        {
            var userLogged = _httpContext.HttpContext.User;
            var userId = _userManager.GetUserId(userLogged);
            var ticket = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId && p.OwnerUserId == userId);

            bool result = false;
            if (ticket != null)
            {
                result = true;
            }

            return result;
        }

        public async Task<BTUser> GetProjectUser(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user;
        }
    }
}
