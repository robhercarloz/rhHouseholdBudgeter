using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rhHouseholdBudgeter.Models
{
    public class Household
    {
        //PROPERTIES
        public int Id { get; set; }

        public string Name { get; set; }

        public string Greeting { get; set; }
        public DateTime Created { get; set; }
        //CHILDREN
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        //CONSTRUCTOR
        public Household()
        {
            BankAccounts = new HashSet<BankAccount>();
            Budgets = new HashSet<Budget>();
            Notifications = new HashSet<Notification>();
            Invitations = new HashSet<Invitation>();
            Users = new HashSet<ApplicationUser>();
        }
    }
}