using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Login.Models;
using Microsoft.AspNet.Identity;
namespace Login.Controllers
{
    public class RatingsController : Controller
    {
        private Entities db = new Entities();

        // GET: Ratings
        public ActionResult Index()
        {
            var ratings = db.Ratings.Include(r => r.Upload);
            return View(ratings.ToList());
        }

        // GET: Ratings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // GET: Ratings/Create
        public ActionResult Create()
        {
            ViewBag.UploadID = new SelectList(db.Uploads, "UploadID", "category");
            return View();
        }

        // POST: Ratings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RatingID,RatingValue,UploadID")] Rating rating)
        {
            rating.Username = User.Identity.GetUserName();
            string user = User.Identity.GetUserName();
            var querytwo = (from r in db.Ratings
                            where r.Username == user
                            && r.UploadID == rating.UploadID
                            select r);
            if (querytwo.ToList().Count == 0)
            {
                if (ModelState.IsValid)
                {
                    db.Ratings.Add(rating);
                    db.SaveChanges();
                    float count = 0;
                    float resultCount = 0;
                    float avg = 0;
                    var query = (from r in db.Ratings
                                 where r.UploadID == rating.UploadID
                                 select r);
                    foreach (Rating item in query.ToList())
                    {
                        count += item.RatingValue;
                        resultCount++;
                    }
                    avg = count / resultCount;
                    Upload upload = (from u in db.Uploads
                                     where u.UploadID == rating.UploadID
                                     select u).FirstOrDefault();

                    upload.rating = avg;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }
            else
            {
                return RedirectToAction("Index");
            }
            ViewBag.UploadID = new SelectList(db.Uploads, "UploadID", "category", rating.UploadID);
            return View(rating);
        }

        // GET: Ratings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            ViewBag.UploadID = new SelectList(db.Uploads, "UploadID", "category", rating.UploadID);
            return View(rating);
        }

        // POST: Ratings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RatingID,RatingValue,Username,UploadID")] Rating rating)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UploadID = new SelectList(db.Uploads, "UploadID", "category", rating.UploadID);
            return View(rating);
        }

        // GET: Ratings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rating rating = db.Ratings.Find(id);
            db.Ratings.Remove(rating);
            db.SaveChanges();
            return RedirectToAction("Index");
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
