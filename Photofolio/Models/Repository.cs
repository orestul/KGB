using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Login.Models
{
    public class Repository
    {
        private Entities db = new Entities();
        public List<Login.Models.AspNetUser> getTopMembers()
        {
            var query = (from x in db.AspNetUsers
                         where x.AvgRating >= 3
                         select x);

            return query.OrderByDescending(p => p.AvgRating).ToList();
        }
    }
}