namespace BugTracker.Migrations
{
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Project Manager" };

                manager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Developer" };

                manager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Submitter" };

                manager.Create(role);
            }
            if (!context.Users.Any(u => u.Email == "salmohjib@gmail.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    UserName = "salmohjib@gmail.com",
                    Email = "salmohjib@gmail.com",
                    FirstName = "Salahadin",
                    LastName = "Jibril",
                    DispalyName = "Salahadin"
                };

                    manager.Create(user, "Password-1");
                // manager.AddToRole(user.Id, "Admin");
                // manager.AddToRole(user.Id, "Moderator");
                //manager.AddToRole(user.Id, new String[] {"Admin", "Moderator"});
                    manager.AddToRoles(user.Id, "Admin");
                    manager.AddToRoles(user.Id, "Project Manager");
                    manager.AddToRoles(user.Id, "Developer");
                    manager.AddToRoles(user.Id, "Submitter");
            }
            if (!context.Users.Any(u => u.Email == "Guestemail@guest.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    UserName = "Guestemail@guest.com",
                    Email = "Guestemail@guest.com",
                    FirstName = "Guest",
                    LastName = "Guest",
                    DispalyName = "Guest"
                };

                manager.Create(user, "Password-1");
                // manager.AddToRole(user.Id, "Admin");
                // manager.AddToRole(user.Id, "Moderator");
                //manager.AddToRole(user.Id, new String[] {"Admin", "Moderator"});
                manager.AddToRoles(user.Id, "Admin");
                manager.AddToRoles(user.Id, "Project Manager");
                manager.AddToRoles(user.Id, "Developer");
                manager.AddToRoles(user.Id, "Submitter");
            }
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //if (!context.Users.Any(u => u.Email == "ajenson@coderfoundrycom"))
            //{
            //    userManager.Create(new ApplicationUser
            //    {
            //        UserName = "ajenson@coderfoundry.com",
            //        Email = "ajenson@coderfoundry.com",
            //    },  "Password-1");
            //}
               
            var userId = userManager.FindByEmail("salmohjib@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");
            //Create a new BTUser entry to match   
            if (!context.Users.Any(u => u.Email == "pmguestemail@guest.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    UserName = "pmguestemail@guest.com",
                    Email = "pmguestemail@guest.com",
                    FirstName = "PMGuest",
                    LastName = "PMGuest",
                    DispalyName = "PMGuest"
                };

                manager.Create(user, "Password-1");
                // manager.AddToRole(user.Id, "Admin");
                // manager.AddToRole(user.Id, "Moderator");
                //manager.AddToRole(user.Id, new String[] {"Admin", "Moderator"});
                manager.AddToRoles(user.Id, "Project Manager");
                manager.AddToRoles(user.Id, "Developer");
                manager.AddToRoles(user.Id, "Submitter");
            }
            if (!context.Users.Any(u => u.Email == "dguestemail@guest.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    UserName = "dguestemail@guest.com",
                    Email = "dguestemail@guest.com",
                    FirstName = "DGuest",
                    LastName = "DGuest",
                    DispalyName = "DGuest"
                };

                manager.Create(user, "Password-1");
                // manager.AddToRole(user.Id, "Admin");
                // manager.AddToRole(user.Id, "Moderator");
                //manager.AddToRole(user.Id, new String[] {"Admin", "Moderator"});
                manager.AddToRoles(user.Id, "Developer");
                manager.AddToRoles(user.Id, "Submitter");
            }
            if (!context.Users.Any(u => u.Email == "sguestemail@guest.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    UserName = "sguestemail@guest.com",
                    Email = "sguestemail@guest.com",
                    FirstName = "SGuest",
                    LastName = "SGuest",
                    DispalyName = "SGuest"
                };

                manager.Create(user, "Password-1");
                // manager.AddToRole(user.Id, "Admin");
                // manager.AddToRole(user.Id, "Moderator");
                //manager.AddToRole(user.Id, new String[] {"Admin", "Moderator"});
                manager.AddToRoles(user.Id, "Submitter");
            }
        }
    }
}
