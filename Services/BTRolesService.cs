using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugTracker.Data;
using BugTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Services
{
    public class BTRolesService : IBTRolesService
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BTUser> _userManager;
        private readonly IHttpContextAccessor _httpContext;

        public BTRolesService(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<BTUser> userManager, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<bool> AddUserToRole(BTUser user, string roleName)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }
        public async Task<bool> IsUserInRole(BTUser user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);   
        }

        public async Task<IEnumerable<string>> ListUserRoles(BTUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> RemoveUserFromRole(BTUser user, string roleName)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<ICollection<BTUser>> UsersInRole(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return users;
        }

        public async Task<ICollection<BTUser>> UsersNotInRole(IdentityRole role)
        {
            //var roleId = await _roleManager.GetRoleIdAsync(role);
            return await _userManager.Users.Where(u => IsUserInRole(u, role.Name).Result == false).ToListAsync();
        }

        public async Task<SelectList> RoleSelect()
        {
            var thisUser = await _userManager.GetUserAsync(_httpContext.HttpContext.User);
            if (await _userManager.IsInRoleAsync(thisUser, "Project Manager"))
            {
                var roles = await _context.Roles.Where(r => (r.Name != "Demo User") && (r.Name != "Administrator")).ToListAsync();
                var roleSelect = new SelectList(roles, "Name", "Name");
                return roleSelect;
            }
            else
            {
                var roles = await _context.Roles.Where(r => r.Name != "Demo User").ToListAsync();
                var roleSelect = new SelectList(roles, "Name", "Name");
                return roleSelect;
            }
        }

        public async Task<List<BTUser>> AllUsersSelect()
        {

            var thisUser = await _userManager.GetUserAsync(_httpContext.HttpContext.User);
            var totalUsers = _context.Users.Where(u => u.Id != thisUser.Id).ToList();
            var selectOptions = new List<BTUser>();

            foreach (var user in totalUsers)
            {
                if (!(await _userManager.IsInRoleAsync(user, "Demo User")))
                {
                    if ((await _userManager.IsInRoleAsync(thisUser, "Project Manager")) && (await _userManager.IsInRoleAsync(user, "Administrator")))
                    {
                    }
                    else
                    {
                        selectOptions.Add(user);
                    }
                }
            }

            return selectOptions;
        }

        public async Task<bool> CanInteractProject(int projectId)
        {
            var userLogged = _httpContext.HttpContext.User;
            var userId = _userManager.GetUserId(userLogged);
            var thisUser = _context.Users.Find(userId);
            var roleName = _userManager.GetRolesAsync(thisUser).Result.FirstOrDefault();

            switch (roleName)
            {
                case "Administrator":
                    return true;
                case "Project Manager":
                    var onProject = _context.Projects.Find(projectId).ProjectUserIds;
                    if (onProject != null)
                    {
                        if (await _context.Projects.Where(p => p.OwnerUserId == userId || onProject.Contains(userId)).AnyAsync())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (await _context.Projects.Where(p => p.OwnerUserId == userId).AnyAsync())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                default:
                    return false;
            }
        }

        public async Task<bool> CanInteractTicket(int ticketId)
        {
            var userLogged = _httpContext.HttpContext.User;
            var userId = _userManager.GetUserId(userLogged);
            var thisUser = _context.Users.Find(userId);
            var roleName = _userManager.GetRolesAsync(thisUser).Result.FirstOrDefault();

            bool result = false;
            switch (roleName)
            {
                case "Administrator":
                    result = true;
                    break;
                case "Project Manager":
                    var projectId = _context.Tickets.Find(ticketId).ProjectId;
                    var onProject = _context.Projects.Find(projectId).ProjectUserIds;
                    if (onProject != null)
                    {
                        if (await _context.Projects.Where(p => p.OwnerUserId == userId || onProject.Contains(userId)).AnyAsync())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (await _context.Projects.Where(p => p.OwnerUserId == userId).AnyAsync())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                case "Developer":
                    if (await _context.Tickets.Where(t => t.Id == ticketId && (t.DeveloperUserId == thisUser.Id || t.OwnerUserId == thisUser.Id)).AnyAsync())
                    {
                        result = true;
                        break;
                    }
                    break;
                case "Submitter":
                    if (await _context.Tickets.Where(t => t.Id == ticketId && (t.OwnerUserId == thisUser.Id || t.DeveloperUserId == thisUser.Id)).AnyAsync())
                    {
                        result = true;
                        break;
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
