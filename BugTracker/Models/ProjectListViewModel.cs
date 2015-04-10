using System.Collections.Generic;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class PListViewModel
    {
        public Project Title { get; set; }
        public List<string> SelectedProjectUsers { get; set; }
        public List<SelectListItem> zUsers { get; set; }
        public List<string> SelectednonPUsers { get; set; }
        public List<SelectListItem> otherUsers { get; set; }
    }
}