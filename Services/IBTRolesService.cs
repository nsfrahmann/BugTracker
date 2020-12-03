using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugTracker.Services
{
    public interface IBTRolesService
    {
        public Task<bool> AddUserToRole(BTUser user, string roleName);
        public Task<bool> IsUserInRole(BTUser user, string roleName);
        public Task<IEnumerable<string>> ListUserRoles(BTUser user);
        public Task<bool> RemoveUserFromRole(BTUser user, string roleName);
        public Task<ICollection<BTUser>> UsersInRole(string roleName);
        public Task<ICollection<BTUser>> UsersNotInRole(IdentityRole role);
        public SelectList RoleSelect();
        public Task<MultiSelectList> MultiUsersSelect();
        public Task<bool> CanInteractProject(int projectId);
        public Task<bool> CanInteractTicket(int ticketId);
    }
}
