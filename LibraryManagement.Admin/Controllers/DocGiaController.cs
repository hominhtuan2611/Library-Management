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
    public class DocGiaController : Controller
    {
        private readonly LibraryDBContext _context;

        public DocGiaController(LibraryDBContext context)
        {
            _context = context;
        }

        // GET: DocGia
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";

            var list_docgia = await _context.DocGia.ToListAsync();

            if (searchString != null)
                page = 1;
            else searchString = currentFilter;
            ViewBag.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                list_docgia = list_docgia.Where(s => s.TenDg.ToUpper().Contains(searchString.ToUpper())).ToList();
                if (list_docgia.Count() > 0)
                {
                    TempData["notice"] = "Have result";
                    TempData["dem"] = list_docgia.Count();
                }
                else
                {
                    TempData["notice"] = "No result";
                }
            }
            switch (sortOrder)
            {
                case "name_desc":
                    list_docgia = list_docgia.OrderByDescending(s => s.TenDg).ToList();
                    break;
                case "Date":
                    list_docgia = list_docgia.OrderBy(s => s.NgaySinh).ToList();
                    break;
                case "date_desc":
                    list_docgia = list_docgia.OrderByDescending(s => s.NgaySinh).ToList();
                    break;
                default:
                    list_docgia = list_docgia.OrderBy(s => s.TenDg).ToList();
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(list_docgia.ToPagedList(pageNumber, pageSize));
        }

        // GET: DocGia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docGia = await _context.DocGia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (docGia == null)
            {
                return NotFound();
            }

            return View(docGia);
        }

        // GET: DocGia/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DocGia/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenDg,GioiTinh,NgaySinh,Cmnd,DiaChi,Sdt,SoLanViPham,NgayDangKy,Username,Password")] DocGia docGia)
        {
            if (ModelState.IsValid)
            {
                docGia.NgayDangKy = DateTime.Now;
                docGia.Password = Password_Encryptor.HashSHA1(docGia.Password);
                docGia.TrangThai = true;

                _context.Add(docGia);
                await _context.SaveChangesAsync();

                TempData["notice"] = "Successfully create";
                TempData["docgia"] = docGia.TenDg;

                return RedirectToAction(nameof(Index));
            }
            return View(docGia);
        }

        // GET: DocGia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docGia = await _context.DocGia.FindAsync(id);
            if (docGia == null)
            {
                return NotFound();
            }
            return View(docGia);
        }

        // POST: DocGia/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenDg,GioiTinh,NgaySinh,Cmnd,DiaChi,Sdt,SoLanViPham,NgayDangKy,Username,Password,TrangThai")] DocGia docGia)
        {
            if (id != docGia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(docGia);
                    await _context.SaveChangesAsync();

                    TempData["notice"] = "Successfully edit";
                    TempData["docgia"] = docGia.TenDg;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocGiaExists(docGia.Id))
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
            return View(docGia);
        }

        // GET: DocGia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docGia = await _context.DocGia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (docGia == null)
            {
                return NotFound();
            }

            return View(docGia);
        }

        // POST: DocGia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var docGia = await _context.DocGia.FindAsync(id);
            docGia.TrangThai = false;
            await _context.SaveChangesAsync();

            TempData["notice"] = "Successfully delete";
            TempData["docgia"] = docGia.TenDg;

            return RedirectToAction(nameof(Index));
        }

        private bool DocGiaExists(int id)
        {
            return _context.DocGia.Any(e => e.Id == id);
        }
    }
}
