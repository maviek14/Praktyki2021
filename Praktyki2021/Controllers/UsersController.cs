using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Praktyki2021.DAL;
using Praktyki2021.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace Praktyki2021.Controllers
{
    public class UsersController : Controller
    {
        private Praktyki2021Context db = new Praktyki2021Context();

        // GET: Users
        [Authorize(Roles = "User, Admin")]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var users = from u in db.Users select u;

            //var user = UserManager.FindById(User.Identity.GetUserId());

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    users = users.OrderBy(s => s.AddedTime);
                    break;
                case "date_desc":
                    users = users.OrderByDescending(s => s.AddedTime);
                    break;
                default:
                    users = users.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            users = users.Include(u => u.Profile);
            if (User.IsInRole("User"))
            {
                string username = User.Identity.GetUserName();
                int profileID = db.Profiles.Single(_ => _.UserName.Equals(username)).ID;
                users = users.Where(_ => _.ProfileID == profileID);
            }
            return View(users.ToPagedList(pageNumber, pageSize));
        }

        // GET: Users/Details/5
        [Authorize(Roles = "User, Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.ProfileID = new SelectList(db.Profiles, "ID", "UserName");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Surname,AddedTime,ProfileID")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProfileID = new SelectList(db.Profiles, "ID", "UserName", user.ProfileID);
            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "User, Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Surname")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Single(u => u.ID == user.ID).Name = user.Name;
                db.Users.Single(u => u.ID == user.ID).Surname = user.Surname;
                //db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProfileID = new SelectList(db.Profiles, "ID", "UserName", user.ProfileID);
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
