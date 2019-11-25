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
            PhieuMuon phieuMuon = await _apiService.GetAsync($"api/phieuMuon/{phieuMuonId}").Result.Content.ReadAsAsync<PhieuMuon>();
            ViewData["phieuMuon"] = phieuMuon;
            ViewBag.NgayMuon = phieuMuon.NgayMuon.ToShortDateString();
            ViewBag.HanTra = phieuMuon.HanTra.ToShortDateString();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SoLuongSortParm = string.IsNullOrEmpty(sortOrder) ? "slsp_desc" : "";

            var list_chitietphieumuon = await _apiService.GetAsync($"api/ctPhieuMuon/{phieuMuon.Id}").Result.Content.ReadAsAsync<List<CtphieuMuon>>();

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
                
                var phieuMuon = await _apiService.GetAsync($"api/phieuMuon/{ctphieuMuon.PhieuMuon}").Result.Content.ReadAsAsync<PhieuMuon>();
                var sach=  await _apiService.GetAsync($"api/sach/{ctphieuMuon.Book}").Result.Content.ReadAsAsync<Sach>();
                if(sach.SoLuong >= ctphieuMuon.SoLuong)
                {
                    phieuMuon.TongSachMuon += ctphieuMuon.SoLuong;
                    phieuMuon.MaDgNavigation = null;
                    phieuMuon.MaNvNavigation = null;
                    sach.SoLuong -= ctphieuMuon.SoLuong;

                    CtphieuMuon new_ctPhieuMuon = await _apiService.PostAsJsonAsync("api/ctPhieuMuon", ctphieuMuon).Result.Content.ReadAsAsync<CtphieuMuon>();
                    HttpResponseMessage respond_sach = await _apiService.PutAsJsonAsync($"api/sach/{sach.Id}", sach);
                    respond_sach.EnsureSuccessStatusCode();
                    HttpResponseMessage respond_phieuMuon = await _apiService.PutAsJsonAsync($"api/phieuMuon/{phieuMuon.Id}", phieuMuon);
                    respond_phieuMuon.EnsureSuccessStatusCode();
                   
                    @TempData["notice"] = "Successfully create";
                    @TempData["tensp"] = new_ctPhieuMuon.BookNavigation.TenSach;
                    @TempData["masp"] = new_ctPhieuMuon.Book;
                    return RedirectToAction("Index", new { phieuMuonId = ctphieuMuon.PhieuMuon });
                }
                @TempData["notice"] = "Error create";
                @TempData["tensp"] = sach.TenSach;
                @TempData["masp"] = sach.Id;
            }
            ViewData["Book"] = new SelectList(_context.Sach, "Id", "TenSach", ctphieuMuon.Book);
            return View(ctphieuMuon);
        }

        // GET: CtphieuMuon/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ctphieuMuon = await _apiService.GetAsync($"api/ctPhieuMuon/Detail/{id}").Result.Content.ReadAsAsync<CtphieuMuon>();
            if (ctphieuMuon == null)
            {
                return NotFound();
            }
            ViewData["Book"] = new SelectList(_context.Sach, "Id", "TenSach", ctphieuMuon.Book);
            return View(ctphieuMuon);
        }

        // POST: CtphieuMuon/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PhieuMuon,Book,SoLuong,NgayMuon,NgayTra,TinhTrangSach")] CtphieuMuon ctphieuMuon)
        {
            var ctPhieMuon_cu = await _apiService.GetAsync($"api/ctPhieuMuon/Detail/{id}").Result.Content.ReadAsAsync<CtphieuMuon>();
            int slpm_cu = ctPhieMuon_cu.SoLuong;
            ViewBag.SoLuongSPCu = slpm_cu;
            if (id != ctphieuMuon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Sach sach = await _apiService.GetAsync($"api/sach/{ctphieuMuon.Book}").Result.Content.ReadAsAsync<Sach>();
                int slSachThem= ctphieuMuon.SoLuong - slpm_cu;
                if(sach.SoLuong>slSachThem)
                {
                    CtphieuMuon entity = await _apiService.PutAsJsonAsync($"api/ctPhieuMuon/{id}", ctphieuMuon).Result.Content.ReadAsAsync<CtphieuMuon>();
                    sach.SoLuong = sach.SoLuong - slSachThem;
                    PhieuMuon phieuMuon = await _apiService.GetAsync($"api/phieuMuon/{ctphieuMuon.PhieuMuon}").Result.Content.ReadAsAsync<PhieuMuon>();
                    phieuMuon.TongSachMuon = phieuMuon.TongSachMuon + slSachThem;
                    phieuMuon.MaDgNavigation = null;
                    phieuMuon.MaNvNavigation = null;
                    HttpResponseMessage respond_sach = await _apiService.PutAsJsonAsync($"api/sach/{sach.Id}", sach);
                    respond_sach.EnsureSuccessStatusCode();
                    HttpResponseMessage respond_phieuMuon = await _apiService.PutAsJsonAsync($"api/phieuMuon/{phieuMuon.Id}", phieuMuon);
                    respond_phieuMuon.EnsureSuccessStatusCode();

                    @TempData["notice"] = "Successfully edit";
                    @TempData["masp"] = entity.Book;
                    @TempData["tensp"] = entity.BookNavigation.TenSach;
                    return RedirectToAction("Index", new { phieuMuonId = ctphieuMuon.PhieuMuon });
                }
                @TempData["notice"] = "Error edit";
                @TempData["tesp"] = ctPhieMuon_cu.BookNavigation.TenSach;
                @TempData["masp"] = ctPhieMuon_cu.Book;
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

            var ctphieuMuon = await _apiService.GetAsync($"api/ctPhieuMuon/Detail/{id}").Result.Content.ReadAsAsync<CtphieuMuon>();
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
            var ctphieuMuon = await _apiService.GetAsync($"api/ctPhieuMuon/Detail/{id}").Result.Content.ReadAsAsync<CtphieuMuon>();
            var phieuMuon = await _apiService.GetAsync($"api/phieuMuon/{ctphieuMuon.PhieuMuon}").Result.Content.ReadAsAsync<PhieuMuon>();
            phieuMuon.TongSachMuon -= ctphieuMuon.SoLuong;
            Sach sach = await _apiService.GetAsync($"api/sach/{ctphieuMuon.Book}").Result.Content.ReadAsAsync<Sach>();
            sach.SoLuong = sach.SoLuong + ctphieuMuon.SoLuong;
            phieuMuon.MaNvNavigation = null;
            phieuMuon.MaDgNavigation = null;
            ctphieuMuon.BookNavigation = null;
            ctphieuMuon.PhieuMuonNavigation = null;

            HttpResponseMessage respond = await _apiService.DeleteAsync($"api/ctPhieuMuon/{id}");
            respond.EnsureSuccessStatusCode();
            HttpResponseMessage respond_phieuMuon = await _apiService.PutAsJsonAsync($"api/phieuMuon/{phieuMuon.Id}", phieuMuon);
            respond_phieuMuon.EnsureSuccessStatusCode();
            HttpResponseMessage respond_sach = await _apiService.PutAsJsonAsync($"api/sach/{sach.Id}", sach);
            respond_sach.EnsureSuccessStatusCode();

            return RedirectToAction("Index", new { phieuMuonId = ctphieuMuon.PhieuMuon }); ;
        }

        private async Task<bool> CtphieuMuonExists(int id)
        {
            var list_ctPhieuMuon = await _apiService.GetAsync($"api/ctPhieuMuon/{id}").Result.Content.ReadAsAsync<List<CtphieuMuon>>();
            return list_ctPhieuMuon.Any(e => e.Id == id);
        }
    }
}
