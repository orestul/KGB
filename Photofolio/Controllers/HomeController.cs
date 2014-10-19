using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Photofolio.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "PhotoFolio";
            return View();
        }

        public ActionResult Error()
        {

            return View();
        }

        public ActionResult Gallery()
        {
            return View();
        }


        public ActionResult UploadArt()
        {
            return View();
        }

        public ActionResult UploadPhoto()
        {
            return View();
        }

        public ActionResult MyAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/Uploads"), fileName);
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