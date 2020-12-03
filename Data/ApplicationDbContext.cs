using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BugTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<BTUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<TicketType>().HasData(
                new TicketType
                {
                    Id = 1,
                    Name = "Database"
                },
                new TicketType
                {
                    Id = 2,
                    Name = "BackEnd"
                },
                new TicketType
                {
                    Id = 3,
                    Name = "FrontEnd"
                }
            );
            builder.Entity<TicketPriority>().HasData(
                new TicketPriority
                {
                    Id = 1,
                    Name = "Low"
                },
                new TicketPriority
                {
                    Id = 2,
                    Name = "High"
                },
                new TicketPriority
                {
                    Id = 3,
                    Name = "Urgent"
                }
            );
            builder.Entity<TicketStatus>().HasData(
                new TicketStatus
                {
                    Id = 1,
                    Name = "New"
                },
                new TicketStatus
                {
                    Id = 2,
                    Name = "Open"
                },
                new TicketStatus
                {
                    Id = 3,
                    Name = "In Progress"
                },
                new TicketStatus
                {
                    Id = 4,
                    Name = "Closed"
                }
            );
            builder.Entity<ProjectUser>()
                .HasKey(pu => new { pu.ProjectId, pu.UserId });

        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public DbSet<TicketAttachment> TicketAttachments { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }
        public DbSet<TicketSubComment> TicketSubComments { get; set; }
        public DbSet<TicketHistory> TicketHistories { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<TicketPriority> TicketPriorities { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Profile> Profiles { get; set; }
    }
}
