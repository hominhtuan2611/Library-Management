using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.API.Models;
using X.PagedList;

namespace LibraryManagement.Admin.Controllers
{
    public class PhieuNhapController : Controller
    {
        private readonly LibraryDBContext _context;

        public PhieuNhapController(LibraryDBContext context)
        {
            _context = context;
        }

        // GET: PhieuNhap
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.MaPNSortParm = string.IsNullOrEmpty(sortOrder) ? "mapn" : "";
            ViewBag.NgayNhapSortParm = sortOrder == "ngaynhap" ? "ngaynhap_desc" : "ngaynhap";

            var list_phieunhap = await _context.PhieuNhap.Where(x => x.TrangThai == 1).Include(p => p.NhanVienNhapNavigation).ToListAsync();

            if (searchString != null)
                page = 1;
            else searchString = currentFilter;
            ViewBag.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                list_phieunhap = list_phieunhap.Where(s => s.NhanVienNhapNavigation.TenNv.ToUpper().Contains(searchString.ToUpper())).ToList();
                if (list_phieunhap.Count() > 0)
                {
                    TempData["notice"] = "Have result";
                    TempData["dem"] = list_phieunhap.Count();
                }
                else
                {
                    TempData["notice"] = "No result";
                }
            }
            switch (sortOrder)
            {
                case "mapn":
                    list_phieunhap = list_phieunhap.OrderBy(s => s.Id).ToList();
                    break;
                case "ngaynhap":
                    list_phieunhap = list_phieunhap.OrderBy(s => s.NgayNhap).ToList();
                    break;
                case "ngaynhap_desc":
                    list_phieunhap = list_phieunhap.OrderByDescending(s => s.NgayNhap).ToList();
                    break;
                default:
                    list_phieunhap = list_phieunhap.OrderByDescending(s => s.Id).ToList();
                    break;

            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(list_phieunhap.ToPagedList(pageNumber, pageSize));
        }

        // GET: PhieuNhap/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phieuNhap = await _context.PhieuNhap
                .Include(p => p.NhanVienNhapNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phieuNhap == null)
            {
                return NotFound();
            }

            return View(phieuNhap);
        }

        // GET: PhieuNhap/Create
        public IActionResult Create()
        {
            ViewData["NhanVienNhap"] = new SelectList(_context.NhanVien.Where(x => x.ViTri != "Admin"), "Id", "TenNv");
            return View();
        }

        // POST: PhieuNhap/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NgayNhap,SoLuong,NhaCungCap,NhanVienNhap,TrangThai")] PhieuNhap phieuNhap)
        {
            if (ModelState.IsValid)
            {
                phieuNhap.NgayNhap = DateTime.Now;
                phieuNhap.SoLuong = 0;
                phieuNhap.TrangThai = 1;

                _context.Add(phieuNhap);
                await _context.SaveChangesAsync();

                TempData["notice"] = "Successfully create";
                TempData["phieunhap"] = phieuNhap.NhaCungCap + "-" + phieuNhap.NgayNhap.ToShortDateString();

                return RedirectToAction(nameof(Index));
            }
            ViewData["NhanVienNhap"] = new SelectList(_context.NhanVien.Where(x => x.ViTri != "Admin"), "Id", "TenNv", phieuNhap.NhanVienNhap);
            return View(phieuNhap);
        }

        // GET: PhieuNhap/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phieuNhap = await _context.PhieuNhap.FindAsync(id);
            if (phieuNhap == null)
            {
                return NotFound();
            }
            ViewData["NhanVienNhap"] = new SelectList(_context.NhanVien.Where(x => x.ViTri != "Admin"), "Id", "TenNv", phieuNhap.NhanVienNhap);
            return View(phieuNhap);
        }

        // POST: PhieuNhap/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NgayNhap,SoLuong,NhaCungCap,NhanVienNhap,TrangThai")] PhieuNhap phieuNhap)
        {
            if (id != phieuNhap.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phieuNhap);
                    await _context.SaveChangesAsync();

                    TempData["notice"] = "Successfully edit";
                    TempData["phieunhap"] = phieuNhap.NhaCungCap + "-" + phieuNhap.NgayNhap.ToShortDateString();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhieuNhapExists(phieuNhap.Id))
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
            ViewData["NhanVienNhap"] = new SelectList(_context.NhanVien.Where(x => x.ViTri != "Admin"), "Id", "TenNv", phieuNhap.NhanVienNhap);
            return View(phieuNhap);
        }

        // GET: PhieuNhap/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phieuNhap = await _context.PhieuNhap
                .Include(p => p.NhanVienNhapNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phieuNhap == null)
            {
                return NotFound();
            }

            return View(phieuNhap);
        }

        // POST: PhieuNhap/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phieuNhap = await _context.PhieuNhap.FindAsync(id);
            phieuNhap.TrangThai = 0;

            await _context.SaveChangesAsync();

            TempData["notice"] = "Successfully delete";
            TempData["phieunhap"] = phieuNhap.NhaCungCap + "-" + phieuNhap.NgayNhap.ToShortDateString();

            return RedirectToAction(nameof(Index));
        }

        private bool PhieuNhapExists(int id)
        {
            return _context.PhieuNhap.Any(e => e.Id == id);
        }
    }
}
