using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rhHouseholdBudgeter.Models
{
    public class Budget
    {
        //PROPERTIES
        public int Id { get; set; }
        public int HouseHoldId { get; set; }
        public string OwnerId { get; set; }
        public DateTime Created { get; set; }
        public string BudgetName { get; set; }
        public float TargetAmount { get; set; }
        public float CurrentAmount { get; set; }
        //KEYS
        public virtual ApplicationUser Owner { get; set; }
        public virtual Household Household { get; set; }
        //CHILDREN
        public virtual ICollection<BudgetItem> BudgetItems { get; set; }
        public Budget()
        {
            BudgetItems = new HashSet<BudgetItem>();
        }
    }
}