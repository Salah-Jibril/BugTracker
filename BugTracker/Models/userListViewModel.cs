using System.Collections.Generic;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class userListViewModel
    {
        public List<string> userRoles { get; set; }
        public List<ApplicationUser> Person { get; set; }
    }
}