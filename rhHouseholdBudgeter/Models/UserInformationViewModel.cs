using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rhHouseholdBudgeter.Models
{
    public class UserInformationViewModel
    {
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string DisplayName { get; set; }
        public string AvatarPath { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

    }
}