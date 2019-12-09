using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using rhHouseholdBudgeter.Models;
using rhHouseholdBudgeter.Helpers;


namespace rhHouseholdBudgeter.Controllers
{
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper roleHelper = new UserRoleHelper();



        // GET: Households
        public ActionResult Index()
        {
            return View(db.Households.ToList());
        }

        // GET: Households/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // GET: Households/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Greeting,Created")] Household household)
        {
            if (ModelState.IsValid)
            {

                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                //unassign
                foreach(var userRole in roleHelper.ListUserRole(userId))
                {
                    roleHelper.RemoveUserFromRole(userId, userRole);
                }
                //assign
                roleHelper.AddUserToRole(userId, "HouseholdOwner");
                

                household.Created = DateTime.Now;                
                db.Households.Add(household);
                //gives new household ID 
                user.HouseholdId = household.Id;                

                db.SaveChanges();
                //log out and back in to update
                await HttpContext.RefreshAuthentication(user);

                return RedirectToAction("Index", "Home");
            }

            return View(household);
        }

        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Greeting,Created")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }

        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Get 
        public ActionResult AppointSuccessor()
        {
            var userId = User.Identity.GetUserId();
            var myHouseholdId = db.Users.Find(userId).HouseholdId ?? 0;

            if (myHouseholdId == 0)
                return RedirectToAction("Index");

            var members = db.Users.Where(u => u.HouseholdId == myHouseholdId && u.Id != userId);
            ViewBag.NewHOh = new SelectList(members, "Id", "FullName");

            return View();
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AppointSuccessorAsync(string newHOh)
        {
            if (string.IsNullOrEmpty(newHOh))
                return RedirectToAction("Index", "Home");

            var me = db.Users.Find(User.Identity.GetUserId());
            me.HouseholdId = null;
            db.SaveChanges();

            roleHelper.RemoveUserFromRole(me.Id, "HouseholdOwner");
            await ControllerContext.HttpContext.RefreshAuthentication(me);

            roleHelper.RemoveUserFromRole(newHOh, "HouseholdMember");
            roleHelper.AddUserToRole(newHOh, "HouseholdOwner");

            //Add a new notification record
            roleHelper.notifyNewHead(newHOh, "HouseOwner");


            return RedirectToAction("Index", "Home");
        }

        //GET
        public async Task<ActionResult> LeaveAsync()
        {
            //get user
            var userId = User.Identity.GetUserId();
            //Determine the role 
            var myRole = roleHelper.ListUserRole(userId).FirstOrDefault();
            var user = db.Users.Find(userId);

            switch (myRole)
            {
                case "HouseholdOwner":
                    var inhabitants = db.Users.Where(u => u.HouseholdId == user.HouseholdId).Count();
                    if (inhabitants > 1)
                    {
                        TempData["Message"] = $"You are unable to leave the Household at this time as there are still <b>{inhabitants}<b>";
                        return RedirectToAction("ExitDenied");
                    }

                    user.HouseholdId = null;
                    db.SaveChanges();

                    roleHelper.RemoveUserFromRole(userId, "HouseholdOwner");
                    await ControllerContext.HttpContext.RefreshAuthentication(user);
                    roleHelper.AddUserToRole(userId, "Guest");

                    return RedirectToAction("Index", "Home");

                case "HouseholdMember":
                default:
                    user.HouseholdId = null;
                    db.SaveChanges();

                    roleHelper.RemoveUserFromRole(userId, "HouseholdMember");
                    await ControllerContext.HttpContext.RefreshAuthentication(user);
                    roleHelper.AddUserToRole(userId, "Guest");

                    return RedirectToAction("Index", "Home");
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
