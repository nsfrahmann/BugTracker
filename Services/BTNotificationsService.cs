using BugTracker.Data;
using BugTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Services
{
    public class BTNotificationsService : IBTNotificationsService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IHttpContextAccessor _httpContext;

        public BTNotificationsService(ApplicationDbContext context, UserManager<BTUser> userManager, IHttpContextAccessor httpContextAccesor)
        {
            _context = context;
            _userManager = userManager;
            _httpContext = httpContextAccesor;
        }

        public async Task<List<Notification>> NotificationList()
        {
            var user = _httpContext.HttpContext.User;
            var userId = _userManager.GetUserId(user);
            var notificationList = new List<Notification>();
            var notification = await _context.Notifications
                .Where(n => n.RecipientId == userId)
                .Include(n => n.Ticket)
                .ToListAsync();

            notificationList.AddRange(notification);
            _context.Notifications.AddRange(notification);
            
            return notificationList;
        }

        public List<Notification> UserNotifications()
        {
            var user = _httpContext.HttpContext.User;
            var userId = _userManager.GetUserId(user);
            var notification = _context.Notifications
                .Where(n => n.RecipientId == userId)
                .Include(n => n.Ticket)
                .ToList();

            return notification;
        }
    }
}
