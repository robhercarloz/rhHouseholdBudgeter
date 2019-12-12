using rhHouseholdBudgeter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rhHouseholdBudgeter.Helpers
{
    public class NotificationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();


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

        private void LowLevelAlert(BankAccount newAccount)
        {







        }

        private void NegativeAlert(BankAccount newAccount)
        {

        }


        public void SendNewRoleNotification(string Role, string UserRole)
        {

        }

    }
}