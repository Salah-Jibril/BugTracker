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
using PagedList;

namespace BugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tickets
        public ActionResult Index(string sortOrder,string currentFilter, int? page, string searchString, string myFilter, string myFilter2,string myFilter3,string myFilter4,string myFilter5,string myFilter6)
        {
            var Helper = new UserNotificationsHelper();
            ViewBag.Notifications = Helper.filterNotifications(User.Identity.GetUserId());
            var aHelper = new UserHelper();
            var uHelper = new UserRolesHelper();
            if (searchString != null)
            { 
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }           
            ViewBag.CurrentFilter = searchString;
            ViewBag.myFilter = new SelectList(db.Projects, "Name", "Name");
            ViewBag.myFilter2 = new SelectList(db.TicketTypes, "Name", "Name");
            ViewBag.myFilter3 = new SelectList(db.TicketStatuses, "Name", "Name");
            ViewBag.myFilter4 = new SelectList(db.TicketPriorities, "Name", "Name");
            ViewBag.myFilter5 = new SelectList(db.Users, "DispalyName", "DispalyName");
            ViewBag.myFilter6 = new SelectList(uHelper.UsersInRole("Project Manager"), "DispalyName", "DispalyName");
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title" : "";
            ViewBag.PNameSortParm = String.IsNullOrEmpty(sortOrder) ? "Pname" : "";
            ViewBag.TtypeSortParm = String.IsNullOrEmpty(sortOrder) ? "ttypename" : "";
            ViewBag.OwnerSortParm = String.IsNullOrEmpty(sortOrder) ? "owner" : "";
            ViewBag.TstatusSortParm = String.IsNullOrEmpty(sortOrder) ? "tstatus" : "";
            ViewBag.TprioritySortParm = String.IsNullOrEmpty(sortOrder) ? "tpriority" : "";
            ViewBag.AssigneduserSortParm = String.IsNullOrEmpty(sortOrder) ? "assigneduser" : "";
            ViewBag.CreatedSortParm = sortOrder == "created" ? "created_desc" : "created";
            ViewBag.UpdatedSortParm = sortOrder == "updated" ? "updated_desc" : "updated";
                //var tickets = aHelper.ListTicketsPM();
            if (User.IsInRole("Project Manager"))
            {
                var tickets = from t in aHelper.ListTicketsPM() select t;
                if (!String.IsNullOrEmpty(myFilter))
                {
                    tickets = tickets.Where(t => t.Project.Name.Contains(myFilter));
                }
                if (!String.IsNullOrEmpty(myFilter2))
                {
                    tickets = tickets.Where(t => t.TicketType.Name.Contains(myFilter2));
                }
                if (!String.IsNullOrEmpty(myFilter3))
                {
                    tickets = tickets.Where(t => t.TicketStatus.Name.Contains(myFilter3));
                }
                if (!String.IsNullOrEmpty(myFilter4))
                {
                    tickets = tickets.Where(t => t.TicketPriority.Name.Contains(myFilter4));
                }
                if (!String.IsNullOrEmpty(myFilter5))
                {
                    tickets = tickets.Where(t => t.OwnerUser.DispalyName.Contains(myFilter5));
                }
                if (!String.IsNullOrEmpty(myFilter6))
                {
                    tickets = tickets.Where(t => t.AssignedToUser.DispalyName.Contains(myFilter6));
                }
                if (!String.IsNullOrEmpty(searchString))
                {
                    tickets = tickets.Where(t => t.Title.Contains(searchString)).Union(tickets.Where(t =>t.Description.Contains(searchString)))
                        .Union(tickets.Where(t =>t.Project.Name.Contains(searchString))).Union(tickets.Where(t =>t.TicketType.Name.Contains(searchString)))
                        .Union(tickets.Where(t => t.TicketStatus.Name.Contains(searchString))).Union(tickets.Where(t => t.TicketPriority != null && t.TicketPriority.Name.Contains(searchString) ))
                        .Union(tickets.Where(t => t.OwnerUser.DispalyName.Contains(searchString))).Union(tickets.Where(t => t.AssignedToUser != null && t.AssignedToUser.DispalyName.Contains(searchString)));
                }
                switch (sortOrder)
                {
                    case "title":
                        tickets = tickets.OrderBy(t => t.Title);
                        break;
                    case "Pname":
                        tickets = tickets.OrderBy(t => t.Project.Name);
                        break;
                    case "ttypename":
                        tickets = tickets.OrderBy(t => t.TicketType.Name);
                        break;
                    case "owner":
                        tickets = tickets.OrderBy(t => t.OwnerUser.DispalyName);
                        break;
                    case "created":
                        tickets = tickets.OrderBy(t => t.Created);
                        break;
                    case "created_desc":
                        tickets = tickets.OrderByDescending(t => t.Created);
                        break;
                    case "updated":
                        tickets = tickets.OrderBy(t => t.Updated);
                        break;
                    case "updated_desc":
                        tickets = tickets.OrderByDescending(t => t.Updated);
                        break;
                    case "tstatus":
                        tickets = tickets.OrderBy(t => t.TicketStatus.Name);
                        break;
                    case "tpriority":
                        tickets = tickets.OrderBy(t => t.TicketPriority.Name);
                        break;
                    case "assigneduser":
                        tickets = tickets.OrderBy(t => t.AssignedToUser.DispalyName);
                        break;
                    default:
                        tickets = tickets.OrderByDescending(t => t.Created);
                        break;
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(tickets.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                var tickets = from t in aHelper.ListTicketsForUser(User.Identity.GetUserId()) select t;
                if (!String.IsNullOrEmpty(searchString))
                {
                    tickets = tickets.Where(t => t.Title.Contains(searchString)).Union(tickets.Where(t => t.Description.Contains(searchString)))
                        .Union(tickets.Where(t => t.Project.Name.Contains(searchString))).Union(tickets.Where(t => t.TicketType.Name.Contains(searchString)))
                        .Union(tickets.Where(t => t.TicketStatus.Name.Contains(searchString))).Union(tickets.Where(t => t.OwnerUser.DispalyName.Contains(searchString)));
                }
                switch (sortOrder)
                {
                    case "title":
                        tickets = tickets.OrderBy(t => t.Title);
                        break;
                    case "Pname":
                        tickets = tickets.OrderBy(t => t.Project.Name);
                        break;
                    case "ttypename":
                        tickets = tickets.OrderBy(t => t.TicketType.Name);
                        break;
                    case "owner":
                        tickets = tickets.OrderBy(t => t.OwnerUser.DispalyName);
                        break;
                    case "created":
                        tickets = tickets.OrderBy(t => t.Created);
                        break;
                    case "created_desc":
                        tickets = tickets.OrderByDescending(t => t.Created);
                        break;
                    case "updated":
                        tickets = tickets.OrderBy(t => t.Updated);
                        break;
                    case "updated_desc":
                        tickets = tickets.OrderByDescending(t => t.Updated);
                        break;
                    case "tstatus":
                        tickets = tickets.OrderBy(t => t.TicketStatus.Name);
                        break;
                    default:
                        tickets = tickets.OrderByDescending(t => t.Created);
                        break;
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(tickets.ToPagedList(pageNumber, pageSize));
            }            
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            var Helper = new UserNotificationsHelper();
            ViewBag.Notifications = Helper.filterNotifications(User.Identity.GetUserId());
            var aHelper = new UserHelper();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ticket = db.Tickets.Find((int) id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            foreach (var notif in ticket.TicketNotifications)
            {
                notif.Read = true;
            }
            db.Entry(ticket).State = EntityState.Modified;
            db.SaveChanges();
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize]
        public ActionResult Create()
        {
            var Helper = new UserNotificationsHelper();
            ViewBag.Notifications = Helper.filterNotifications(User.Identity.GetUserId());
            var rHelper = new UserRolesHelper();
            ViewBag.AssignedToUserId = new SelectList(rHelper.UsersInRole("Developer"), "Id", "FirstName");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");            
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        
        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Created,ProjectId,TicketTypeId,TicketPriorityId,OwnerUserId,TicketStatusId,AssignedToUserId, TicketComments, TicketHistories, TicketAttachments")] Ticket ticket, ApplicationUser user, string comment, string descriptions, HttpPostedFileBase fileUpload)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                ticket.OwnerUserId = User.Identity.GetUserId();
                ticket.Created = System.DateTimeOffset.UtcNow;                
                
                var Helper = new UserRolesHelper();
                var anotherHelper = new UserHelper();

                #region Notifications
                db.TicketNotifications.Add(new TicketNotification()
                {
                    Notice = "You created a new ticket on " + ticket.Created.ToString(),
                    TicketId = ticket.Id,
                    UserId = ticket.OwnerUserId
                });

                foreach (var auser in Helper.UsersInRole("Project Manager"))
                {
                    db.TicketNotifications.Add(new TicketNotification()
                    {
                        Notice = anotherHelper.TicketOwner(ticket.OwnerUserId).DispalyName + " has created a new ticket on " + ticket.Created.ToString(),
                        TicketId = ticket.Id,
                        UserId = auser.Id
                    });
                }
                if (ticket.AssignedToUserId != null)
                {
                    ticket.AssignedToUser = db.Users.Find(ticket.AssignedToUserId);
                    db.TicketNotifications.Add(new TicketNotification()
                    {
                        Notice = "A ticket has been assigned to you on " + ticket.Created.ToString(),
                        TicketId = ticket.Id,
                        UserId = ticket.AssignedToUserId
                    });

                    db.TicketNotifications.Add(new TicketNotification()
                    {
                        Notice = "Your ticket has been assigned to a developer on " + ticket.Created.ToString(),
                        TicketId = ticket.Id,
                        UserId = ticket.OwnerUserId
                    });
                }
                #endregion

                if (comment != "")
                { db.TicketComments.Add(new TicketComment() { TicketId = ticket.Id, Created = ticket.Created, UserId = ticket.OwnerUserId, Comment = comment }); }

                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    var attachment = new TicketAttachment();
                    attachment.FilePath = Path.GetFileName(fileUpload.FileName);
                    attachment.TicketId = ticket.Id;
                    attachment.Description = descriptions;
                    attachment.Created = ticket.Created;
                    attachment.UserId = ticket.OwnerUserId;
                    attachment.FileUrl = "/Attachments/" + attachment.FilePath;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    fileUpload.SaveAs(Path.Combine(Server.MapPath("/Attachments"), attachment.FilePath));
                    db.TicketAttachments.Add(attachment);
                }                
                
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var rHelper = new UserRolesHelper();
            ViewBag.AssignedToUserId = new SelectList(rHelper.UsersInRole("Developer"), "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tHelper = new UserHelper();
            Ticket ticket = tHelper.FindTicket((int)id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            TempData["Ticket"] = ticket;
            var rHelper = new UserRolesHelper();
            ViewBag.AssignedToUserId = new SelectList(rHelper.UsersInRole("Developer"), "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,ProjectId,TicketTypeId,OwnerUserId,TicketPriorityId,TicketStatusId,AssignedToUserId,TicketComments,TicketHistories,TicketAttachments")] Ticket ticket, string comment, string descriptions, HttpPostedFileBase fileUpload)
        {
            Ticket pticket = TempData["Ticket"] as Ticket;
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            var anotherHelper = new UserHelper();

            if (ModelState.IsValid)
            {
                ticket.Updated = System.DateTimeOffset.UtcNow;
                if (ticket.TicketStatusId == 0) { ticket.TicketStatusId = 1; }

                if (comment != "")
                {
                    db.TicketComments.Add(new TicketComment() { TicketId = ticket.Id, Created = System.DateTimeOffset.UtcNow, UserId = ticket.OwnerUserId, Comment = comment });
                }
                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    var attachment = new TicketAttachment();
                    attachment.FilePath = Path.GetFileName(fileUpload.FileName);
                    attachment.TicketId = ticket.Id;
                    attachment.Description = descriptions;
                    attachment.Created = System.DateTimeOffset.UtcNow;
                    attachment.UserId = ticket.OwnerUserId;
                    attachment.FileUrl = "/Attachments/" + attachment.FilePath;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    fileUpload.SaveAs(Path.Combine(Server.MapPath("/Attachments"), attachment.FilePath));
                    db.TicketAttachments.Add(attachment);
                }

                #region Notifications
                if (fileUpload != null && fileUpload.ContentLength > 0 && comment != "")
                {
                    db.TicketNotifications.Add(new TicketNotification()
                    {
                        Notice = User.Identity.Name + " added a new comment and attachment to the ticket on " + ticket.Updated.ToString(),
                        TicketId = ticket.Id,
                        UserId = User.Identity.GetUserId()
                    });
                    var Helper = new UserRolesHelper();
                    foreach (var auser in Helper.UsersInRole("Project Manager"))
                    {
                        db.TicketNotifications.Add(new TicketNotification()
                        {
                            Notice = User.Identity.Name + " added a new comment and attachment to the ticket on " + ticket.Updated.ToString(),
                            TicketId = ticket.Id,
                            UserId = auser.Id
                        });
                    }
                }
                else
                {
                    if (fileUpload != null && fileUpload.ContentLength > 0)
                    {
                        db.TicketNotifications.Add(new TicketNotification()
                        {
                            Notice = User.Identity.Name + " added a new attachment to the ticket on " + ticket.Updated.ToString(),
                            TicketId = ticket.Id,
                            UserId = User.Identity.GetUserId()
                        });
                        var Helper = new UserRolesHelper();
                        foreach (var auser in Helper.UsersInRole("Project Manager"))
                        {
                            db.TicketNotifications.Add(new TicketNotification()
                            {
                                Notice = User.Identity.Name + " added a new attachment to the ticket on " + ticket.Updated.ToString(),
                                TicketId = ticket.Id,
                                UserId = auser.Id
                            });
                        }
                    }
                    if (comment != "")
                    {
                        db.TicketNotifications.Add(new TicketNotification()
                        {
                            Notice = User.Identity.Name + " added a new comment to the ticket on " + ticket.Updated.ToString(),
                            TicketId = ticket.Id,
                            UserId = User.Identity.GetUserId()
                        });
                        var Helper = new UserRolesHelper();
                        foreach (var auser in Helper.UsersInRole("Project Manager"))
                        {
                            db.TicketNotifications.Add(new TicketNotification()
                            {
                                Notice = User.Identity.Name + " added a new comment to the ticket on " + ticket.Updated.ToString(),
                                TicketId = ticket.Id,
                                UserId = auser.Id
                            });
                        }
                    }
                }
                if (pticket.AssignedToUserId != ticket.AssignedToUserId)
                {
                    db.TicketNotifications.Add(new TicketNotification()
                    {
                        Notice = "A ticket was assigned to you on with a priority: " + db.TicketPriorities.Find(ticket.TicketPriorityId).Name,
                        TicketId = ticket.Id,
                        UserId = ticket.AssignedToUserId
                    });
                    db.TicketNotifications.Add(new TicketNotification()
                    {
                        Notice = "A Developer has been assigned to your ticket on " + ticket.Updated.ToString(),
                        TicketId = ticket.Id,
                        UserId = ticket.OwnerUserId
                    });
                }
                #endregion

                #region Histories
                if (pticket.ProjectId != ticket.ProjectId)
                {
                    db.TicketHistories.Add(new TicketHistory()
                    {
                        TicketId = ticket.Id,
                        UserId = ticket.OwnerUserId,
                        OldValue = pticket.Project.Name,
                        NewValue = db.Projects.Find(ticket.ProjectId).Name,
                        Changed = ticket.Updated,
                        Property = "Project/Catagory",
                    });
                }
                if (pticket.TicketStatusId != ticket.TicketStatusId)
                {
                    db.TicketHistories.Add(new TicketHistory()
                    {
                        TicketId = ticket.Id,
                        UserId = ticket.OwnerUserId,
                        OldValue = pticket.TicketStatus.Name,
                        NewValue = db.TicketStatuses.Find(ticket.TicketStatusId).Name,
                        Changed = ticket.Updated,
                        Property = "Ticket Status"
                    });
                }
                if (pticket.TicketTypeId != ticket.TicketTypeId)
                {
                    db.TicketHistories.Add(new TicketHistory()
                    {
                        TicketId = ticket.Id,
                        UserId = ticket.OwnerUserId,
                        OldValue = pticket.TicketType.Name,
                        NewValue = db.TicketTypes.Find(ticket.TicketTypeId).Name,
                        Changed = ticket.Updated,
                        Property = "Ticket Type"
                    });
                }
                if (ticket.TicketPriorityId != null && pticket.TicketPriorityId != ticket.TicketPriorityId)
                {
                    if (pticket.TicketPriority == null)
                    {
                        db.TicketHistories.Add(new TicketHistory()
                        {
                            TicketId = ticket.Id,
                            UserId = ticket.OwnerUserId,
                            OldValue = "Not Set",
                            NewValue = db.TicketPriorities.Find(ticket.TicketPriorityId).Name,
                            Changed = ticket.Updated,
                            Property = "Ticket Priority"
                        });
                    }
                    else
                    {
                        db.TicketHistories.Add(new TicketHistory()
                        {
                            TicketId = ticket.Id,
                            UserId = ticket.OwnerUserId,
                            OldValue = pticket.TicketPriority.Name,
                            NewValue = db.TicketPriorities.Find(ticket.TicketPriorityId).Name,
                            Changed = ticket.Updated,
                            Property = "Ticket Priority"
                        });
                    }
                }
                if (ticket.AssignedToUserId != null && ticket.AssignedToUserId != pticket.AssignedToUserId)
                {
                    if (pticket.AssignedToUser == null)
                    {
                        db.TicketHistories.Add(new TicketHistory()
                        {
                            TicketId = ticket.Id,
                            UserId = ticket.OwnerUserId,
                            OldValue = "Not yet Assigned",
                            NewValue = db.Users.Find(ticket.AssignedToUserId).DispalyName,
                            Changed = ticket.Updated,
                            Property = "Assigned Developer"
                        });
                    }
                    else
                    {
                        db.TicketHistories.Add(new TicketHistory()
                        {
                            TicketId = ticket.Id,
                            UserId = ticket.OwnerUserId,
                            OldValue = pticket.AssignedToUser.DispalyName,
                            NewValue = db.Users.Find(ticket.AssignedToUserId).DispalyName,
                            Changed = ticket.Updated,
                            Property = "Assigned Developer"
                        });
                    }
                }
                #endregion

                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                //anotherHelper.TicketModified(ticket);
                return RedirectToAction("Index");
            }

            var rHelper = new UserRolesHelper();
            ViewBag.AssignedToUserId = new SelectList(rHelper.UsersInRole("Developer"), "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
           
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tHelper = new UserHelper();
            Ticket ticket = tHelper.FindTicket((int)id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            var tHelper = new UserHelper();
            Ticket ticket = tHelper.FindTicket((int)id);
            tHelper.RemoveFromTicket(ticket);
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
