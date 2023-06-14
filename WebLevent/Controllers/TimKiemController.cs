using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLevent.Areas.Identity.Data;
using WebLevent.Models;

namespace WebLevent.Controllers
{
    public class TimKiemController : Controller
    {
            private readonly WebLeventContext _context;

            public TimKiemController(WebLeventContext context)
            {
                _context = context;
            }
            [HttpPost]
            public IActionResult FindProduct(string keyword)
            {
                List<SanPham> ls = new List<SanPham>();
                if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
                {
                    return PartialView("DanSachSanPhamTimKiemPartial", null);
                }
                ls = _context.SanPham
                    .AsNoTracking()
                    .Include(a => a.Loai)
                    .Where(x => x.Name.Contains(keyword))
                    .OrderByDescending(x => x.Name)
                    .Take(10)
                    .ToList();
                if (ls == null)
                {
                    return PartialView("DanSachSanPhamTimKiemPartial", null);
                }
                else
                {
                    return PartialView("DanSachSanPhamTimKiemPartial", ls);
                }
            }
            public IActionResult Index()
            {
                return View();
            }
        }
    }
