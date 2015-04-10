using System.Collections.Generic;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class ListViewModel
    {
        public IEnumerable<string> SelectedroleUsers { get; set; }
        public IEnumerable<SelectListItem> zUsers { get; set; }
        public IEnumerable<string> SelectednonUsers { get; set; }
        public IEnumerable<SelectListItem> otherUsers { get; set; }    
    }
}