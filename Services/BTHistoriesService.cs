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
        private readonly IBTTicketsService _ticketsService;

        public BTHistoriesService(ApplicationDbContext context, UserManager<BTUser> userManager, IEmailSender emailService, IBTTicketsService ticketsService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
            _ticketsService = ticketsService;
        }

        public async Task AddHistory(Ticket oldTicket, Ticket newTicket, string userId)
        {

            if (oldTicket != newTicket)
            {
                TicketHistory history = new TicketHistory();

                history.TicketId = newTicket.Id;
                history.DeveloperAssigned = "Developer Assigned";
                if (oldTicket.DeveloperUserId != null)
                {
                    history.OldDeveloperValue = oldTicket.DeveloperUser.FullName;
                }
                else
                {
                    history.OldDeveloperValue = "None";
                }
                if (newTicket.DeveloperUserId != null)
                {
                    history.NewDeveloperValue = newTicket.DeveloperUser.FullName;
                }
                else
                {
                    history.NewDeveloperValue = "None";
                }
                history.TicketTitle = "Title";
                history.OldTitleValue = oldTicket.Title;
                history.NewTitleValue = newTicket.Title;
                history.TicketDescription = "Description";
                history.OldDescriptionValue = oldTicket.Description;
                history.NewDescriptionValue = newTicket.Description;
                history.TicketType = "Type";
                history.OldTypeValue = oldTicket.TicketType.Name;
                history.NewTypeValue = newTicket.TicketType.Name;
                history.TicketPriority = "Priority";
                if (oldTicket.TicketPriorityId != null)
                {
                    history.OldPriorityValue = oldTicket.TicketPriority.Name;
                }
                else
                {
                    history.OldPriorityValue = "None";
                }

                if (newTicket.TicketPriorityId != null)
                {
                    history.NewPriorityValue = newTicket.TicketPriority.Name;
                }
                else
                {
                    history.NewPriorityValue = "None";
                }

                if (oldTicket.TicketStatusId != null)
                {
                    history.OldStatusValue = oldTicket.TicketStatus.Name;
                }
                else
                {
                    history.OldStatusValue = "None";
                }

                if (newTicket.TicketStatusId != null)
                {
                    history.NewStatusValue = newTicket.TicketStatus.Name;
                }
                else
                {
                    history.NewStatusValue = "None";
                }
                history.Created = DateTimeOffset.Now;
                history.UserId = userId;

                await _context.TicketHistories.AddAsync(history);
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
                var totalcom = await _ticketsService.GetCommentCount(newTicket.Id);
                if ((totalcom - newTicket.TicketComments.Count) < (totalcom - oldTicket.TicketComments.Count))
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

                if (newTicket.Attachments.Count > oldTicket.Attachments.Count)
                {
                    Notification notification = new Notification
                    {
                        TicketId = newTicket.Id,
                        Description = "New Ticket Attachment",
                        Created = DateTime.Now,
                        SenderId = userId,
                        RecipientId = newTicket.DeveloperUserId
                    };

                    if (userId != newTicket.DeveloperUserId && newTicket.DeveloperUserId != null)
                    {
                        string DevEmail = newTicket.DeveloperUser.Email;
                        string subject = "New Ticket Activity";
                        string message = $"Attachment has been posted for ticket: (newTicket.Title)";
                        await _context.Notifications.AddAsync(notification);
                        await _emailService.SendEmailAsync(DevEmail, subject, message);
                    }
                }
                await _context.SaveChangesAsync();
            }

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

