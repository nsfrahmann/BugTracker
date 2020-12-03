using BugTracker.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Data
{    
    public static class ContextSeed
    {
        public static async Task RunSeedMethodsAsync(
            RoleManager<IdentityRole> roleManager,
            UserManager<BTUser> userManager,
            ApplicationDbContext context)
        {
            await SeedRolesAsync(roleManager);
            await SeedDefaultUsersAsync(userManager);
            await SeedProjectsAsync(context);
            await SeedProjectUsersAsync(context, userManager);
            await SeedTicketsAsync(context, userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
                await roleManager.CreateAsync(new IdentityRole("Administrator"));
                await roleManager.CreateAsync(new IdentityRole("Project Manager"));
                await roleManager.CreateAsync(new IdentityRole("Developer"));
                await roleManager.CreateAsync(new IdentityRole("Submitter"));
                await roleManager.CreateAsync(new IdentityRole("New User"));
                await roleManager.CreateAsync(new IdentityRole("Demo User"));
        }

        private static async Task SeedDefaultUsersAsync(UserManager<BTUser> userManager)
        {
            var defaultUser = new BTUser
            {
                UserName = "Admin@mailinator.com",
                Email = "Admin@mailinator.com",
                FirstName = "Michael",
                LastName = "Scott",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "P@ssword1");
                    await userManager.AddToRoleAsync(defaultUser, "Administrator");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("____________________________________");
                Debug.WriteLine("Error Seeding Default Admin User");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("____________________________________");
                throw;
            }

            defaultUser = new BTUser
            {
                UserName = "ProjectManager@mailinator.com",
                Email = "ProjectManager@mailinator.com",
                FirstName = "Dwight",
                LastName = "Schrute",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "P@ssword1");
                    await userManager.AddToRoleAsync(defaultUser, "Project Manager");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("____________________________________");
                Debug.WriteLine("Error Seeding Default Project Manager User");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("____________________________________");
                throw;
            }

            defaultUser = new BTUser
            {
                UserName = "Dev@mailinator.com",
                Email = "Dev@mailinator.com",
                FirstName = "Jim",
                LastName = "Halpert",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "P@ssword1");
                    await userManager.AddToRoleAsync(defaultUser, "Developer");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("____________________________________");
                Debug.WriteLine("Error Seeding Default Developer User");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("____________________________________");
                throw;
            }

            defaultUser = new BTUser
            {
                UserName = "Submitter@mailinator.com",
                Email = "Submitter@mailinator.com",
                FirstName = "Pam",
                LastName = "Beesly",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "P@ssword1");
                    await userManager.AddToRoleAsync(defaultUser, "Submitter");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("____________________________________");
                Debug.WriteLine("Error Seeding Default Submitter User");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("____________________________________");
                throw;
            }            

            //Demo Users
            string demoPassword = "456Pickup$tix";

            defaultUser = new BTUser
            {
                UserName = "DemoAdmin@mailinator.com",
                Email = "DemoAdmin@mailinator.com",
                FirstName = "Michael",
                LastName = "Scott",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, demoPassword);
                    await userManager.AddToRoleAsync(defaultUser, "Administrator");
                    await userManager.AddToRoleAsync(defaultUser, "Demo User");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("____________________________________");
                Debug.WriteLine("Error Seeding Demo Admin User");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("____________________________________");
                throw;
            }

            defaultUser = new BTUser
            {
                UserName = "DemoPM@mailinator.com",
                Email = "DemoPM@mailinator.com",
                FirstName = "Dwight",
                LastName = "Schrute",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, demoPassword);
                    await userManager.AddToRoleAsync(defaultUser, "Project Manager");
                    await userManager.AddToRoleAsync(defaultUser, "Demo User");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("____________________________________");
                Debug.WriteLine("Error Seeding Demo Project Manager User");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("____________________________________");
                throw;
            }

            defaultUser = new BTUser
            {
                UserName = "DemoDev@mailinator.com",
                Email = "DemoDev@mailinator.com",
                FirstName = "Jim",
                LastName = "Halpert",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, demoPassword);
                    await userManager.AddToRoleAsync(defaultUser, "Developer");
                    await userManager.AddToRoleAsync(defaultUser, "Demo User");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("____________________________________");
                Debug.WriteLine("Error Seeding Demo Developer User");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("____________________________________");
                throw;
            }

            defaultUser = new BTUser
            {
                UserName = "DemoSubmitter@mailinator.com",
                Email = "DemoSubmitter@mailinator.com",
                FirstName = "Pam",
                LastName = "Beesly",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, demoPassword);
                    await userManager.AddToRoleAsync(defaultUser, "Submitter");
                    await userManager.AddToRoleAsync(defaultUser, "Demo User");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("____________________________________");
                Debug.WriteLine("Error Seeding Demo Submitter User");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("____________________________________");
                throw;
            }
        }

        private static async Task SeedProjectsAsync(ApplicationDbContext context)
        {
            List<Project> projects = new List<Project>();
            Project seedProject1 = new Project
            {
                Id = 1,
                Name = "Bug Tracker"
            };
            try
            {
                var project = await context.Projects.FirstOrDefaultAsync(p => p.Name == "Bug Tracker");
                if (project == null)
                {
                    await context.Projects.AddAsync(seedProject1);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("____________________________________");
                Debug.WriteLine("Error Seeding Default Project 1");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("____________________________________");
                throw;
            };

            Project seedProject2 = new Project
            {
                Id = 2,
                Name = "Blog"
            };
            try
            {
                var project = await context.Projects.FirstOrDefaultAsync(p => p.Name == "Blog");
                if (project == null)
                {
                    await context.Projects.AddAsync(seedProject2);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("____________________________________");
                Debug.WriteLine("Error Seeding Default Project 2");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("____________________________________");
                throw;
            };

            Project seedProject3 = new Project
            {
                Id = 3,
                Name = "Financial Portal"
            };
            try
            {
                var project = await context.Projects.FirstOrDefaultAsync(p => p.Name == "Financial Portal");
                if (project == null)
                {
                    await context.Projects.AddAsync(seedProject3);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("____________________________________");
                Debug.WriteLine("Error Seeding Default Project 3");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("____________________________________");
                throw;
            };
        }

        private static async Task SeedProjectUsersAsync(ApplicationDbContext context, UserManager<BTUser> userManager)
        {
            string adminId = (await userManager.FindByEmailAsync("Admin@mailinator.com")).Id;
            string pManagerId = (await userManager.FindByEmailAsync("ProjectManager@mailinator.com")).Id;
            string devId = (await userManager.FindByEmailAsync("Dev@mailinator.com")).Id;
            string submitterId = (await userManager.FindByEmailAsync("Submitter@mailinator.com")).Id;
            int project1Id = (await context.Projects.FirstOrDefaultAsync(p => p.Name == "Bug Tracker")).Id;
            int project2Id = (await context.Projects.FirstOrDefaultAsync(p => p.Name == "Blog")).Id;
            int project3Id = (await context.Projects.FirstOrDefaultAsync(p => p.Name == "Financial Portal")).Id;
            ProjectUser projectUser = new ProjectUser
            {
                UserId = adminId,
                ProjectId = project1Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == adminId && r.ProjectId == project1Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("____________________________________");
                Debug.WriteLine("Error Seeding Admin to Project 1");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("____________________________________");
                throw;
            };
            projectUser = new ProjectUser
            {
                UserId = adminId,
                ProjectId = project2Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == adminId && r.ProjectId == project2Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("____________________________________");
                Debug.WriteLine("Error Seeding Admin to Project 2");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("____________________________________");
                throw;
            };
            projectUser = new ProjectUser
            {
                UserId = adminId,
                ProjectId = project3Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == adminId && r.ProjectId == project3Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("____________________________________");
                Debug.WriteLine("Error Seeding Admin to Project 3");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("____________________________________");
                throw;
            };
            projectUser = new ProjectUser
            {
                UserId = pManagerId,
                ProjectId = project2Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == pManagerId && r.ProjectId == project2Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("____________________________________");
                Debug.WriteLine("Error Seeding Project Manager to Project 1");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("____________________________________");
                throw;
            };
            projectUser = new ProjectUser
            {
                UserId = pManagerId,
                ProjectId = project3Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == pManagerId && r.ProjectId == project3Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("____________________________________");
                Debug.WriteLine("Error Seeding Project Manager to Project 1");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("____________________________________");
                throw;
            };
            projectUser = new ProjectUser
            {
                UserId = devId,
                ProjectId = project1Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == devId && r.ProjectId == project1Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("____________________________________");
                Debug.WriteLine("Error Seeding Project Manager to Project 1");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("____________________________________");
                throw;
            };
            projectUser = new ProjectUser
            {
                UserId = devId,
                ProjectId = project3Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == devId && r.ProjectId == project3Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("____________________________________");
                Debug.WriteLine("Error Seeding Project Manager to Project 1");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("____________________________________");
                throw;
            };
            projectUser = new ProjectUser
            {
                UserId = submitterId,
                ProjectId = project1Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == submitterId && r.ProjectId == project1Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("____________________________________");
                Debug.WriteLine("Error Seeding Project Manager to Project 1");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("____________________________________");
                throw;
            };
            projectUser = new ProjectUser
            {
                UserId = submitterId,
                ProjectId = project2Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == submitterId && r.ProjectId == project2Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("____________________________________");
                Debug.WriteLine("Error Seeding Project Manager to Project 1");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("____________________________________");
                throw;
            };
        }

        private static async Task SeedTicketsAsync(ApplicationDbContext context, UserManager<BTUser> userManager)
        {
            string adminId = (await userManager.FindByEmailAsync("Admin@mailinator.com")).Id;
            string pManagerId = (await userManager.FindByEmailAsync("ProjectManager@mailinator.com")).Id;
            string devId = (await userManager.FindByEmailAsync("Dev@mailinator.com")).Id;
            string submitterId = (await userManager.FindByEmailAsync("Submitter@mailinator.com")).Id;
            int project1Id = (await context.Projects.FirstOrDefaultAsync(p => p.Name == "Bug Tracker")).Id;
            int project2Id = (await context.Projects.FirstOrDefaultAsync(p => p.Name == "Blog")).Id;
            int project3Id = (await context.Projects.FirstOrDefaultAsync(p => p.Name == "Financial Portal")).Id;
            int statusId = (await context.TicketStatuses.FirstOrDefaultAsync(ts => ts.Name == "Open")).Id;
            int typeId = (await context.TicketTypes.FirstOrDefaultAsync(ts => ts.Name == "BackEnd")).Id;
            int priorityId = (await context.TicketPriorities.FirstOrDefaultAsync(ts => ts.Name == "High")).Id;

            Ticket ticket = new Ticket
            {
                Title = "The Bug Tracker needs more bugs to track",
                Description = "We're not having enough issues currently, we should look into generating more",
                Created = DateTimeOffset.Now.AddDays(-7),
                Updated = DateTimeOffset.Now.AddHours(-30),
                ProjectId = project1Id,
                TicketPriorityId = priorityId,
                TicketTypeId = typeId,
                TicketStatusId = statusId,
                DeveloperUserId = devId,
                OwnerUserId = submitterId
            };
        }
    }
}
