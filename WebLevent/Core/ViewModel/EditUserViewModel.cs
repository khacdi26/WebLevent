using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebLevent.Areas.Identity.Data;

namespace WebLevent.Core.ViewModel
{
    public class EditUserViewModel
    {
        public WebLeventUser User{ get; set; }
        public IList<SelectListItem> Roles{ get; set; }
    }
}
