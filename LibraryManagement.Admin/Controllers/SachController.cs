using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.API.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using LibraryManagement.Application.Common;
using Newtonsoft.Json;
using X.PagedList;

namespace LibraryManagement.Admin.Controllers
{
    public class SachController : Controller
    {
        private readonly LibraryDBContext _context;

        public IConfiguration _configuration;

        private dynamic apiAddress;

        public SachController(LibraryDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            apiAddress = _configuration.GetSection("ApiAddress").GetSection("Url").Value;
        }

        // GET: Sach
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TenSachSortParm = sortOrder == "tensach" ? "tensach_desc" : "tensach";
            ViewBag.SoLuongSortParm = sortOrder == "soluong" ? "soluong_desc" : "soluong";

            var list_sach = new List<Sach>();

            HttpResponseMessage respond = await ApiService.GetAPI(apiAddress).GetAsync("/api/sach");

            if (respond.IsSuccessStatusCode)
            {
                var sachJsonString = await respond.Content.ReadAsStringAsync();

                list_sach = JsonConvert.DeserializeObject<IEnumerable<Sach>>(sachJsonString).ToList();
            }

            if (searchString != null)
                page = 1;
            else searchString = currentFilter;
            ViewBag.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                list_sach = list_sach.Where(s => s.TenSach.ToUpper().Contains(searchString.ToUpper())).ToList();
                if (list_sach.Count() > 0)
                {
                    TempData["notice"] = "Have result";
                    TempData["dem"] = list_sach.Count();
                }
                else
                {
                    TempData["notice"] = "No result";
                }
            }
            switch (sortOrder)
            {
                case "tensach":
                    list_sach = list_sach.OrderBy(s => s.TenSach).ToList();
                    break;
                case "tensach_desc":
                    list_sach = list_sach.OrderByDescending(s => s.TenSach).ToList();
                    break;
                case "soluong":
                    list_sach = list_sach.OrderBy(s => s.SoLuong).ToList();
                    break;
                case "soluong_desc":
                    list_sach = list_sach.OrderByDescending(s => s.SoLuong).ToList();
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(list_sach.ToPagedList(pageNumber, pageSize));
        }

        // GET: Sach/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Sach sach = null;

            HttpResponseMessage respond = await ApiService.GetAPI(apiAddress).GetAsync($"/api/sach/{id}");

            if (respond.IsSuccessStatusCode)
            {
                sach = await respond.Content.ReadAsAsync<Sach>();
            }

            if (sach == null)
            {
                return NotFound();
            }

            return View(sach);
        }

        // GET: Sach/Create
        public IActionResult Create()
        {
            ViewData["LoaiSach"] = new SelectList(_context.LoaiSach, "Id", "TenLoai");
            return View();
        }

        // POST: Sach/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenSach,TacGia,NhaXuatBan,NamXuatBan,TongSoTrang,TomTat,LoaiSach,SoLuong,HinhAnh,TrangThai")] Sach sach)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sach);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LoaiSach"] = new SelectList(_context.LoaiSach, "Id", "TenLoai", sach.LoaiSach);
            return View(sach);
        }

        // GET: Sach/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sach = await _context.Sach.FindAsync(id);
            if (sach == null)
            {
                return NotFound();
            }
            ViewData["LoaiSach"] = new SelectList(_context.LoaiSach, "Id", "TenLoai", sach.LoaiSach);
            return View(sach);
        }

        // POST: Sach/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,TenSach,TacGia,NhaXuatBan,NamXuatBan,TongSoTrang,TomTat,LoaiSach,SoLuong,HinhAnh,TrangThai")] Sach sach)
        {
            if (id != sach.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sach);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SachExists(sach.Id))
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
            ViewData["LoaiSach"] = new SelectList(_context.LoaiSach, "Id", "TenLoai", sach.LoaiSach);
            return View(sach);
        }

        // GET: Sach/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sach = await _context.Sach
                .Include(s => s.LoaiSachNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sach == null)
            {
                return NotFound();
            }

            return View(sach);
        }

        // POST: Sach/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var sach = await _context.Sach.FindAsync(id);
            _context.Sach.Remove(sach);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SachExists(string id)
        {
            return _context.Sach.Any(e => e.Id == id);
        }
    }
}
