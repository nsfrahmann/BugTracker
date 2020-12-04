using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugTracker.Data;
using BugTracker.Models;
using BugTracker.Models.ViewModels;
using BugTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTRolesService _rolesService;
        private readonly UserManager<BTUser> _userManager;
        private readonly IBTAttachmentsService _attachmentsService;

        public UsersController(ApplicationDbContext context, IBTRolesService rolesService, UserManager<BTUser> userManager, IBTAttachmentsService attachmentsService)
        {
            _context = context;
            _rolesService = rolesService;
            _userManager = userManager;
            _attachmentsService = attachmentsService;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> ManageUserRoles()
        {
            List<ManageUserRolesViewModel> model = new List<ManageUserRolesViewModel>();
            List<BTUser> users = _context.Users.ToList();

            foreach (var user in users)
            {
                ManageUserRolesViewModel vm = new ManageUserRolesViewModel();
                vm.User = user;
                var selected = await _rolesService.ListUserRoles(user);
                vm.Roles = new MultiSelectList(_context.Roles, "Name", "Name", selected);
                model.Add(vm);
            }

            return View(model);
        }

        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Assign(ManageUserRolesViewModel btuser)
        //{
        //    BTUser user = await _context.Users.FindAsync(btuser.User.Id);
        //    IEnumerable<string> roles = await _rolesService.ListUserRoles(user);
        //    await _userManager.RemoveFromRolesAsync(user, roles);
        //    string userRole = btuser.SelectedRoles.FirstOrDefault();

        //    if (Enum.TryParse(userRole, out Roles roleValue))
        //    {
        //        await _rolesService.AddUserToRole(user, userRole);
        //        return RedirectToAction("ManageUserRoles");
        //    }

        //    return RedirectToAction("ManageUserRoles");
        //}

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(List<string> UsersSelected, string RoleSelected)
        {
            if (ModelState.IsValid)
            {
                foreach (var userId in UsersSelected)
                {
                    var btuser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                    IEnumerable<string> roles = await _rolesService.ListUserRoles(btuser);
                    await _userManager.RemoveFromRolesAsync(btuser, roles);
                    await _rolesService.AddUserToRole(btuser, RoleSelected);
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Home", "Dashboard");
        }

        [Authorize]
        public async Task<IActionResult> Profile(string userId, bool click)
        {
            if (click == true)
            {
                var profile = await _context.Profiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.UserId == userId);

                return View(profile);
            }

            var thisUser = _userManager.GetUserId(User);
            var profile2 = await _context.Profiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.UserId == thisUser);

            return View(profile2);
        }

        [Authorize]
        [HttpPost, ActionName("Profile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileCreate([Bind("UserId,Created")] string userId, string bio, IFormFile Avatar, IFormFile BannerImage)
        {
            if (bio.ToCharArray().Length > 3000)
            {
                TempData["StopIt"] = "You're either writing out your entire life story or trying to break the application. Either way please be more brief. Sorry...";
                var userProfile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
                var myId = userProfile.UserId;
                return RedirectToAction("Profile", "Users", new { Id = myId });
            }

            if (ModelState.IsValid)
            {
                userId = _userManager.GetUserId(User);
                var thisUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                var userProfile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);

                userProfile.Updated = DateTime.Now;
                userProfile.Bio = bio;

                if (Avatar != null)
                {
                    userProfile.AvatarContentType = Avatar.ContentType;
                    userProfile.AvatarData = await _attachmentsService.EncodeAttachmentAsync(Avatar);
                    userProfile.AvatarName = Avatar.FileName;
                }
                else
                {
                    userProfile.AvatarContentType = userProfile.AvatarContentType;
                    userProfile.AvatarData = userProfile.AvatarData;
                    userProfile.AvatarName = userProfile.AvatarName;
                }

                if (BannerImage != null)
                {
                    userProfile.BannerContentType = BannerImage.ContentType;
                    userProfile.BannerData = await _attachmentsService.EncodeAttachmentAsync(BannerImage);
                    userProfile.BannerName = BannerImage.FileName;
                }
                else
                {
                    userProfile.BannerContentType = userProfile.BannerContentType;
                    userProfile.BannerData = userProfile.BannerData;
                    userProfile.BannerName = userProfile.BannerName;
                }

                _context.Update(userProfile);

                if (userProfile.AvatarData != null)
                {
                    thisUser.AvatarContentType = userProfile.AvatarContentType;
                    thisUser.AvatarData = userProfile.AvatarData;
                    thisUser.AvatarName = userProfile.AvatarName;
                }

                _context.Update(thisUser);

                await _context.SaveChangesAsync();
                var myId = userProfile.UserId;
                return RedirectToAction("Profile", "Users", new { Id = myId });
            }
            return View();
        }
    }
}
