using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Services
{
    public interface IBTNotificationsService
    {
        public Task<List<Notification>> NotificationList();
        public List<Notification> UserNotifications();
    }
}
