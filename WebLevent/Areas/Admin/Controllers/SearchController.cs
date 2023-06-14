using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLevent.Areas.Identity.Data;
using WebLevent.Models;

namespace WebLevent.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchController : Controller
    {
        private readonly WebLeventContext _context;

        public SearchController(WebLeventContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult FindProduct (string keyword)
        {
            List<SanPham> ls = new List<SanPham>();
            if(string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListProductsSearchPartial",null);
            }
            ls = _context.SanPham
                .AsNoTracking()
                .Include(a=>a.Loai)
                .Where(x=>x.Name.Contains(keyword))
                .OrderByDescending(x=>x.Name)
                .Take(10)
                .ToList();
            if (ls == null)
            {
                return PartialView("ListProductsSearchPartial", null);
            }
            else
            {
                return PartialView("ListProductsSearchPartial", ls);
            }
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
