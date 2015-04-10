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

namespace BugTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Projects
        [Authorize(Roles = "Project Manager")]
        public ActionResult Index()
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
        [Authorize(Roles = "Project Manager")]
        public ActionResult Details(int? id)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<SelectListItem> listOfUsersOnProject = new List<SelectListItem>();
            List<SelectListItem> listOfUsersNotOnProject = new List<SelectListItem>();
            var zHelper = new UserProjetsHelper();
            var usersOnProject = zHelper.UsersOnProject((int)id, "Developer");
            var usersNotOnProject = zHelper.UsersNotOnProject((int)id, "Developer");
            //var listOfUsersOnProject = new MultiSelectList(usersOnProject, "Id", "DisplayName");
            //var listOfUsersNotOnProject = new MultiSelectList(usersNotOnProject, "Id", "DisplayName");
            foreach (var user in usersOnProject)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.DispalyName,
                    Value = user.Id
                };
                listOfUsersOnProject.Add(selectList);
            }
            foreach (var user in usersNotOnProject)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.DispalyName,
                    Value = user.Id
                };
                listOfUsersNotOnProject.Add(selectList);
            }
            PListViewModel LViewModel = new PListViewModel()
            {
                zUsers = listOfUsersOnProject,
                otherUsers = listOfUsersNotOnProject,
                Title = db.Projects.Find(id)
            };
            return View(LViewModel);
        }
        
        //// Post: Projects/Assign Users
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Project Manager")]
        public ActionResult AssignToProject(PListViewModel selectedusers, int Projectid)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            var Helper = new UserProjetsHelper();
            foreach (var id in selectedusers.SelectednonPUsers)
            {
                if (id != null)
                {
                    Helper.AddUserToProject(id, Projectid);
                }                
            }            
            List<SelectListItem> listOfUsersOnProject = new List<SelectListItem>();
            List<SelectListItem> listOfUsersNotOnProject = new List<SelectListItem>();
            var zHelper = new UserProjetsHelper();
            var usersOnProject = zHelper.UsersOnProject(Projectid, "Developer");
            var usersNotOnProject = zHelper.UsersNotOnProject(Projectid, "Developer");
            //var listOfUsersOnProject = new MultiSelectList(usersOnProject, "Id", "DisplayName");
            //var listOfUsersNotOnProject = new MultiSelectList(usersNotOnProject, "Id", "DisplayName");
            foreach (var user in usersOnProject)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.DispalyName,
                    Value = user.Id
                };
                listOfUsersOnProject.Add(selectList);
            }
            foreach (var user in usersNotOnProject)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.DispalyName,
                    Value = user.Id
                };
                listOfUsersNotOnProject.Add(selectList);
            }
            PListViewModel LViewModel = new PListViewModel()
            {
                zUsers = listOfUsersOnProject,
                otherUsers = listOfUsersNotOnProject,
                Title = db.Projects.Find(Projectid)
            };
            return View("Details", LViewModel);
        }
        
        /// Post: Projects/Unassign Users
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Project Manager")]
        public ActionResult UnassignFromProject(PListViewModel selectedusers, int Projectid)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            var Helper = new UserProjetsHelper();
            foreach (var id in selectedusers.SelectedProjectUsers)
            {
                if (id != null)
                {
                    Helper.RemoveUserFromProject(id, Projectid);
                }                
            }
            List<SelectListItem> listOfUsersOnProject = new List<SelectListItem>();
            List<SelectListItem> listOfUsersNotOnProject = new List<SelectListItem>();
            var zHelper = new UserProjetsHelper();
            var usersOnProject = zHelper.UsersOnProject(Projectid, "Developer");
            var usersNotOnProject = zHelper.UsersNotOnProject(Projectid, "Developer");
            //var listOfUsersOnProject = new MultiSelectList(usersOnProject, "Id", "DisplayName");
            //var listOfUsersNotOnProject = new MultiSelectList(usersNotOnProject, "Id", "DisplayName");
            foreach (var user in usersOnProject)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.DispalyName,
                    Value = user.Id
                };
                listOfUsersOnProject.Add(selectList);
            }
            foreach (var user in usersNotOnProject)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.DispalyName,
                    Value = user.Id
                };
                listOfUsersNotOnProject.Add(selectList);
            }
            PListViewModel LViewModel = new PListViewModel()
            {
                zUsers = listOfUsersOnProject,
                otherUsers = listOfUsersNotOnProject,
                Title = db.Projects.Find(Projectid)
            };
            return View("Details", LViewModel);
        }
        
        //// GET: Projects/Create
        [Authorize(Roles = "Project Manager")]
        public ActionResult Create()
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            return View();
        }

        //// POST: Projects/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Project Manager")]
        public ActionResult Create([Bind(Include = "Id,Name")] Project project)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        //// GET: Projects/Edit/5
        [Authorize(Roles = "Project Manager")]
        public ActionResult Edit(int? id)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        //// POST: Projects/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Project Manager")]
        public ActionResult Edit([Bind(Include = "Id,Name")] Project project)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        //// GET: Projects/Delete/5
        [Authorize(Roles = "Project Manager")]
        public ActionResult Delete(int? id)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        //// POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Project Manager")]
        public ActionResult DeleteConfirmed(int id)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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
