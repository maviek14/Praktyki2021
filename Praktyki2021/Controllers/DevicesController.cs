using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Praktyki2021.DAL;
using Praktyki2021.Models;

namespace Praktyki2021.Controllers
{
    public class DevicesController : Controller
    {
        private Praktyki2021Context db = new Praktyki2021Context();

        // GET: Devices
        [Authorize(Roles = "User, Admin")]
        public ActionResult Index()
        {
            return View(db.Devices.Include(c => c.Profile).ToList());
        }

        // GET: Devices/Details/5
        [Authorize(Roles = "User, Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        // GET: Devices/Create
        [Authorize(Roles = "User, Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Devices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Type,Name,Description")] Device device)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["pictureFile"];
                if (file != null && file.ContentLength > 0)
                {
                    device.PictureName = System.Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    //device.PictureName = file.FileName;
                    file.SaveAs(HttpContext.Server.MapPath("~/Pictures/") + device.PictureName);
                }

                device.Profile = db.Profiles.Single(p => p.UserName.Equals(User.Identity.Name));
                db.Devices.Add(device);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(device);
        }

        // GET: Devices/Edit/5
        [Authorize(Roles = "User, Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        // POST: Devices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Type,Name,Description")] Device device)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["pictureFile"];
                if (file != null && file.ContentLength > 0)
                {
                    device.PictureName = System.Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    //device.PictureName = file.FileName;
                    file.SaveAs(HttpContext.Server.MapPath("~/Pictures/") + device.PictureName);
                }

                db.Entry(device).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(device);
        }

        // GET: Devices/Delete/5
        [Authorize(Roles = "User, Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        // POST: Devices/Delete/5
        [Authorize(Roles = "User, Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Device device = db.Devices.Find(id);
            db.Devices.Remove(device);
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
