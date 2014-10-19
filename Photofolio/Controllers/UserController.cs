using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using Photofolio.Models;
using System.Web.Security;


namespace Photofolio.Controllers
{
    public class UserController : Controller
    {

                private ApplicationUserManager _userManager;

        public UserController()
        {
        }

        public UserController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(UserModel user)
        {
            if (ModelState.IsValid)
            {
                if (user.Password != null && user.Password.Length > 5)
                {
                    UserModel.AddUser(user);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Login(UserModel user)
        {
            if (ModelState.IsValid)
            {
                User login = UserModel.Login(user);
                if (login != null)
                {
                    Session["userName"] = login.Username; 
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            }
            return RedirectToAction("error", "Home");
        }

    }
}