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
using System.Data.SqlClient;
using X.PagedList;

namespace LibraryManagement.Admin.Controllers
{
    public class CtphieuMuonController : Controller
    {
        private readonly LibraryDBContext _context;

        public IConfiguration _configuration;

        private HttpClient _apiService;
        private readonly string apiAddress;

        public CtphieuMuonController(LibraryDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

            apiAddress = _configuration.GetSection("ApiAddress").GetSection("Url").Value;
            _apiService = ApiService.GetAPI(apiAddress);
        }

        // GET: CtphieuMuon
        public async Task<IActionResult> Index(int phieuMuonId, string sortOrder, string currentFilter, string searchString, int? page)
        {
            PhieuMuon phieuMuon = _context.PhieuMuon.Where(s => s.Id == phieuMuonId).FirstOrDefault();
            ViewBag.TongSoLuong = phieuMuon.TongSachMuon;
            ViewBag.NgayMuon = phieuMuon.NgayMuon;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SoLuongSortParm = string.IsNullOrEmpty(sortOrder) ? "slsp_desc" : "";

            var list_chitietphieumuon = await _context.CtphieuMuon.Where(x => x.PhieuMuon == phieuMuonId).Include(c => c.BookNavigation).Include(c => c.PhieuMuonNavigation).Where(x => x.PhieuMuon == phieuMuonId).ToListAsync();

            ViewBag.MaPM = phieuMuonId;

            if (searchString != null) page = 1;
            else searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                list_chitietphieumuon = list_chitietphieumuon.Where(s => s.BookNavigation.TenSach.ToUpper().Contains(searchString.ToUpper())).ToList();
                if (list_chitietphieumuon.Count() > 0)
                {
                    TempData["notice"] = "Have result";
                    TempData["dem"] = list_chitietphieumuon.Count();
                }
                else
                {
                    TempData["notice"] = "No result";
                }
            }
            switch (sortOrder)
            {
                case "slsp_desc":
                    list_chitietphieumuon = list_chitietphieumuon.OrderByDescending(s => s.SoLuong).ToList();
                    break;
                default:
                    list_chitietphieumuon = list_chitietphieumuon.OrderBy(s => s.SoLuong).ToList();
                    break;

            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(list_chitietphieumuon.ToPagedList(pageNumber, pageSize));
        }

        // GET: CtphieuMuon/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ctphieuMuon = await _context.CtphieuMuon
                .Include(c => c.BookNavigation)
                .Include(c => c.PhieuMuonNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ctphieuMuon == null)
            {
                return NotFound();
            }

            return View(ctphieuMuon);
        }

        // GET: CtphieuMuon/Create
        public IActionResult Create(int PhieuMuonId)
        {
            CtphieuMuon chitietphieumuon = new CtphieuMuon();

            chitietphieumuon.PhieuMuon = PhieuMuonId;
            ViewData["Book"] = new SelectList(_context.Sach, "Id", "TenSach");
            ViewData["PhieuMuon"] = new SelectList(_context.PhieuMuon, "Id", "Id");
            return View(chitietphieumuon);
        }

        // POST: CtphieuMuon/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PhieuMuon,Book,SoLuong,NgayMuon,NgayTra,TinhTrangSach")] CtphieuMuon ctphieuMuon)
        {
            if (ModelState.IsValid)
            {
                
                var PhieuMuon = _context.PhieuMuon.Where(p => p.Id == ctphieuMuon.PhieuMuon).FirstOrDefault();
                ctphieuMuon.NgayMuon = PhieuMuon.NgayMuon;
                _context.Add(ctphieuMuon);
                PhieuMuon.TongSachMuon += ctphieuMuon.SoLuong;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { phieuMuonId = ctphieuMuon.PhieuMuon });
            }
            ViewData["Book"] = new SelectList(_context.Sach, "Id", "Id", ctphieuMuon.Book);
            ViewData["PhieuMuon"] = new SelectList(_context.PhieuMuon, "Id", "Id", ctphieuMuon.PhieuMuon);
            return View(ctphieuMuon);
        }

        // GET: CtphieuMuon/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ctphieuMuon = await _context.CtphieuMuon.FindAsync(id);
            if (ctphieuMuon == null)
            {
                return NotFound();
            }
            ViewData["Book"] = new SelectList(_context.Sach, "Id", "TenSach", ctphieuMuon.Book);
            ViewData["PhieuMuon"] = new SelectList(_context.PhieuMuon, "Id", "Id", ctphieuMuon.PhieuMuon);
            return View(ctphieuMuon);
        }

        // POST: CtphieuMuon/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PhieuMuon,Book,SoLuong,NgayMuon,NgayTra,TinhTrangSach")] CtphieuMuon ctphieuMuon)
        {
            int slpm_cu = _context.CtphieuMuon.Find(id).SoLuong;
            ViewBag.SoLuongSPCu = slpm_cu;
            if (id != ctphieuMuon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {            
                var entity = _context.CtphieuMuon.Where(c => c.Id == ctphieuMuon.Id).AsQueryable().FirstOrDefault();
                _context.Entry(entity).CurrentValues.SetValues(ctphieuMuon);

                @TempData["notice"] = "Successfully edit";
                @TempData["ctpm"] = ctphieuMuon.PhieuMuon;
                @TempData["masp"] = ctphieuMuon.Book;

                Sach sach = _context.Sach.Where(m => m.Id == ctphieuMuon.Book).FirstOrDefault();
                sach.SoLuong = sach.SoLuong + (ctphieuMuon.SoLuong - slpm_cu);

                PhieuMuon pm = _context.PhieuMuon.SingleOrDefault(m => m.Id == ctphieuMuon.PhieuMuon);
                pm.TongSachMuon = pm.TongSachMuon + (ctphieuMuon.SoLuong - slpm_cu);
                _context.Entry(pm).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new { phieuMuonId = ctphieuMuon.PhieuMuon });
            }
            ViewData["Book"] = new SelectList(_context.Sach, "Id", "Id", ctphieuMuon.Book);
            ViewData["PhieuMuon"] = new SelectList(_context.PhieuMuon, "Id", "Id", ctphieuMuon.PhieuMuon);
            return View(ctphieuMuon);
        }

        // GET: CtphieuMuon/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ctphieuMuon = await _context.CtphieuMuon
                .Include(c => c.BookNavigation)
                .Include(c => c.PhieuMuonNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ctphieuMuon == null)
            {
                return NotFound();
            }

            return View(ctphieuMuon);
        }

        // POST: CtphieuMuon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ctphieuMuon = await _context.CtphieuMuon.FindAsync(id);
            _context.CtphieuMuon.Remove(ctphieuMuon);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { phieuMuonId = ctphieuMuon.PhieuMuon }); ;
        }

        private bool CtphieuMuonExists(int id)
        {
            return _context.CtphieuMuon.Any(e => e.Id == id);
        }
    }
}
