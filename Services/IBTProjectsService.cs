using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugTracker.Models;

namespace BugTracker.Services
{
    public interface IBTProjectsService
    {
        public Task<bool> IsUserOnProject(string userId, int projectId);
        public Task<ICollection<Project>> ListUserProjects(string userId);
        public Task AddUserToProject(string userId, int projectId);
        public Task RemoveUserFromProject(string userId, int projectId);
        public Task<ICollection<BTUser>> UsersOnProject(int projectId);
        public Task<ICollection<BTUser>> UsersNotOnProject(int projectId);
        public Task<List<Project>> All();
        public Project Project();
        public Task<int> GetUserProjectCount(string userId);
        public Task<bool> OwnsProject(int projectId);
        public Task<BTUser> GetProjectUser(string userId);
    }
}
