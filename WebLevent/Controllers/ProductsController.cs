using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using WebLevent.Areas.Identity.Data;
using WebLevent.Models;

namespace WebLevent.Controllers
{
    public class ProductsController : Controller
    {
        private readonly WebLeventContext _context;
        public INotyfService _notifyService { get; }

        public ProductsController(WebLeventContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }
        public IActionResult Index(int page = 1, int CatID = 0)
        {
            var pageNumber = page;
            var pageSize = 20;
            List<SanPham> lsProducts = new List<SanPham>();
            if (CatID != 0)
            {
                lsProducts = _context.SanPham
                .AsNoTracking()
                .Where(x => x.LoaiId == CatID)
                .Include(x => x.Loai)
                .OrderByDescending(x => x.Id).ToList();
            }
            else
            {
                lsProducts = _context.SanPham.AsNoTracking()
                .Include(x => x.Loai)
                .OrderByDescending(x => x.Id).ToList();
            }

            PagedList<SanPham> models = new PagedList<SanPham>(lsProducts.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentCateID = CatID;
            ViewBag.CurrentPage = pageNumber;
            ViewData["DanhMuc"] = new SelectList(_context.LoaiSanPham, "Id", "Name");
            return View(models);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SanPham == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPham
                .Include(s => s.Loai)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }
    }
}
