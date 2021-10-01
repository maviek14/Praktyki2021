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

namespace Praktyki2021.Controllers
{
    public class ContractsController : Controller
    {
        private Praktyki2021Context db = new Praktyki2021Context();

        // GET: Contracts
        [Authorize(Roles = "User, Admin")]
        public ActionResult Index()
        {
            var contracts = db.Contracts.Include(c => c.Device).Include(c => c.Principal).Include(c => c.Mandatory);
            return View(contracts.ToList());
        }

        // GET: Contracts/Details/5
        [Authorize(Roles = "User, Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = db.Contracts.Find(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // GET: Contracts/Create
        [Authorize(Roles = "User")]
        public ActionResult Create()
        {
            ViewBag.DeviceID = new SelectList(db.Devices.Where(d => d.Profile.UserName.Equals(User.Identity.Name)), "ID", "Name");
            return View();
        }

        // POST: Contracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Description,DeviceID,Price")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                Profile profile = db.Profiles.Single(p => p.UserName.Equals(User.Identity.Name));
                contract.CompletedTime = null;
                contract.CreatedTime = DateTime.Now.Date;
                contract.Status = ContractStatus.Available;
                contract.Principal = profile;
                db.Contracts.Add(contract);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DeviceID = new SelectList(db.Devices.Where(d => d.Profile.UserName.Equals(User.Identity.Name)), "ID", "Name", contract.DeviceID);
            return View(contract);
        }

        // GET: Contracts/Edit/5
        [Authorize(Roles = "User, Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = db.Contracts.Find(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeviceID = new SelectList(db.Devices.Where(d => d.Profile.UserName.Equals(User.Identity.Name)), "ID", "Name", contract.DeviceID);
            return View(contract);
        }

        // POST: Contracts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,DeviceID,Price")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                db.Contracts.Single(c => c.ID == contract.ID).Name = contract.Name;
                db.Contracts.Single(c => c.ID == contract.ID).Description = contract.Description;
                db.Contracts.Single(c => c.ID == contract.ID).DeviceID = contract.DeviceID;
                db.Contracts.Single(c => c.ID == contract.ID).Price = contract.Price;
                //db.Entry(contract).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DeviceID = new SelectList(db.Devices.Where(d => d.Profile.UserName.Equals(User.Identity.Name)), "ID", "Name", contract.DeviceID);
            return View(contract);
        }

        // GET: Contracts/Delete/5
        [Authorize(Roles = "User, Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = db.Contracts.Find(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // POST: Contracts/Delete/5
        [Authorize(Roles = "User, Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contract contract = db.Contracts.Find(id);
            db.Contracts.Remove(contract);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Contract/Take/5
        [Authorize(Roles = "User")]
        public ActionResult Take(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.Contracts.Find(id).Mandatory = db.Profiles.Single(p => p.UserName.Equals(User.Identity.Name));
            db.Contracts.Find(id).Status = ContractStatus.Taken;
            db.SaveChanges();
            MailService.SendMail(db.Contracts.Find(id).Principal.UserName,
                "Your contract has been taken",
                User.Identity.Name + " has just taken your contract named: " + db.Contracts.Find(id).Name);
            return RedirectToAction("Index");
        }

        // GET: Contract/Complete/5
        [Authorize(Roles = "User")]
        public ActionResult Complete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.Contracts.Find(id).Status = ContractStatus.Completed;
            db.Contracts.Find(id).CompletedTime = DateTime.Now.Date;
            db.SaveChanges();
            MailService.SendMail(db.Contracts.Find(id).Principal.UserName,
                "Your contract has been completed",
                User.Identity.Name + " has just completed your contract named: " + db.Contracts.Find(id).Name);
            return RedirectToAction("Index");
        }

        // GET: Contract/Cancel/5
        [Authorize(Roles = "User")]
        public ActionResult Cancel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                db.Contracts.Find(id).MandatoryID = null;
                db.Contracts.Find(id).Status = ContractStatus.Available;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine("Przechwycono wyjątek w meotdzie Cancel");
            }
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
