using rhHouseholdBudgeter.Enum;
using rhHouseholdBudgeter.Helpers;
using rhHouseholdBudgeter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rhHouseholdBudgeter.Extensions
{
    public static class TransactionExtensions
    {
        public static ApplicationDbContext db = new ApplicationDbContext();
        public static NotificationHelper notificationHelper = new NotificationHelper();

        public static void UpdateBalances(this Transaction transaction)
        {
            UpdateBankBalance(transaction);
            UpdateBudgetBalance(transaction);
            UpdateBudgetItemBalance(transaction);            
        }

        public static void ManageNotifications(this Transaction transaction)
        {

            var bankAccount = db.BankAccounts.AsNoTracking().FirstOrDefault(b => b.Id == transaction.BankAccountId);
            var currentBal = bankAccount.CurrentBalance;

            if (currentBal < 0) 
            {
                //SendOverDraftNotification(transaction);
                //var body = $"Your most recent transaction in the amount of {transaction.Amount} has overdrafted account {transaction.BankAccount.Name} leaving you with a currrent balance of {transaction.BankAccount.CurrentBalance}";
                //CreateNotification(transaction, "You over drafted your account", body);
                //SendOverDraftNotification(transaction);
                var body = $"Your most recent transaction in the amount of {transaction.Amount} has overdrafted account {transaction.BankAccount.Name} leaving you with a currrent balance of {transaction.BankAccount.CurrentBalance}";
                CreateNotification(transaction, "You over drafted your account", body);

            }

            else if (currentBal <= transaction.BankAccount.lowLevelBalance) 
            {
                //SendOverDraftNotification(transaction);

                var body = $"Your most recent transaction in the amount of {transaction.Amount} has triggered a low balance warning account {transaction.BankAccount.Name} leaving you with a currrent balance of {transaction.BankAccount.CurrentBalance}";
                CreateNotification(transaction, "You have hit your low balance", body);
            }
        }

        private static void CreateNotification(Transaction transaction, string body, string subject)
        {


            var houseId = db.Users.Find(transaction.OwnerId).HouseholdId ?? 0;
            var notification = new Notification
            {
                Body = body,
                Created = DateTime.Now,
                HouseholdId = houseId,
                IsRead = false,
                RecipientId = transaction.OwnerId,
                Subject = subject
            };
            db.Notifications.Add(notification);
        }

        private static void UpdateBankBalance(Transaction transaction)
        {

            var bank = db.BankAccounts.Find(transaction.BankAccountId);
            if (transaction.TransactionType == TransactionType.Deposit)
                bank.CurrentBalance += transaction.Amount;
            else
                bank.CurrentBalance += transaction.Amount;

            db.SaveChanges();

        }

        private static void UpdateBudgetBalance(Transaction transaction)
        {

            var budgetItem = db.BudgetItems.Find(transaction.BudgetItemId);
            var budget = db.Budgets.Find(budgetItem.BudgetId);
            var targetamount = db.Budgets.Where(b => b.Id == budget.Id).Select(b => b.TargetAmount).Sum();
            if (transaction.TransactionType == TransactionType.Deposit || transaction.BudgetItemId == null)
            {
                return;
            }
            budget.CurrentAmount += transaction.Amount;
            if (budget.CurrentAmount > targetamount)
                //notificationHelper.SendOverBudgetNotification(transaction.OwnerId, budget.Name);
                //THis is where the notification is fired of depending on low or high level alert
            db.SaveChanges();

        }

        private static void UpdateBudgetItemBalance(Transaction transaction)
        {
            var budgetItem = db.BudgetItems.Find(transaction.BudgetItemId);
            if (transaction.TransactionType == TransactionType.Deposit || budgetItem == null)
            {
                return;
            }
            budgetItem.CurrentAmount += transaction.Amount;
            if (budgetItem.CurrentAmount > budgetItem.TargetAmount)
                //this is where the notificationhelper will fire of to see if the level is high or low
                db.SaveChanges();
        }



    }
}