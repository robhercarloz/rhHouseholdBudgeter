using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace rhHouseholdBudgeter.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "First name must contain 2 - 20 characters.")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [StringLength(35, MinimumLength = 2, ErrorMessage = "Last name must contain 2 - 25 characters.")]
        public string LastName { get; set; }
        [Display(Name = "Display Name")]
        [StringLength(45, MinimumLength = 2, ErrorMessage = "Display Name must be 2 - 25 characters.")]
        public string DisplayName { get; set; }
        public string AvatarPath { get; set; }
        public int? HouseholdId { get; set; }
        //KEY
        public virtual Household Household { get; set; }
        //CHILDREN
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }

        public ApplicationUser()
        {
            Notifications = new HashSet<Notification>();
            BankAccounts = new HashSet<BankAccount>();
            Transactions = new HashSet<Transaction>();
            Budgets = new HashSet<Budget>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Household> Households { get; set; }
        public DbSet<BudgetItem> BudgetItems { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

    }
}