using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System.IO;

namespace BugTracker.Controllers
{
    public class TicketNotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketNotifications
        public ActionResult Index()
        {
            var Helper = new UserNotificationsHelper();
            ViewBag.Notifications = Helper.filterNotifications(User.Identity.GetUserId());
            List<TicketNotification> listTN = new List<TicketNotification>();
            foreach (var tn in db.TicketNotifications)
            {
                if(tn.UserId == User.Identity.GetUserId()) { listTN.Add(tn);}
            }          
            return View(listTN.ToList());
        }

        // GET: TicketNotifications/Details/5
        public ActionResult Details(int? id)
        {
            var Helper = new UserNotificationsHelper();
            ViewBag.Notifications = Helper.filterNotifications(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketNotification ticketNotification = db.TicketNotifications.Find(id);
            if (ticketNotification == null)
            {
                return HttpNotFound();
            }
            return View(ticketNotification);
        }

        // GET: TicketNotifications/Create
        public ActionResult Create(int? id)
        {
            var Helper = new UserNotificationsHelper();
            ViewBag.Notifications = Helper.filterNotifications(User.Identity.GetUserId());
            TicketNotification ticketN = db.TicketNotifications.Find(id);
            return View(ticketN);
        }

        // POST: TicketNotifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TicketId,UserId,Notice,Read")] TicketNotification ticketNotification, string notice)
        {
            var Helper = new UserNotificationsHelper();
            ViewBag.Notifications = Helper.filterNotifications(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                db.TicketNotifications.Add(new TicketNotification { 
                    Notice = notice,
                    TicketId = ticketNotification.TicketId,
                    Read = false,
                    UserId = db.Tickets.Find(ticketNotification.TicketId).OwnerUser.Id
                });
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketNotification.TicketId);
            return View(ticketNotification);
        }

        // GET: TicketNotifications/Edit/5
        public ActionResult Edit(int? id)
        {
            var Helper = new UserNotificationsHelper();
            ViewBag.Notifications = Helper.filterNotifications(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketNotification ticketNotification = db.TicketNotifications.Find(id);
            if (ticketNotification == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketNotification.TicketId);
            return View(ticketNotification);
        }

        // POST: TicketNotifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,UserId,Notice,Read")] TicketNotification ticketNotification)
        {
            var Helper = new UserNotificationsHelper();
            ViewBag.Notifications = Helper.filterNotifications(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                db.Entry(ticketNotification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketNotification.TicketId);
            return View(ticketNotification);
        }

        // GET: TicketNotifications/Delete/5
        public ActionResult Delete(int? id)
        {
            var Helper = new UserNotificationsHelper();
            ViewBag.Notifications = Helper.filterNotifications(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketNotification ticketNotification = db.TicketNotifications.Find(id);
            if (ticketNotification == null)
            {
                return HttpNotFound();
            }
            return View(ticketNotification);
        }

        // POST: TicketNotifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var Helper = new UserNotificationsHelper();
            ViewBag.Notifications = Helper.filterNotifications(User.Identity.GetUserId());
            TicketNotification ticketNotification = db.TicketNotifications.Find(id);
            db.TicketNotifications.Remove(ticketNotification);
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
