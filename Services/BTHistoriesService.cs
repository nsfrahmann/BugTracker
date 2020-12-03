using BugTracker.Data;
using BugTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BugTracker.Services
{
    public class BTHistoriesService : IBTHistoriesService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IEmailSender _emailService;

        public BTHistoriesService(ApplicationDbContext context, UserManager<BTUser> userManager, IEmailSender emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;

        }

        public async Task AddHistory(Ticket oldTicket, Ticket newTicket, string userId)
        {

            if (oldTicket != newTicket)
            {
                if (oldTicket.DeveloperUserId == null && newTicket.DeveloperUserId != null && oldTicket.TicketPriority == null && oldTicket.TicketStatus == null)
                {
                    TicketHistory history1 = new TicketHistory
                    {
                        TicketId = newTicket.Id,
                        DeveloperAssigned = "Developer Assigned",
                        OldDeveloperValue = "None",
                        NewDeveloperValue = newTicket.DeveloperUser.FullName,
                        TicketTitle = "Title",
                        OldTitleValue = oldTicket.Title,
                        NewTitleValue = newTicket.Title,
                        TicketDescription = "Description",
                        OldDescriptionValue = oldTicket.Description,
                        NewDescriptionValue = newTicket.Description,
                        TicketType = "Type",
                        OldTypeValue = oldTicket.TicketType.Name,
                        NewTypeValue = newTicket.TicketType.Name,
                        TicketPriority = "Priority",
                        OldPriorityValue = "None",
                        NewPriorityValue = newTicket.TicketPriority.Name,
                        TicketStatus = "Status",
                        OldStatusValue = "None",
                        NewStatusValue = newTicket.TicketStatus.Name,
                        Created = DateTimeOffset.Now,
                        UserId = userId
                    };
                    await _context.TicketHistories.AddAsync(history1);
                }
                if (oldTicket.DeveloperUserId == null && newTicket.DeveloperUserId == null && oldTicket.TicketPriority == null && oldTicket.TicketStatus == null)
                {
                    TicketHistory history2 = new TicketHistory
                    {
                        TicketId = newTicket.Id,
                        DeveloperAssigned = "Developer Assigned",
                        OldDeveloperValue = "None",
                        NewDeveloperValue = "None",
                        TicketTitle = "Title",
                        OldTitleValue = oldTicket.Title,
                        NewTitleValue = newTicket.Title,
                        TicketDescription = "Description",
                        OldDescriptionValue = oldTicket.Description,
                        NewDescriptionValue = newTicket.Description,
                        TicketType = "Type",
                        OldTypeValue = oldTicket.TicketType.Name,
                        NewTypeValue = newTicket.TicketType.Name,
                        TicketPriority = "Priority",
                        OldPriorityValue = "None",
                        NewPriorityValue = newTicket.TicketPriority.Name,
                        TicketStatus = "Status",
                        OldStatusValue = "None",
                        NewStatusValue = newTicket.TicketStatus.Name,
                        Created = DateTimeOffset.Now,
                        UserId = userId
                    };
                    await _context.TicketHistories.AddAsync(history2);
                }
                if (oldTicket.DeveloperUserId != null && newTicket.DeveloperUserId == null && oldTicket.TicketPriority == null && oldTicket.TicketStatus == null)
                {
                    TicketHistory history3 = new TicketHistory
                    {
                        TicketId = newTicket.Id,
                        DeveloperAssigned = "Developer Assigned",
                        OldDeveloperValue = oldTicket.DeveloperUserId,
                        NewDeveloperValue = "None",
                        TicketTitle = "Title",
                        OldTitleValue = oldTicket.Title,
                        NewTitleValue = newTicket.Title,
                        TicketDescription = "Description",
                        OldDescriptionValue = oldTicket.Description,
                        NewDescriptionValue = newTicket.Description,
                        TicketType = "Type",
                        OldTypeValue = oldTicket.TicketType.Name,
                        NewTypeValue = newTicket.TicketType.Name,
                        TicketPriority = "Priority",
                        OldPriorityValue = "None",
                        NewPriorityValue = newTicket.TicketPriority.Name,
                        TicketStatus = "Status",
                        OldStatusValue = "None",
                        NewStatusValue = newTicket.TicketStatus.Name,
                        Created = DateTimeOffset.Now,
                        UserId = userId
                    };
                    await _context.TicketHistories.AddAsync(history3);
                }
                if (oldTicket.DeveloperUserId != null && newTicket.DeveloperUserId != null && oldTicket.TicketPriority == null && oldTicket.TicketStatus == null)
                {
                        TicketHistory history = new TicketHistory
                        {
                            TicketId = newTicket.Id,
                            DeveloperAssigned = "Developer Assigned",
                            OldDeveloperValue = oldTicket.DeveloperUser.FullName,
                            NewDeveloperValue = newTicket.DeveloperUser.FullName,
                            TicketTitle = "Title",
                            OldTitleValue = oldTicket.Title,
                            NewTitleValue = newTicket.Title,
                            TicketDescription = "Description",
                            OldDescriptionValue = oldTicket.Description,
                            NewDescriptionValue = newTicket.Description,
                            TicketType = "Type",
                            OldTypeValue = oldTicket.TicketType.Name,
                            NewTypeValue = newTicket.TicketType.Name,
                            TicketPriority = "Priority",
                            OldPriorityValue = "None",
                            NewPriorityValue = newTicket.TicketPriority.Name,
                            TicketStatus = "Status",
                            OldStatusValue = "None",
                            NewStatusValue = newTicket.TicketStatus.Name,
                            Created = DateTimeOffset.Now,
                            UserId = userId
                        };
                        await _context.TicketHistories.AddAsync(history);
                    }
                    if (oldTicket.DeveloperUserId != null && newTicket.DeveloperUserId != null && oldTicket.TicketPriority != null && oldTicket.TicketStatus == null)
                    {
                        TicketHistory history = new TicketHistory
                        {
                            TicketId = newTicket.Id,
                            DeveloperAssigned = "Developer Assigned",
                            OldDeveloperValue = oldTicket.DeveloperUser.FullName,
                            NewDeveloperValue = newTicket.DeveloperUser.FullName,
                            TicketTitle = "Title",
                            OldTitleValue = oldTicket.Title,
                            NewTitleValue = newTicket.Title,
                            TicketDescription = "Description",
                            OldDescriptionValue = oldTicket.Description,
                            NewDescriptionValue = newTicket.Description,
                            TicketType = "Type",
                            OldTypeValue = oldTicket.TicketType.Name,
                            NewTypeValue = newTicket.TicketType.Name,
                            TicketPriority = "Priority",
                            OldPriorityValue = oldTicket.TicketPriority.Name,
                            NewPriorityValue = newTicket.TicketPriority.Name,
                            TicketStatus = "Status",
                            OldStatusValue = "None",
                            NewStatusValue = newTicket.TicketStatus.Name,
                            Created = DateTimeOffset.Now,
                            UserId = userId
                        };
                        await _context.TicketHistories.AddAsync(history);
                    }

                    if (oldTicket.DeveloperUserId != null && newTicket.DeveloperUserId != null && oldTicket.TicketPriority != null && oldTicket.TicketStatus != null)
                    {
                        TicketHistory history = new TicketHistory
                        {
                            TicketId = newTicket.Id,
                            DeveloperAssigned = "Developer Assigned",
                            OldDeveloperValue = oldTicket.DeveloperUser.FullName,
                            NewDeveloperValue = newTicket.DeveloperUser.FullName,
                            TicketTitle = "Title",
                            OldTitleValue = oldTicket.Title,
                            NewTitleValue = newTicket.Title,
                            TicketDescription = "Description",
                            OldDescriptionValue = oldTicket.Description,
                            NewDescriptionValue = newTicket.Description,
                            TicketType = "Type",
                            OldTypeValue = oldTicket.TicketType.Name,
                            NewTypeValue = newTicket.TicketType.Name,
                            TicketPriority = "Priority",
                            OldPriorityValue = oldTicket.TicketPriority.Name,
                            NewPriorityValue = newTicket.TicketPriority.Name,
                            TicketStatus = "Status",
                            OldStatusValue = oldTicket.TicketStatus.Name,
                            NewStatusValue = newTicket.TicketStatus.Name,
                            Created = DateTimeOffset.Now,
                            UserId = userId
                        };
                        await _context.TicketHistories.AddAsync(history);
                    }

                    if (oldTicket.DeveloperUserId != null && newTicket.DeveloperUserId != null && oldTicket.TicketPriority == null && oldTicket.TicketStatus != null)
                    {
                        TicketHistory history = new TicketHistory
                        {
                            TicketId = newTicket.Id,
                            DeveloperAssigned = "Developer Assigned",
                            OldDeveloperValue = oldTicket.DeveloperUser.FullName,
                            NewDeveloperValue = newTicket.DeveloperUser.FullName,
                            TicketTitle = "Title",
                            OldTitleValue = oldTicket.Title,
                            NewTitleValue = newTicket.Title,
                            TicketDescription = "Description",
                            OldDescriptionValue = oldTicket.Description,
                            NewDescriptionValue = newTicket.Description,
                            TicketType = "Type",
                            OldTypeValue = oldTicket.TicketType.Name,
                            NewTypeValue = newTicket.TicketType.Name,
                            TicketPriority = "Priority",
                            OldPriorityValue = "None",
                            NewPriorityValue = newTicket.TicketPriority.Name,
                            TicketStatus = "Status",
                            OldStatusValue = oldTicket.TicketStatus.Name,
                            NewStatusValue = newTicket.TicketStatus.Name,
                            Created = DateTimeOffset.Now,
                            UserId = userId
                        };
                        await _context.TicketHistories.AddAsync(history);
                    }

                    if (oldTicket.DeveloperUserId != null && newTicket.DeveloperUserId == null && oldTicket.TicketPriority != null && oldTicket.TicketStatus != null)
                    {
                        TicketHistory history = new TicketHistory
                        {
                            TicketId = newTicket.Id,
                            DeveloperAssigned = "Developer Assigned",
                            OldDeveloperValue = oldTicket.DeveloperUser.FullName,
                            NewDeveloperValue = "None",
                            TicketTitle = "Title",
                            OldTitleValue = oldTicket.Title,
                            NewTitleValue = newTicket.Title,
                            TicketDescription = "Description",
                            OldDescriptionValue = oldTicket.Description,
                            NewDescriptionValue = newTicket.Description,
                            TicketType = "Type",
                            OldTypeValue = oldTicket.TicketType.Name,
                            NewTypeValue = newTicket.TicketType.Name,
                            TicketPriority = "Priority",
                            OldPriorityValue = oldTicket.TicketPriority.Name,
                            NewPriorityValue = newTicket.TicketPriority.Name,
                            TicketStatus = "Status",
                            OldStatusValue = oldTicket.TicketStatus.Name,
                            NewStatusValue = newTicket.TicketStatus.Name,
                            Created = DateTimeOffset.Now,
                            UserId = userId
                        };
                        await _context.TicketHistories.AddAsync(history);
                    }

                    if (oldTicket.DeveloperUserId == null && newTicket.DeveloperUserId != null && oldTicket.TicketPriority != null && oldTicket.TicketStatus != null)
                    {
                        TicketHistory history = new TicketHistory
                        {
                            TicketId = newTicket.Id,
                            DeveloperAssigned = "Developer Assigned",
                            OldDeveloperValue = "None",
                            NewDeveloperValue = newTicket.DeveloperUser.FullName,
                            TicketTitle = "Title",
                            OldTitleValue = oldTicket.Title,
                            NewTitleValue = newTicket.Title,
                            TicketDescription = "Description",
                            OldDescriptionValue = oldTicket.Description,
                            NewDescriptionValue = newTicket.Description,
                            TicketType = "Type",
                            OldTypeValue = oldTicket.TicketType.Name,
                            NewTypeValue = newTicket.TicketType.Name,
                            TicketPriority = "Priority",
                            OldPriorityValue = oldTicket.TicketPriority.Name,
                            NewPriorityValue = newTicket.TicketPriority.Name,
                            TicketStatus = "Status",
                            OldStatusValue = oldTicket.TicketStatus.Name,
                            NewStatusValue = newTicket.TicketStatus.Name,
                            Created = DateTimeOffset.Now,
                            UserId = userId
                        };
                        await _context.TicketHistories.AddAsync(history);
                    }

                    if (oldTicket.DeveloperUserId == null && newTicket.DeveloperUserId != null && oldTicket.TicketPriority != null && oldTicket.TicketStatus == null)
                    {
                        TicketHistory history = new TicketHistory
                        {
                            TicketId = newTicket.Id,
                            DeveloperAssigned = "Developer Assigned",
                            OldDeveloperValue = "None",
                            NewDeveloperValue = newTicket.DeveloperUser.FullName,
                            TicketTitle = "Title",
                            OldTitleValue = oldTicket.Title,
                            NewTitleValue = newTicket.Title,
                            TicketDescription = "Description",
                            OldDescriptionValue = oldTicket.Description,
                            NewDescriptionValue = newTicket.Description,
                            TicketType = "Type",
                            OldTypeValue = oldTicket.TicketType.Name,
                            NewTypeValue = newTicket.TicketType.Name,
                            TicketPriority = "Priority",
                            OldPriorityValue = oldTicket.TicketPriority.Name,
                            NewPriorityValue = newTicket.TicketPriority.Name,
                            TicketStatus = "Status",
                            OldStatusValue = "None",
                            NewStatusValue = newTicket.TicketStatus.Name,
                            Created = DateTimeOffset.Now,
                            UserId = userId
                        };
                        await _context.TicketHistories.AddAsync(history);
                    }

                    if (oldTicket.DeveloperUserId == null && newTicket.DeveloperUserId != null && oldTicket.TicketPriority == null && oldTicket.TicketStatus != null)
                    {
                        TicketHistory history = new TicketHistory
                        {
                            TicketId = newTicket.Id,
                            DeveloperAssigned = "Developer Assigned",
                            OldDeveloperValue = "None",
                            NewDeveloperValue = newTicket.DeveloperUser.FullName,
                            TicketTitle = "Title",
                            OldTitleValue = oldTicket.Title,
                            NewTitleValue = newTicket.Title,
                            TicketDescription = "Description",
                            OldDescriptionValue = oldTicket.Description,
                            NewDescriptionValue = newTicket.Description,
                            TicketType = "Type",
                            OldTypeValue = oldTicket.TicketType.Name,
                            NewTypeValue = newTicket.TicketType.Name,
                            TicketPriority = "Priority",
                            OldPriorityValue = "None",
                            NewPriorityValue = newTicket.TicketPriority.Name,
                            TicketStatus = "Status",
                            OldStatusValue = oldTicket.TicketStatus.Name,
                            NewStatusValue = newTicket.TicketStatus.Name,
                            Created = DateTimeOffset.Now,
                            UserId = userId
                        };
                        await _context.TicketHistories.AddAsync(history);
                    }

                    if (oldTicket.DeveloperUserId == null && newTicket.DeveloperUserId == null && oldTicket.TicketPriority != null && oldTicket.TicketStatus == null)
                    {
                        TicketHistory history = new TicketHistory
                        {
                            TicketId = newTicket.Id,
                            DeveloperAssigned = "Developer Assigned",
                            OldDeveloperValue = "None",
                            NewDeveloperValue = "None",
                            TicketTitle = "Title",
                            OldTitleValue = oldTicket.Title,
                            NewTitleValue = newTicket.Title,
                            TicketDescription = "Description",
                            OldDescriptionValue = oldTicket.Description,
                            NewDescriptionValue = newTicket.Description,
                            TicketType = "Type",
                            OldTypeValue = oldTicket.TicketType.Name,
                            NewTypeValue = newTicket.TicketType.Name,
                            TicketPriority = "Priority",
                            OldPriorityValue = oldTicket.TicketPriority.Name,
                            NewPriorityValue = newTicket.TicketPriority.Name,
                            TicketStatus = "Status",
                            OldStatusValue = "None",
                            NewStatusValue = newTicket.TicketStatus.Name,
                            Created = DateTimeOffset.Now,
                            UserId = userId
                        };
                        await _context.TicketHistories.AddAsync(history);
                    }

                    if (oldTicket.DeveloperUserId == null && newTicket.DeveloperUserId == null && oldTicket.TicketPriority == null && oldTicket.TicketStatus != null)
                    {
                        TicketHistory history = new TicketHistory
                        {
                            TicketId = newTicket.Id,
                            DeveloperAssigned = "Developer Assigned",
                            OldDeveloperValue = "None",
                            NewDeveloperValue = "None",
                            TicketTitle = "Title",
                            OldTitleValue = oldTicket.Title,
                            NewTitleValue = newTicket.Title,
                            TicketDescription = "Description",
                            OldDescriptionValue = oldTicket.Description,
                            NewDescriptionValue = newTicket.Description,
                            TicketType = "Type",
                            OldTypeValue = oldTicket.TicketType.Name,
                            NewTypeValue = newTicket.TicketType.Name,
                            TicketPriority = "Priority",
                            OldPriorityValue = "None",
                            NewPriorityValue = newTicket.TicketPriority.Name,
                            TicketStatus = "Status",
                            OldStatusValue = oldTicket.TicketStatus.Name,
                            NewStatusValue = newTicket.TicketStatus.Name,
                            Created = DateTimeOffset.Now,
                            UserId = userId
                        };
                        await _context.TicketHistories.AddAsync(history);
                    }

                    if (oldTicket.DeveloperUserId != null && newTicket.DeveloperUserId == null && oldTicket.TicketPriority != null && oldTicket.TicketStatus == null)
                    {
                        TicketHistory history = new TicketHistory
                        {
                            TicketId = newTicket.Id,
                            DeveloperAssigned = "Developer Assigned",
                            OldDeveloperValue = oldTicket.DeveloperUser.FullName,
                            NewDeveloperValue = "None",
                            TicketTitle = "Title",
                            OldTitleValue = oldTicket.Title,
                            NewTitleValue = newTicket.Title,
                            TicketDescription = "Description",
                            OldDescriptionValue = oldTicket.Description,
                            NewDescriptionValue = newTicket.Description,
                            TicketType = "Type",
                            OldTypeValue = oldTicket.TicketType.Name,
                            NewTypeValue = newTicket.TicketType.Name,
                            TicketPriority = "Priority",
                            OldPriorityValue = oldTicket.TicketPriority.Name,
                            NewPriorityValue = newTicket.TicketPriority.Name,
                            TicketStatus = "Status",
                            OldStatusValue = "None",
                            NewStatusValue = newTicket.TicketStatus.Name,
                            Created = DateTimeOffset.Now,
                            UserId = userId
                        };
                        await _context.TicketHistories.AddAsync(history);
                    }

                    if (oldTicket.DeveloperUserId != null && newTicket.DeveloperUserId == null && oldTicket.TicketPriority == null && oldTicket.TicketStatus != null)
                    {
                        TicketHistory history = new TicketHistory
                        {
                            TicketId = newTicket.Id,
                            DeveloperAssigned = "Developer Assigned",
                            OldDeveloperValue = oldTicket.DeveloperUser.FullName,
                            NewDeveloperValue = "None",
                            TicketTitle = "Title",
                            OldTitleValue = oldTicket.Title,
                            NewTitleValue = newTicket.Title,
                            TicketDescription = "Description",
                            OldDescriptionValue = oldTicket.Description,
                            NewDescriptionValue = newTicket.Description,
                            TicketType = "Type",
                            OldTypeValue = oldTicket.TicketType.Name,
                            NewTypeValue = newTicket.TicketType.Name,
                            TicketPriority = "Priority",
                            OldPriorityValue = "None",
                            NewPriorityValue = newTicket.TicketPriority.Name,
                            TicketStatus = "Status",
                            OldStatusValue = oldTicket.TicketStatus.Name,
                            NewStatusValue = newTicket.TicketStatus.Name,
                            Created = DateTimeOffset.Now,
                            UserId = userId
                        };
                        await _context.TicketHistories.AddAsync(history);
                    }

                    if (oldTicket.DeveloperUserId == null && newTicket.DeveloperUserId == null && oldTicket.TicketPriority != null && oldTicket.TicketStatus != null)
                    {
                        TicketHistory history = new TicketHistory
                        {
                            TicketId = newTicket.Id,
                            DeveloperAssigned = "Developer Assigned",
                            OldDeveloperValue = "None",
                            NewDeveloperValue = "None",
                            TicketTitle = "Title",
                            OldTitleValue = oldTicket.Title,
                            NewTitleValue = newTicket.Title,
                            TicketDescription = "Description",
                            OldDescriptionValue = oldTicket.Description,
                            NewDescriptionValue = newTicket.Description,
                            TicketType = "Type",
                            OldTypeValue = oldTicket.TicketType.Name,
                            NewTypeValue = newTicket.TicketType.Name,
                            TicketPriority = "Priority",
                            OldPriorityValue = oldTicket.TicketPriority.Name,
                            NewPriorityValue = newTicket.TicketPriority.Name,
                            TicketStatus = "Status",
                            OldStatusValue = oldTicket.TicketStatus.Name,
                            NewStatusValue = newTicket.TicketStatus.Name,
                            Created = DateTimeOffset.Now,
                            UserId = userId
                        };
                        await _context.TicketHistories.AddAsync(history);
                    }
                }                
                await _context.SaveChangesAsync();

            if (oldTicket.DeveloperUserId != newTicket.DeveloperUserId)
            {
                Notification notification = new Notification
                {
                    TicketId = newTicket.Id,
                    Description = "You have been removed from Ticket",
                    Created = DateTime.Now,
                    SenderId = userId,
                    RecipientId = oldTicket.DeveloperUserId
                };

                if (userId != oldTicket.DeveloperUserId && oldTicket.DeveloperUserId != null)
                {
                    string DevEmail = oldTicket.DeveloperUser.Email;
                    string subject = "New Ticket Activity";
                    string message = $"You have been removed from ticket: (newTicket.Title)";
                    await _context.Notifications.AddAsync(notification);
                    await _emailService.SendEmailAsync(DevEmail, subject, message);
                }

                Notification notification2 = new Notification
                {
                    TicketId = newTicket.Id,
                    Description = "You have a new Ticket",
                    Created = DateTime.Now,
                    SenderId = userId,
                    RecipientId = newTicket.DeveloperUserId
                };

                if (userId != newTicket.DeveloperUserId && newTicket.DeveloperUserId != null)
                {
                    string DevEmail2 = newTicket.DeveloperUser.Email;
                    string subject2 = "New Ticket Activity";
                    string message2 = $"You have been assigned to ticket: (newTicket.Title)";
                    await _context.Notifications.AddAsync(notification2);
                    await _emailService.SendEmailAsync(DevEmail2, subject2, message2);
                }
            }
            if (oldTicket.Title != newTicket.Title)
            {
                Notification notification = new Notification
                {
                    TicketId = newTicket.Id,
                    Description = "Ticket title changed",
                    Created = DateTime.Now,
                    SenderId = userId,
                    RecipientId = newTicket.DeveloperUserId
                };

                if (userId != newTicket.DeveloperUserId && newTicket.DeveloperUserId != null)
                {
                    string DevEmail = newTicket.DeveloperUser.Email;
                    string subject = "New Ticket Activity";
                    string message = $"oldTicket.Title title has been changed to newTicket.Title";
                    await _context.Notifications.AddAsync(notification);
                    await _emailService.SendEmailAsync(DevEmail, subject, message);
                }
            }
            if (oldTicket.Description != newTicket.Description)
            {
                Notification notification = new Notification
                {
                    TicketId = newTicket.Id,
                    Description = "Ticket description changed",
                    Created = DateTime.Now,
                    SenderId = userId,
                    RecipientId = newTicket.DeveloperUserId
                };

                if (userId != newTicket.DeveloperUserId && newTicket.DeveloperUserId != null)
                {
                    string DevEmail = newTicket.DeveloperUser.Email;
                    string subject = "New Ticket Activity";
                    string message = $"Description has changed for ticket: (newTicket.Title)";
                    await _context.Notifications.AddAsync(notification);
                    await _emailService.SendEmailAsync(DevEmail, subject, message);
                }
            }
            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId && newTicket.TicketPriorityId != null)
            {
                Notification notification = new Notification
                {
                    TicketId = newTicket.Id,
                    Description = "Ticket priority changed",
                    Created = DateTime.Now,
                    SenderId = userId,
                    RecipientId = newTicket.DeveloperUserId
                };

                if (userId != newTicket.DeveloperUserId && newTicket.DeveloperUserId != null)
                {
                    string DevEmail = newTicket.DeveloperUser.Email;
                    string subject = "New Ticket Activity";
                    string message = $"Priority has been changed for ticket: (newTicket.Title)";
                    await _context.Notifications.AddAsync(notification);
                    await _emailService.SendEmailAsync(DevEmail, subject, message);
                }
            }
            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                Notification notification = new Notification
                {
                    TicketId = newTicket.Id,
                    Description = "Ticket type changed",
                    Created = DateTime.Now,
                    SenderId = userId,
                    RecipientId = newTicket.DeveloperUserId
                };

                if (userId != newTicket.DeveloperUserId && newTicket.DeveloperUserId != null)
                {
                    string DevEmail = newTicket.DeveloperUser.Email;
                    string subject = "New Ticket Activity";
                    string message = $"Ticket type has been changed for ticket: (newTicket.Title)";
                    await _context.Notifications.AddAsync(notification);
                    await _emailService.SendEmailAsync(DevEmail, subject, message);
                }
            }
            if (oldTicket.TicketStatusId != newTicket.TicketStatusId && newTicket.TicketStatusId != null)
            {
                Notification notification = new Notification
                {
                    TicketId = newTicket.Id,
                    Description = "Ticket status changed",
                    Created = DateTime.Now,
                    SenderId = userId,
                    RecipientId = newTicket.DeveloperUserId
                };

                if (userId != newTicket.DeveloperUserId && newTicket.DeveloperUserId != null)
                {
                    string DevEmail = newTicket.DeveloperUser.Email;
                    string subject = "New Ticket Activity";
                    string message = $"Status has been changed for ticket: (newTicket.Title)";
                    await _context.Notifications.AddAsync(notification);
                    await _emailService.SendEmailAsync(DevEmail, subject, message);
                }
            }
            if (newTicket.TicketComments.Count > oldTicket.TicketComments.Count)
            {
                Notification notification = new Notification
                {
                    TicketId = newTicket.Id,
                    Description = "New Ticket Comment",
                    Created = DateTime.Now,
                    SenderId = userId,
                    RecipientId = newTicket.DeveloperUserId
                };

                if (userId != newTicket.DeveloperUserId && newTicket.DeveloperUserId != null)
                {
                    string DevEmail = newTicket.DeveloperUser.Email;
                    string subject = "New Ticket Activity";
                    string message = $"Comment has been posted for ticket: (newTicket.Title)";
                    await _context.Notifications.AddAsync(notification);
                    await _emailService.SendEmailAsync(DevEmail, subject, message);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<TicketHistory>> GetHistories(int ticketId)
        {
            var model = new List<TicketHistory>();
            var tickets = await _context.TicketHistories
                .Where(t => t.TicketId == ticketId)
                .ToListAsync();
            model.AddRange(tickets);
            return model;
        }
    }

}

