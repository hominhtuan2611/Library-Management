using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.API.Models;
using X.PagedList;
using LibraryManagement.Application.Common;

namespace LibraryManagement.Admin.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly LibraryDBContext _context;

        public NhanVienController(LibraryDBContext context)
        {
            _context = context;
        }

        // GET: NhanViens
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";

            var list_nhanvien = await _context.NhanVien.Where(x => x.ViTri != "Admin").ToListAsync();

            if (searchString != null)
                page = 1;
            else searchString = currentFilter;
            ViewBag.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                list_nhanvien = list_nhanvien.Where(s => s.TenNv.ToUpper().Contains(searchString.ToUpper())).ToList();
                if (list_nhanvien.Count() > 0)
                {
                    TempData["notice"] = "Have result";
                    TempData["dem"] = list_nhanvien.Count();
                }
                else
                {
                    TempData["notice"] = "No result";
                }
            }
            switch (sortOrder)
            {
                case "name_desc":
                    list_nhanvien = list_nhanvien.OrderByDescending(s => s.TenNv).ToList();
                    break;
                case "Date":
                    list_nhanvien = list_nhanvien.OrderBy(s => s.NgaySinh).ToList();
                    break;
                case "date_desc":
                    list_nhanvien = list_nhanvien.OrderByDescending(s => s.NgaySinh).ToList();
                    break;
                default:
                    list_nhanvien = list_nhanvien.OrderBy(s => s.TenNv).ToList();
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(list_nhanvien.ToPagedList(pageNumber, pageSize));
        }

        // GET: NhanViens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanVien
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        // GET: NhanViens/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NhanViens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenNv,GioiTinh,NgaySinh,Cmnd,DiaChi,Sdt,ViTri,Username,Password")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                nhanVien.Password = Password_Encryptor.HashSHA1(nhanVien.Password);
                nhanVien.TrangThai = true;

                _context.Add(nhanVien);
                await _context.SaveChangesAsync();

                TempData["notice"] = "Successfully create";
                TempData["nhanvien"] = nhanVien.TenNv;

                return RedirectToAction(nameof(Index));
            }
            return View(nhanVien);
        }

        // GET: NhanViens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanVien.FindAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return View(nhanVien);
        }

        // POST: NhanViens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenNv,GioiTinh,NgaySinh,Cmnd,DiaChi,Sdt,ViTri,Username,Password,TrangThai")] NhanVien nhanVien)
        {
            if (id != nhanVien.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhanVien);
                    await _context.SaveChangesAsync();

                    TempData["notice"] = "Successfully edit";
                    TempData["nhanvien"] = nhanVien.TenNv;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhanVienExists(nhanVien.Id))
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
            return View(nhanVien);
        }

        // GET: NhanViens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanVien
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        // POST: NhanViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhanVien = await _context.NhanVien.FindAsync(id);
            nhanVien.TrangThai = false;
            await _context.SaveChangesAsync();

            TempData["notice"] = "Successfully delete";
            TempData["nhanvien"] = nhanVien.TenNv;

            return RedirectToAction(nameof(Index));
        }

        private bool NhanVienExists(int id)
        {
            return _context.NhanVien.Any(e => e.Id == id);
        }
    }
}
