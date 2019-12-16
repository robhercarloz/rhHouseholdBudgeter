using rhHouseholdBudgeter.Enum;
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

        public static void UpdateBalances(this Transaction transaction)
        {
            UpdateBalances(transaction);
            UpdateBalances(transaction);
            UpdateBalances(transaction);            
        }

        public static void ManageNotifications(this Transaction transaction)
        {
            var currentBal = transaction.BankAccount.CurrentBalance;

            if (currentBal < 0)
                //SendOverDraftNotification(transaction);
                var body = $"Your most recent transaction in the amount of {transaction.Amount} has overdrafted account {transaction.BankAccount.Name} leaving you with a currrent balance of {transaction.BankAccount.CurrentBalance}" 
                CreateNotification(transaction, "You over drafted your account", body);


            else if (currentBal > 0 && currentBal <= transaction.BankAccount.lowLevelBalance)
                //SendOverDraftNotification(transaction);

                var body = $"Your most recent transaction in the amount of {transaction.Amount} has overdrafted account {transaction.BankAccount.Name} leaving you with a currrent balance of {transaction.BankAccount.CurrentBalance}"
                CreateNotification(transaction, "You over drafted your account", body);
        }

        private static void CreateNotification(Transaction transaction, string body, string subject)
        {
            var notification = new Notification
            {
                Body = body,
                Created = DateTime.Now,
                HouseholdId = transaction.BankAccount.HouseholdId,
                IsRead = false,
                Subject = subject
            };
            db.Notifications.Add(notification);
        }



        public static void SendOverDraftNotification(Transaction transaction)
        {
            
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

            if (transaction.TransactionType == TransactionType.Deposit || transaction.BudgetItemId == null)
                return;

            var budgetId = db.BudgetItems.Find(transaction.BudgetItemId).BudgetId;
            var budget = db.Budgets.Find(budgetId);
            budget.CurrentAmount += transaction.Amount;
            db.SaveChanges();            
        }
               

        private static void UpdateBudgetItemBalance(Transaction transaction)
        {
            if (transaction.TransactionType == TransactionType.Deposit || transaction.BudgetItemId == null)
                return;

            var budgetItem = db.BudgetItems.Find(transaction.BudgetItemId);
            budgetItem.CurrentAmount += transaction.Amount;
            db.SaveChanges();
        }



    }
}