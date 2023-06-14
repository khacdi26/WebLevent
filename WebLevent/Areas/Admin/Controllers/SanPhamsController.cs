using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using WebLevent.Areas.Identity.Data;
using WebLevent.Models;
using PagedList.Core.Mvc;
using Microsoft.AspNetCore.Http;
using WebLevent.Helpper;
using System.Runtime.CompilerServices;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace WebLevent.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Manager")]

    public class SanPhamsController : Controller
    {
        private readonly WebLeventContext _context;
        public INotyfService _notifyService { get; }

        public SanPhamsController(WebLeventContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: Admin/SanPhams
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
        public IActionResult Filtter(int CatID = 0)
        {
            var url = $"/Admin/SanPhams?CatID={CatID}";
            if (CatID == 0)
            {
                url = $"/Admin/SanPhams";
            }
            return Json(new { status = "success", redirectUrl = url });
        }

        // GET: Admin/SanPhams/Details/5
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

        // GET: Admin/SanPhams/Create
        public IActionResult Create()
        {
            ViewData["LoaiId"] = new SelectList(_context.Set<LoaiSanPham>(), "Id", "Name");
            ViewData["NhanVienId"] = new SelectList(_context.Users, "Id", "Id");

            return View();
        }

        // POST: Admin/SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,NhanVienId,Price,Mota,Hinh,SoLgTn,DateTimeCreate,DateTimeEdit,LoaiId")] SanPham sanPham, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                sanPham.Name = Utilities.ToTitleCase(sanPham.Name);
                if (fThumb != null)
                {
                    string extention = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(sanPham.Name) + extention;
                    sanPham.Hinh = await Utilities.UploadFile(fThumb, @"sanphams", image.ToLower());
                }
                if (string.IsNullOrEmpty(sanPham.Hinh)) sanPham.Hinh = "default.jpg";
              

                sanPham.DateTimeCreate = DateTime.Now;
                sanPham.DateTimeEdit = DateTime.Now;
                _context.Add(sanPham);
                await _context.SaveChangesAsync();
                _notifyService.Success("Thêm sản phẩm thành công!");
                return RedirectToAction(nameof(Index));
            }
            ViewData["LoaiId"] = new SelectList(_context.Set<LoaiSanPham>(), "Id", "Name", sanPham.LoaiId);
            ViewData["NhanVienId"] = new SelectList(_context.Users, "Id", "Id");

            return View(sanPham);
        }

        // GET: Admin/SanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SanPham == null)
                    
            {
                return NotFound();
            }

            var sanPham = await _context.SanPham.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            ViewData["LoaiId"] = new SelectList(_context.Set<LoaiSanPham>(), "Id", "Id", sanPham.LoaiId);
            return View(sanPham);
        }

        // POST: Admin/SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,NhanVienId,Price,Mota,Hinh,SoLgTn,DateTimeCreate,DateTimeEdit,LoaiId")] SanPham sanPham, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (id != sanPham.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    sanPham.Name = Utilities.ToTitleCase(sanPham.Name);
                    if (fThumb != null)
                    {
                        string extention = Path.GetExtension(fThumb.FileName);
                        string image = Utilities.SEOUrl(sanPham.Name) + extention;
                        sanPham.Hinh = await Utilities.UploadFile(fThumb, @"sanphams", image.ToLower());
                    }
                    if (string.IsNullOrEmpty(sanPham.Hinh)) sanPham.Hinh = "default.jpg";

                    sanPham.DateTimeEdit = DateTime.Now;
                    _context.Update(sanPham);
                    _notifyService.Success("Cập nhật sản phẩm thành công!");
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LoaiId"] = new SelectList(_context.Set<LoaiSanPham>(), "Id", "Id", sanPham.LoaiId);
            return View(sanPham);
        }

        // GET: Admin/SanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Admin/SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SanPham == null)
            {
                return Problem("Entity set 'WebLeventContext.SanPham'  is null.");
            }
            var sanPham = await _context.SanPham.FindAsync(id);
            if (sanPham != null)
            {
                _context.SanPham.Remove(sanPham);
            }
            
            await _context.SaveChangesAsync();
            _notifyService.Success("Xóa sản phẩm thành công!");
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(int id)
        {
          return (_context.SanPham?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
