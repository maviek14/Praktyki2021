using Praktyki2021.ViewModels;
using Praktyki2021.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Praktyki2021.Controllers
{
    public class HomeController : Controller
    {
        private Praktyki2021Context db = new Praktyki2021Context();

        [Authorize(Roles = "Admin")]
        public ActionResult About()
        {
            IQueryable<AddedDateGroup> data = from user in db.Users
                                              group user by user.AddedTime into dateGroup
                                              select new AddedDateGroup()
                                              {
                                                  AddedTime = dateGroup.Key,
                                                  UserCount = dateGroup.Count()
                                              };
            return View(data.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}