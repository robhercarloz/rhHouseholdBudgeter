using Microsoft.AspNet.Identity;
using rhHouseholdBudgeter.Helpers;
using rhHouseholdBudgeter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace rhHouseholdBudgeter.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper roleHelper = new UserRoleHelper();

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);


            var dash = new DashboardViewModel();

            dash.FullName = $"{user.FirstName} {user.LastName}";
            dash.Email = user.Email;
            dash.DisplayName = user.DisplayName;
            


            return View(dash);
        }

        //GET
        public ActionResult EditProfile(string Id)
        {
            //var userId = User.Identity.GetUserId();
            //Get specific user ID 
            var user = db.Users.Find(Id);
            //Creating view model with info of user
            var profile = new UserInformationViewModel();
            //assigning the value of properties
            profile.Fname = user.FirstName;
            profile.Lname = user.LastName;
            profile.DisplayName = user.DisplayName;
            profile.Email = user.Email;
            profile.AvatarPath = user.AvatarPath;
            profile.Role = roleHelper.ListUserRole(user.Id).FirstOrDefault();
            //profile.Password = user.PasswordHash.ToString();
            //all info passed in as user
            return View(profile);            
        }
        //POST
        [HttpPost]
        public ActionResult EditProfile(UserInformationViewModel model, HttpPostedFileBase avatar)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            user.FirstName = model.Fname;
            user.LastName = model.Lname;
            user.DisplayName = model.DisplayName;
            user.Email = model.Email;
            user.UserName = model.Email;
            //db.Entry(editUser);

            if (avatar != null)
            {
                if (ImageUploadValidator.IsWebFriendlyImage(avatar))
                {
                    var filename = Path.GetFileName(avatar.FileName);
                    avatar.SaveAs(Path.Combine(Server.MapPath("~/Avatars/"), filename));
                    user.AvatarPath = "/Avatars/" + filename;
                }
            }

            db.SaveChanges();
            return RedirectToAction("EditProfile", "Home");
        }

        //GET
        public ActionResult myProfile(string Id)
        {
            //var userId = User.Identity.GetUserId();
            //Get specific user ID 
            var user = db.Users.Find(Id);
            //Creating view model with info of user
            var profile = new UserInformationViewModel();
            //assigning the value of properties
            profile.Fname = user.FirstName;
            profile.Lname = user.LastName;
            profile.DisplayName = user.DisplayName;
            profile.Email = user.Email;
            profile.AvatarPath = user.AvatarPath;
            profile.Role = roleHelper.ListUserRole(user.Id).FirstOrDefault();
            //profile.Password = user.PasswordHash.ToString();
            //all info passed in as user
            return View(profile);
            
        }
    }
}