using Microsoft.AspNet.Identity.Owin;
using rhHouseholdBudgeter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace rhHouseholdBudgeter.Helpers
{
    public static class InvitationExtension
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static async Task EmailInvitation(this Invitation invitation)
        {

            var Url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var callbackUrl = Url.Action("AcceptInvitation", "Account", new { recipientEmail = invitation.RecipientEmail, code = invitation.Code }, protocol: HttpContext.Current.Request.Url.Scheme);
            var from = $"RH Financial Portal<{WebConfigurationManager.AppSettings["emailfrom"]}>";

            var emailMessage = new MailMessage(from, invitation.RecipientEmail)
            {
                Subject = $"You have been invited to join the RH Financial Portal application",
                Body = $"Please accept this invitation and register as a new household member <a href=\"{callbackUrl}\">here</a>",
                IsBodyHtml = true
            };

            var svc = new PersonalEmail();
            await svc.SendAsync(emailMessage);
        }

        public static void MarkAsInvalid(int Id)
        {
            //find inv in db
            var inv = db.Invitations.Find(Id);
            //find property isValid and change to false            
            inv.IsValid = false;
            db.SaveChanges();


        }

        public static async Task RefreshAuthentication(this HttpContextBase context, ApplicationUser User)
        {
            
            //sign current user logged in out
            context.GetOwinContext().Authentication.SignOut();
            //sign back in
            await context.GetOwinContext().Get<ApplicationSignInManager>().SignInAsync(User, isPersistent: false, rememberBrowser: false);

        }
    }
}