﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    public class TicketPrioritiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketPriorities
        public ActionResult Index()
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            return View(db.TicketPriorities.ToList());
        }

        // GET: TicketPriorities/Details/5
        public ActionResult Details(int? id)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketPriority ticketPriority = db.TicketPriorities.Find(id);
            if (ticketPriority == null)
            {
                return HttpNotFound();
            }
            return View(ticketPriority);
        }

        // GET: TicketPriorities/Create
        public ActionResult Create()
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            return View();
        }

        // POST: TicketPriorities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] TicketPriority ticketPriority)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                db.TicketPriorities.Add(ticketPriority);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ticketPriority);
        }

        // GET: TicketPriorities/Edit/5
        public ActionResult Edit(int? id)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketPriority ticketPriority = db.TicketPriorities.Find(id);
            if (ticketPriority == null)
            {
                return HttpNotFound();
            }
            return View(ticketPriority);
        }

        // POST: TicketPriorities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] TicketPriority ticketPriority)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                db.Entry(ticketPriority).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ticketPriority);
        }

        // GET: TicketPriorities/Delete/5
        public ActionResult Delete(int? id)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketPriority ticketPriority = db.TicketPriorities.Find(id);
            if (ticketPriority == null)
            {
                return HttpNotFound();
            }
            return View(ticketPriority);
        }

        // POST: TicketPriorities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            TicketPriority ticketPriority = db.TicketPriorities.Find(id);
            db.TicketPriorities.Remove(ticketPriority);
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