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
        Repository rep = new Repository();
        private Entities db = new Entities();
        public ActionResult Index()
        {   
            string user = User.Identity.GetUserName();
            var querytwo = (from u in db.Uploads
                            where u.rating >= 3
                            select u);
            List<int> numberOfRatings = new List<int>();
            foreach(var item in querytwo.ToList())
            {
                int ratingCount = (from r in db.Ratings
                                   where r.UploadID == item.UploadID
                                   select r).Count();
                numberOfRatings.Add(ratingCount);
            }
            var query = (from u in db.Ratings
                         where u.Username == user
                         select u);
            ViewData["NumberRatings"] = numberOfRatings;
            ViewData["PhotosRated"] = query.ToList();
            ViewData["TopMembers"] = rep.getTopMembers();
            return View(querytwo.ToList());
        }

        public ActionResult UserUploadsView(string user)
        {
            var query = (from r in db.Uploads
                         where r.username == user
                         select r);
            ViewData["User"] = user;
            return View(query.ToList());
        }

        public ActionResult SearchUser(string username)
        {

            var query = (from u in db.AspNetUsers
                         where u.UserName.Contains(username)
                         select u);
            ViewData["UserSearchKeyword"] = username;
            return View(query.ToList());
        }

        public ActionResult SearchPhoto(string description)
        {

            var query = (from u in db.Uploads
                         where u.title.Contains(description) || u.description.Contains(description)
                         select u);
            return View(query.ToList());
        }

        public ActionResult Gallery()
        {
            string user = User.Identity.GetUserName();
            var query = (from u in db.Ratings
                         where u.Username == user
                        select u);
            ViewData["PhotosRated"] = query.ToList();
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