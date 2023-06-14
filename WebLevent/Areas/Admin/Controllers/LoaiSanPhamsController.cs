﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebLevent.Areas.Identity.Data;
using WebLevent.Models;

namespace WebLevent.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Administrator,Manager")]
    public class LoaiSanPhamsController : Controller
    {
        private readonly WebLeventContext _context;

        public LoaiSanPhamsController(WebLeventContext context)
        {
            _context = context;
        }

        // GET: Admin/LoaiSanPhams
        public async Task<IActionResult> Index()
        {
              return _context.LoaiSanPham != null ? 
                          View(await _context.LoaiSanPham.ToListAsync()) :
                          Problem("Entity set 'WebLeventContext.LoaiSanPham'  is null.");
        }

        // GET: Admin/LoaiSanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LoaiSanPham == null)
            {
                return NotFound();
            }

            var loaiSanPham = await _context.LoaiSanPham
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loaiSanPham == null)
            {
                return NotFound();
            }

            return View(loaiSanPham);
        }

        // GET: Admin/LoaiSanPhams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/LoaiSanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] LoaiSanPham loaiSanPham)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loaiSanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loaiSanPham);
        }

        // GET: Admin/LoaiSanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LoaiSanPham == null)
            {
                return NotFound();
            }

            var loaiSanPham = await _context.LoaiSanPham.FindAsync(id);
            if (loaiSanPham == null)
            {
                return NotFound();
            }
            return View(loaiSanPham);
        }

        // POST: Admin/LoaiSanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] LoaiSanPham loaiSanPham)
        {
            if (id != loaiSanPham.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loaiSanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiSanPhamExists(loaiSanPham.Id))
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
            return View(loaiSanPham);
        }

        // GET: Admin/LoaiSanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LoaiSanPham == null)
            {
                return NotFound();
            }

            var loaiSanPham = await _context.LoaiSanPham
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loaiSanPham == null)
            {
                return NotFound();
            }

            return View(loaiSanPham);
        }

        // POST: Admin/LoaiSanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LoaiSanPham == null)
            {
                return Problem("Entity set 'WebLeventContext.LoaiSanPham'  is null.");
            }
            var loaiSanPham = await _context.LoaiSanPham.FindAsync(id);
            if (loaiSanPham != null)
            {
                _context.LoaiSanPham.Remove(loaiSanPham);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoaiSanPhamExists(int id)
        {
          return (_context.LoaiSanPham?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
