using Login.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Login.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private Entities db = new Entities();
        // GET: Admin
        public ActionResult Index()
        {
            var users = db.AspNetUsers.ToList();
            foreach(AspNetUser user in users.ToList())
            {
                foreach(AspNetRole role in user.AspNetRoles)
                {
                    if(role.Name == "Admin")
                    {
                        users.Remove(user);
                    }
                }
            }
            ViewData["UploadList"] = db.Uploads.ToList();
            return View(users);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser user = db.AspNetUsers.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult EditUpload(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Upload upload = db.Uploads.Find(id);
            if (upload == null)
            {
                return HttpNotFound();
            }
            return View(upload);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,"
            + "PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName, TagOne, TagTwo, TagThree")] AspNetUser user)
        {
            var context = new ApplicationDbContext();
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            if (ModelState.IsValid)
            {
                if (user.LockoutEnabled)
                {
                    
                    userManager.AddToRole(user.Id, "Banned");
                    userManager.RemoveFromRole(user.Id, "User");
                }
                else if(!user.LockoutEnabled)
                {
                    userManager.AddToRole(user.Id, "User");
                    userManager.RemoveFromRole(user.Id, "Banned");
                }
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: AspNetUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            
            //var query = (from r in db.Uploads
            //             where r.username == aspNetUser.UserName
            //             select r);
            List<Upload> uploads = db.Uploads.Where(u => u.username == aspNetUser.UserName).ToList();
            List<int> uploadIDs = new List<int>();
            foreach(var item in uploads)
            {
                uploadIDs.Add(item.UploadID);
            }
            List<Rating> ratings = db.Ratings.Where(r => r.Username == aspNetUser.UserName).ToList();

            //var querytwo = (from r in db.Ratings
            //                where r.Username == aspNetUser.UserName
            //                select r);
            foreach (var item in ratings)
            {
                if(uploadIDs.Contains(item.UploadID))
                {
                    Rating rating = db.Ratings.Find(item.UploadID);
                    db.Ratings.Remove(rating);
                }
                db.Ratings.Remove(item);
                db.SaveChanges();
            }
            foreach (var item in uploads)
            {
                db.Uploads.Remove(item);
                db.SaveChanges();
            }
            db.AspNetUsers.Remove(aspNetUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Uploads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUpload([Bind(Include = "UploadID,location, username, category,title, description")] Upload upload)
        {
            if (ModelState.IsValid)
            {
                db.Entry(upload).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(upload);
        }

        // GET: Uploads/Delete/5
        public ActionResult DeleteUpload(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Upload upload = db.Uploads.Find(id);
            if (upload == null)
            {
                return HttpNotFound();
            }
            return View(upload);
        }

        // POST: Uploads/Delete/5
        [HttpPost, ActionName("DeleteUpload")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUploadConfirmed(int id)
        {
            Upload upload = db.Uploads.Find(id);
            db.Uploads.Remove(upload);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}