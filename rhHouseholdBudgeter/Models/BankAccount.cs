using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using rhHouseholdBudgeter.Enum;

namespace rhHouseholdBudgeter.Models
{
    public class BankAccount
    {
        //PROPERTIES
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        //public string AccountType { get; set; }
        public float StartingBalance { get; set; }
        public float lowLevelBalance { get; set; }
        public float CurrentBalance { get; set; }
        public int HouseholdId { get; set; }
        public string OwnerId { get; set; }
        //KEYS 
        public virtual Household Household { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public virtual AccountType AccountType { get; set; }

        //CHILDREN
        public virtual ICollection<Transaction> Transactions { get; set; }
      
        //CONSTRUCTOR
        public BankAccount()
        {
            Transactions = new HashSet<Transaction>();
        }
    }
}