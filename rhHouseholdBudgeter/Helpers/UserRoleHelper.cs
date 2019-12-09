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

        

        //bankstuff
        public void ManageNotifications(BankAccount newAccount)
        {
            //This is checking current balance
            var lowAlert = newAccount.CurrentBalance < newAccount.lowLevelBalance;
            var negativeAlert = newAccount.CurrentBalance < 0.00; 
            //var ticketHasBeenReassigned = oldTicket.AssignedToUserId != null && newTicket.AssignedToUserId == null;
            if (lowAlert)
            {
                LowLevelAlert(newAccount);
            }
            else if (negativeAlert)
            {
                NegativeAlert(newAccount);
            }
            
        }
        
        private void LowLevelAlert (BankAccount newAccount)
        {
            
           
          
                

            

        }

        private void NegativeAlert(BankAccount newAccount)
        {

        }
        

    }
}