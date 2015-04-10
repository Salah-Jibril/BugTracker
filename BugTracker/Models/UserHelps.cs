using System.Data.Entity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using BugTracker.Models;

namespace BugTracker.Models
{
    public class UserRolesHelper
    {
        private UserManager<ApplicationUser> manager =
            new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        public bool IsUserInRole(string userId, string roleName)
        {
           return manager.IsInRole(userId, roleName);
        }
        public IList<string> ListUserRoles(string userId)
        {
            return manager.GetRoles(userId);
        }
        public bool AddUserToRole(string userId, string roleName)
        {
            var result = manager.AddToRole(userId, roleName);
            return result.Succeeded;
        }
        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = manager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }
        public IList<ApplicationUser> UsersInRole(string roleName)
        {
            var db = new ApplicationDbContext();
            var resultList = new List<ApplicationUser>();
            foreach (var user in db.Users)
	        {
                if (IsUserInRole(user.Id,roleName))
	            {
                    resultList.Add(user);
	            }
	        }
            return resultList;
        }
        public IList<ApplicationUser> UsersNotInRole(string roleName)
        {
            var db = new ApplicationDbContext();
            var resultList = new List<ApplicationUser>();
            foreach (var user in db.Users)
            {
                if (!IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }
    }
    public class UserProjetsHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public bool IsOnProject(string userId, int projectId)
        {
            if (db.Projects.Find(projectId).Users.Contains(db.Users.Find(userId)))
            {
                return true;
            }
            return false;
        }
        public void AddUserToProject(string userId, int projectId)
        {
            if (!(this.IsOnProject(userId, projectId)))
            {
                db.Projects.Find(projectId).Users.Add(db.Users.Find(userId));
                db.SaveChanges();
            }
        }
        public void RemoveUserFromProject(string userId, int projectId)
        {
            if ( this.IsOnProject(userId, projectId))
            {                
                db.Projects.Find(projectId).Users.Remove(db.Users.Find(userId));
                db.SaveChanges();
            }
        }

        public ICollection<Project> ListProjectForUser(string userId)
        {
            return db.Users.Find(userId).Projects;
        }
        
        public  ICollection<ApplicationUser> UsersOnProject(int projectId)
        {
            return db.Projects.Find(projectId).Users;
        }

        public  IList<ApplicationUser> UsersNotOnProject(int projectId)
        {
            var userList = new List<ApplicationUser>();

            foreach (var user in db.Users)
            {
                if (!(this.IsOnProject(user.Id, projectId))) 
                {
                    userList.Add(user);
                }
            }
            return userList;
        }
        
        public IList<ApplicationUser> UsersOnProject(int projectId, string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var rolesHelper = new UserRolesHelper();

            foreach (var user in db.Projects.Find(projectId).Users)
            {
                if (rolesHelper.IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }
        public IList<ApplicationUser> UsersNotOnProject(int projectId, string roleName)
        {
            var userList = new List<ApplicationUser>();
            var rolesHelper = new UserRolesHelper();

            foreach (var user in db.Users)
            {
                if ((!this.IsOnProject(user.Id, projectId)) && (rolesHelper.IsUserInRole(user.Id, roleName)))
                {
                    userList.Add(user);
                }
            }
            return userList;
        }
    }
    public class UserHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public string TicketAssignedUsers(int ticketId)
        {
            return db.Tickets.Find(ticketId).AssignedToUserId;
        }

        public TicketType ticketType(int? tickettypeid)
        {
            return db.TicketTypes.Find(tickettypeid);
        }
        public TicketStatus ticketStatus(int? ticketstatusid)
        {
            return db.TicketStatuses.Find(ticketstatusid);
        }
        public TicketPriority ticketPriority(int? ticketpriorityid)
        {
            return db.TicketPriorities.Find(ticketpriorityid);
        }
        public Project project(int? projectid)
        {
            return db.Projects.Find(projectid);
        }


        public ApplicationUser TicketOwner(string userId)
        {   
            return db.Users.Find(userId);            
        }

        public Ticket FindTicket(int id)
        {
            return db.Tickets.Find(id);
        }

        public ICollection<Ticket> ListTicketsPM()
        {
            var tickets = new List<Ticket>();
            foreach (var ticket in db.Tickets)
            {
                tickets.Add(ticket);
            }
            return tickets;
        }

        public ICollection<Ticket> ListTicketsForUser(string userId)
        {
            return db.Users.Find(userId).TicketsOwned;
        }

        public void AddtoTicket(Ticket ticket)
        {
            db.Tickets.Add(ticket);
        }

        public void RemoveFromTicket(Ticket ticket)
        {
            db.Tickets.Remove(ticket);
            db.SaveChanges();
        }

        public void AddTicketToUser(Ticket ticket, string userId)
        {
            db.Users.Find(userId).TicketsOwned.Add(ticket);
            db.SaveChanges();
        }

        public void AddTicketComment(int ticketid, TicketComment comment)
        {
            db.Tickets.Find(ticketid).TicketComments.Add(comment);
            Ticket ticket = db.Tickets.Find(ticketid);
            db.Entry(ticket).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void TicketModified(Ticket ticket)
        {            
            db.Entry(ticket).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
    public class UserNotificationsHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public List<string> ShowNewNotification(string userid)
        {
            var notifications = new List<string>();
            foreach (var item in db.TicketNotifications)
            {
                if(!item.Read && item.UserId == userid)
                {
                    notifications.Add(item.Notice);
                }                
            }
            return notifications;
        }

        public ICollection<TicketNotification> ShowAllNotification(string userid)
        {
            
            return db.Users.Find(userid).TicketNotifications;
        }

        public bool AlreadyAssigned(ApplicationUser user, Ticket ticket)
        {
            bool a = false;
            foreach (var note in user.TicketNotifications)
            {
                if (ticket.AssignedToUserId == user.Id) { a = true; }
            }
            return a;
        }

        public NotificationViewModel filterNotifications(string userid)
        {
            if (userid != null)
            {
                var list = new List<string>();
                var listId = new List<int>();
                foreach (var notice in db.TicketNotifications)
                {
                    if (!notice.Read && notice.UserId == userid)
                    {
                        list.Add(notice.Notice);
                        listId.Add(notice.TicketId);
                    }                        
                }
                NotificationViewModel NVModel = new NotificationViewModel()
                {
                    Notifications = list,
                    Count = list.Count,
                    TicketId = listId
                };
                return NVModel;
            }                    
            return null;            
        }
    }
}