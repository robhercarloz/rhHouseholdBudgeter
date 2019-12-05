using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using rhHouseholdBudgeter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rhHouseholdBudgeter.Helpers
{
    public class UserRoleHelper
    {

        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>
            (new UserStore<ApplicationUser>(new ApplicationDbContext()));

        private ApplicationDbContext db = new ApplicationDbContext();


        //list user role
        public ICollection<string> ListUserRole(string userId)
        {
            return userManager.GetRoles(userId);
        }

        public bool AddUserToRole(string userId, string roleName)
        {
            var result = userManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }

        public bool RemoveUserFromRole(string userId, string RoleName)
        {
            var result = userManager.RemoveFromRole(userId, RoleName);
            return result.Succeeded;
        }

        public static List<Notification> SendNewRoleNotification (string user, string UserRole)
        {
            var notification = new Notification
            {
                Created = DateTime.Now,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                IsRead = false,
                Body = $"You have been assigned to a new role"
            };


            

        }

        

    }
}