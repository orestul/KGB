using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Login.Models;
using System.IO;
using Microsoft.AspNet.Identity;
namespace Login.Controllers
{
    public class UploadsController : Controller
    {
        private Entities db = new Entities();

        // GET: Uploads
        public ActionResult Index()
        {
            return View(db.Uploads.ToList());
        }

        // GET: Uploads/Details/5
        public ActionResult Details(int? id)
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

        // GET: Uploads/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Uploads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "category")] Upload upload, HttpPostedFileBase file, string category)
        {
            var path = "";
            //[Bind(Include = "UploadID,category,location,username")]
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                path = Server.MapPath("~/Images/Uploads/" + User.Identity.GetUserName() + "/" + category);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = Path.Combine(path, fileName);
                if (System.IO.File.Exists(path))
                {
                    return RedirectToAction("Error");
                }
                file.SaveAs(path);
                upload.username = User.Identity.GetUserName();
                upload.location = "Images/Uploads/" + User.Identity.GetUserName() + "/" + category + "/" + fileName;
                upload.category = category;
            }

            if (ModelState.IsValid)
            {
                db.Uploads.Add(upload);
                db.SaveChanges();
                return RedirectToAction("Index");
            }



            return View(upload);
        }

        // GET: Uploads/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Uploads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UploadID,category,location,username")] Upload upload)
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
        public ActionResult Delete(int? id)
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Upload upload = db.Uploads.Find(id);
            db.Uploads.Remove(upload);
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
