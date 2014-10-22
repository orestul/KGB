using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Photofolio.Models
{
    public class UserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }

        private static PhotofolioEntities db = new PhotofolioEntities();
        public static void AddUser(UserModel user)
        {
            User newUser = new User();
            newUser.Username = user.Username;
            newUser.Password = user.Password;
            newUser.Email = user.Email;
            db.Users.Add(newUser);
            db.SaveChanges();
        }
        public static User Login(UserModel user)
        {
            User dbUser = db.Users.Find(new object[] { user.Username });
            if (dbUser != null && dbUser.Password == user.Password)
            {
                return dbUser;
            }
            else
                return null;
        }

    }
}