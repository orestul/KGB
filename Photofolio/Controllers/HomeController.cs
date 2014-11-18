﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using Login.Models;
using Microsoft.AspNet.Identity;
namespace Login.Controllers
{
    public class HomeController : Controller
    {
        private Entities db = new Entities();
        public ActionResult Index()
        {   string user = User.Identity.GetUserName();
            var querytwo = (from u in db.Uploads
                            where u.rating >= 3
                            select u);
            return View(querytwo.ToList());
        }

        public ActionResult Gallery()
        {
            return View(db.Uploads.ToList());
        }


        public ActionResult UploadArt()
        {
            return View();
        }

        public ActionResult UploadPhoto()
        {
            return View();
        }

        public ActionResult SearchUser(string username)
        {
            
            var query = (from u in db.AspNetUsers
                         where u.UserName.StartsWith(username)
                         select u);
            return View(query.ToList());
        }

        public ActionResult SearchPhoto(string description)
        {

            var query = (from u in db.Uploads
                         where u.username.Contains(description)
                         select u);
            return View(query.ToList());
        }


        [HttpPost]
        public ActionResult UploadPhoto(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Server.MapPath("~/Images/Uploads/" + Session["userName"] + "/Photos");
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
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UploadArt(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Server.MapPath("~/Images/Uploads/" + Session["userName"] + "/Art");
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
            }

            return RedirectToAction("Index");
        }
    }
}