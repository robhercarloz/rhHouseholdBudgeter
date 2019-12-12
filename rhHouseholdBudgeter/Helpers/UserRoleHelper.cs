using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using rhHouseholdBudgeter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using rhHouseholdBudgeter.Enum;

namespace rhHouseholdBudgeter.Helpers
{
    public class UserRoleHelper
    {

        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>
            (new UserStore<ApplicationUser>(new ApplicationDbContext()));

        private static ApplicationDbContext db = new ApplicationDbContext();


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

        public void notifyNewHead(string user, string role)
        {
            var notification = new Notification
            {
                Created = DateTime.Now,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                Body = $"You are now the {role}, Good Luck!",
                RecipientId = user                
            };
            
            //send the email
            db.Notifications.Add(notification);
            db.SaveChanges();
            

        }

        public ICollection<ApplicationUser> ListUsersOnHouse(int? houseId)
        {
            //make list 
            var users = new List<ApplicationUser>();
            var list = db.Users.Where(u => u.HouseholdId == houseId).ToList();
            foreach(var user in list)
            {
                users.Add(user);
            }
            return (users);
            
            

        }


    }
}