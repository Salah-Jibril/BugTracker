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
using Microsoft.AspNet.Identity.EntityFramework;
using System.Text;

namespace BugTracker.Controllers
{
    public class RolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Roles
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            return View(db.Roles.ToList());
        }
        
        // GET: Roles/AdminAccount
        [Authorize(Roles = "Admin")]
        public ActionResult AdminAccount()
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            List<SelectListItem> listadminUsers = new List<SelectListItem>();
            List<SelectListItem> listnonadminUsers = new List<SelectListItem>();
            var adminHelper = new UserRolesHelper();
            var result1 = adminHelper.UsersInRole("Admin");
            foreach (var user in result1)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.DispalyName,
                    Value = user.Id
                };
                listadminUsers.Add(selectList);
            }
            var result2 = adminHelper.UsersNotInRole("Admin");
            foreach (var user in result2)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.DispalyName,
                    Value = user.Id
                };
                listnonadminUsers.Add(selectList);
            }
            ListViewModel adminLViewModel = new ListViewModel()
            {
                zUsers = listadminUsers,
                otherUsers = listnonadminUsers
            };
            
            return View(adminLViewModel);
            
            /*ViewBag.adminresult = result;
            return View(result);*/
        }
        //Post: Roles/AdminAccount
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult AdminAddToRole(ListViewModel selectedUser, string RoleName)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            var Helper = new UserRolesHelper();
            foreach (var id in selectedUser.SelectednonUsers)
            {
                if (id != null)
                {                    
                    Helper.AddUserToRole(id, RoleName);
                    Helper.AddUserToRole(id, "Project Manager");
                    Helper.AddUserToRole(id, "Developer");
                }
            }
                List<SelectListItem> listadminUsers = new List<SelectListItem>();
                List<SelectListItem> listnonadminUsers = new List<SelectListItem>();
                var result1 = Helper.UsersInRole(RoleName);
                foreach (var user in result1)
                {
                    SelectListItem selectList = new SelectListItem()
                    {
                        Text = user.DispalyName,
                        Value = user.Id
                    };
                    listadminUsers.Add(selectList);
                }
                var result2 = Helper.UsersNotInRole(RoleName);
                foreach (var user in result2)
                {
                    SelectListItem selectList = new SelectListItem()
                    {
                        Text = user.DispalyName,
                        Value = user.Id
                    };
                    listnonadminUsers.Add(selectList);
                }
                ListViewModel adminLViewModel = new ListViewModel()
                {
                    zUsers = listadminUsers,
                    otherUsers = listnonadminUsers
                };
                return View("AdminAccount", adminLViewModel);            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult AdminRemoveFromRole(ListViewModel selectedUser, string RoleName)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            var Helper = new UserRolesHelper();
            foreach (var id in selectedUser.SelectedroleUsers)
            {
                if (id != null)
                {
                    Helper.RemoveUserFromRole(id, RoleName);
                }
            }
            List<SelectListItem> listadminUsers = new List<SelectListItem>();
            List<SelectListItem> listnonadminUsers = new List<SelectListItem>();
            var result1 = Helper.UsersInRole(RoleName);
            foreach (var user in result1)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.DispalyName,
                    Value = user.Id
                };
                listadminUsers.Add(selectList);
            }
            var result2 = Helper.UsersNotInRole(RoleName);
            foreach (var user in result2)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.DispalyName,
                    Value = user.Id
                };
                listnonadminUsers.Add(selectList);
            }
            ListViewModel adminLViewModel = new ListViewModel()
            {
                zUsers = listadminUsers,
                otherUsers = listnonadminUsers
            };
            return View("AdminAccount", adminLViewModel);
        }

        // GET: Roles/Project Manager
        [Authorize(Roles = "Admin")]
        public ActionResult ProjectManager()
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            List<SelectListItem> listadminUsers = new List<SelectListItem>();
            List<SelectListItem> listnonadminUsers = new List<SelectListItem>();
            var adminHelper = new UserRolesHelper();
            var result1 = adminHelper.UsersInRole("Project Manager");
            foreach (var user in result1)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.DispalyName,
                    Value = user.Id
                };
                listadminUsers.Add(selectList);
            }
            var result2 = adminHelper.UsersNotInRole("Project Manager");
            foreach (var user in result2)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.DispalyName,
                    Value = user.Id
                };
                listnonadminUsers.Add(selectList);
            }
            ListViewModel adminLViewModel = new ListViewModel()
            {
                zUsers = listadminUsers,
                otherUsers = listnonadminUsers
            };
            return View(adminLViewModel);
        }
        // Post: Roles/Project Manager
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult PMAddToRole(ListViewModel selectedUser, string RoleName)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            var Helper = new UserRolesHelper();
            foreach (var id in selectedUser.SelectednonUsers)
            {
                if (id != null)
                {
                    Helper.AddUserToRole(id, RoleName);
                    Helper.AddUserToRole(id, "Developer");
                }
            }
            List<SelectListItem> listadminUsers = new List<SelectListItem>();
            List<SelectListItem> listnonadminUsers = new List<SelectListItem>();
            var result1 = Helper.UsersInRole(RoleName);
            foreach (var user in result1)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.DispalyName,
                    Value = user.Id
                };
                listadminUsers.Add(selectList);
            }
            var result2 = Helper.UsersNotInRole(RoleName);
            foreach (var user in result2)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.DispalyName,
                    Value = user.Id
                };
                listnonadminUsers.Add(selectList);
            }
            ListViewModel adminLViewModel = new ListViewModel()
            {
                zUsers = listadminUsers,
                otherUsers = listnonadminUsers
            };
            return View("ProjectManager", adminLViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult PMRemoveFromRole(ListViewModel selectedUser, string RoleName)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            var Helper = new UserRolesHelper();
            foreach (var id in selectedUser.SelectedroleUsers)
            {
                if (id != null)
                {
                    Helper.RemoveUserFromRole(id, RoleName);
                }
            }
            List<SelectListItem> listadminUsers = new List<SelectListItem>();
            List<SelectListItem> listnonadminUsers = new List<SelectListItem>();
            var result1 = Helper.UsersInRole(RoleName);
            foreach (var user in result1)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.DispalyName,
                    Value = user.Id
                };
                listadminUsers.Add(selectList);
            }
            var result2 = Helper.UsersNotInRole(RoleName);
            foreach (var user in result2)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.DispalyName,
                    Value = user.Id
                };
                listnonadminUsers.Add(selectList);
            }
            ListViewModel adminLViewModel = new ListViewModel()
            {
                zUsers = listadminUsers,
                otherUsers = listnonadminUsers
            };
            return View("ProjectManager", adminLViewModel);
        }

        // GET: Roles/Developer/5
        [Authorize(Roles = "Admin")]
        public ActionResult Developer()
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            List<SelectListItem> listadminUsers = new List<SelectListItem>();
            List<SelectListItem> listnonadminUsers = new List<SelectListItem>();
            var adminHelper = new UserRolesHelper();
            var result1 = adminHelper.UsersInRole("Developer");
            foreach (var user in result1)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.DispalyName,
                    Value = user.Id
                };
                listadminUsers.Add(selectList);
            }
            var result2 = adminHelper.UsersNotInRole("Developer");
            foreach (var user in result2)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.DispalyName,
                    Value = user.Id
                };
                listnonadminUsers.Add(selectList);
            }
            ListViewModel adminLViewModel = new ListViewModel()
            {
                zUsers = listadminUsers,
                otherUsers = listnonadminUsers
            };
            return View(adminLViewModel);
        }
        // POST: Roles/Developer
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DAddToRole(ListViewModel selectedUser, string RoleName)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            var Helper = new UserRolesHelper();
            foreach (var id in selectedUser.SelectednonUsers)
            {
                if (id != null)
                {
                    Helper.AddUserToRole(id, RoleName);
                }
            }
            List<SelectListItem> listadminUsers = new List<SelectListItem>();
            List<SelectListItem> listnonadminUsers = new List<SelectListItem>();
            var result1 = Helper.UsersInRole(RoleName);
            foreach (var user in result1)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.DispalyName,
                    Value = user.Id
                };
                listadminUsers.Add(selectList);
            }
            var result2 = Helper.UsersNotInRole(RoleName);
            foreach (var user in result2)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.DispalyName,
                    Value = user.Id
                };
                listnonadminUsers.Add(selectList);
            }
            ListViewModel adminLViewModel = new ListViewModel()
            {
                zUsers = listadminUsers,
                otherUsers = listnonadminUsers
            };
            return View("Developer", adminLViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DRemoveFromRole(ListViewModel selectedUser, string RoleName)
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            var Helper = new UserRolesHelper();
            foreach (var id in selectedUser.SelectedroleUsers)
            {
                if (id != null)
                {
                    Helper.RemoveUserFromRole(id, RoleName);
                }
            }
            List<SelectListItem> listadminUsers = new List<SelectListItem>();
            List<SelectListItem> listnonadminUsers = new List<SelectListItem>();
            var result1 = Helper.UsersInRole(RoleName);
            foreach (var user in result1)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.DispalyName,
                    Value = user.Id
                };
                listadminUsers.Add(selectList);
            }
            var result2 = Helper.UsersNotInRole(RoleName);
            foreach (var user in result2)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.DispalyName,
                    Value = user.Id
                };
                listnonadminUsers.Add(selectList);
            }
            ListViewModel adminLViewModel = new ListViewModel()
            {
                zUsers = listadminUsers,
                otherUsers = listnonadminUsers
            };
            return View("Developer", adminLViewModel);
        }

        // Get: Roles/Submitter
        [Authorize(Roles = "Admin")]
        public ActionResult Submitter()
        {
            var nHelper = new UserNotificationsHelper();
            ViewBag.Notifications = nHelper.filterNotifications(User.Identity.GetUserId());
            userListViewModel zthings = new userListViewModel();
            var list = new List<string>();
            var list1 = new List<ApplicationUser>();
            var list2 = new List<string>();
            var users = db.Users.ToList();
            foreach (var user in users)
	        {
                list1.Add(user);
		        var roles = user.Roles;
                list.Clear();
                foreach (var role in roles)
	            {
                    var zRole = db.Roles.Find(role.RoleId);
                    list.Add(zRole.Name);                    
	            }
                list.Sort();
                list2.Add(string.Join(", ",list));
	        }
            zthings.Person = list1;
            zthings.userRoles = list2;
            ViewBag.usernrole = zthings.Person.Zip(zthings.userRoles, (p, u) => new { zUser = p, zRole = u });            
            return View(zthings);
        }
        
    }
}
