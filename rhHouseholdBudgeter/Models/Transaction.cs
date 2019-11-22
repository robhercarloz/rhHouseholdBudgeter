using rhHouseholdBudgeter.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rhHouseholdBudgeter.Models
{
    public class Transaction
    {
        //PROPERTIES
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public float Amount { get; set; }
        public string Memo { get; set; }
        public int BankAccountId { get; set; }
        public int? BudgetItemId { get; set; }
        public string OwnerId { get; set; }

        //KEYS        
        public virtual ApplicationUser Owner { get; set; }
        public virtual TransactionType TransactionType { get; set; }
        public virtual BankAccount BankAccount { get; set; }
        public virtual BudgetItem BudgetItem { get; set; }
    }
}
