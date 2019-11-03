using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.API.Models;
using System.Net.Http;
using LibraryManagement.Application.Common;
using Microsoft.Extensions.Configuration;
using X.PagedList;
using Newtonsoft.Json;

namespace LibraryManagement.Admin.Controllers
{
    public class CtphieuNhapController : Controller
    {
        private readonly LibraryDBContext _context;

        public IConfiguration _configuration;

        private HttpClient _apiService;
        private readonly string apiAddress;

        public CtphieuNhapController(LibraryDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            apiAddress = _configuration.GetSection("ApiAddress").GetSection("Url").Value;
            _apiService = ApiService.GetAPI(apiAddress);
        }

        // GET: CtphieuNhap
        public async Task<IActionResult> Index(int phieuNhapId, string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.TongSoLuong = _context.PhieuNhap.Where(s => s.Id == phieuNhapId).FirstOrDefault().SoLuong;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SoLuongSortParm = string.IsNullOrEmpty(sortOrder) ? "slsp_desc" : "";

            var list_chitietphieunhap = await _context.CtphieuNhap.Where(x => x.PhieuNhap == phieuNhapId).Include(c => c.BookNavigation).Include(c => c.PhieuNhapNavigation).Where(x => x.PhieuNhap == phieuNhapId).ToListAsync();

            ViewBag.MaPN = phieuNhapId;

            if (searchString != null) page = 1;
            else searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                list_chitietphieunhap = list_chitietphieunhap.Where(s => s.BookNavigation.TenSach.ToUpper().Contains(searchString.ToUpper())).ToList();
                if (list_chitietphieunhap.Count() > 0)
                {
                    TempData["notice"] = "Have result";
                    TempData["dem"] = list_chitietphieunhap.Count();
                }
                else
                {
                    TempData["notice"] = "No result";
                }
            }
            switch (sortOrder)
            {
                case "slsp_desc":
                    list_chitietphieunhap = list_chitietphieunhap.OrderByDescending(s => s.SoLuong).ToList();
                    break;
                default:
                    list_chitietphieunhap = list_chitietphieunhap.OrderBy(s => s.SoLuong).ToList();
                    break;

            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(list_chitietphieunhap.ToPagedList(pageNumber, pageSize));
        }

        // GET: CtphieuNhap/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ctphieuNhap = await _context.CtphieuNhap
                .Include(c => c.BookNavigation)
                .Include(c => c.PhieuNhapNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ctphieuNhap == null)
            {
                return NotFound();
            }

            return View(ctphieuNhap);
        }

        // GET: CtphieuNhap/Create
        public async Task<IActionResult> Create(int phieuNhapId)
        {
            CtphieuNhap chitietphieunhap = new CtphieuNhap();

            chitietphieunhap.PhieuNhap = phieuNhapId;

            var list_sach = new List<Sach>();

            HttpResponseMessage respond = await _apiService.GetAsync("api/sach");

            if (respond.IsSuccessStatusCode)
            {
                var sachJsonString = await respond.Content.ReadAsStringAsync();

                list_sach = JsonConvert.DeserializeObject<IEnumerable<Sach>>(sachJsonString).ToList();
            }

            Sach sach = list_sach.Where(x => x.TrangThai == true).FirstOrDefault();

            TempData["sltmacdinh"] = Convert.ToInt32(sach.SoLuong);

            ViewData["Book"] = new SelectList(list_sach, "Id", "TenSach");
            return View(chitietphieunhap);
        }

        // POST: CtphieuNhap/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PhieuNhap,Book,SoLuong,TinhTrangSach")] CtphieuNhap ctphieuNhap)
        {
            var list_sach = new List<Sach>();

            HttpResponseMessage respond = await _apiService.GetAsync("api/sach");

            if (respond.IsSuccessStatusCode)
            {
                var sachJsonString = await respond.Content.ReadAsStringAsync();

                list_sach = JsonConvert.DeserializeObject<IEnumerable<Sach>>(sachJsonString).ToList();
            }

            if (ModelState.IsValid)
            {
                ctphieuNhap.TinhTrangSach = "Tốt";
                
                try
                {
                    _context.CtphieuNhap.Add(ctphieuNhap);
                    await _context.SaveChangesAsync();

                    Sach up_sltsp = list_sach.SingleOrDefault(m => m.Id == ctphieuNhap.Book);
                    up_sltsp.SoLuong += ctphieuNhap.SoLuong;

                    respond = await _apiService.PutAsJsonAsync($"api/sach/{up_sltsp.Id}", up_sltsp);

                    respond.EnsureSuccessStatusCode();

                    PhieuNhap uppn_soluong = _context.PhieuNhap.SingleOrDefault(m => m.Id == ctphieuNhap.PhieuNhap);
                    uppn_soluong.SoLuong += ctphieuNhap.SoLuong;
                    _context.Entry(uppn_soluong).State = EntityState.Modified;
                    _context.SaveChanges();

                    TempData["notice"] = "Successfully create";
                    TempData["masp"] = ctphieuNhap.Book;

                    Sach giveten = _context.Sach.Where(s => s.Id == ctphieuNhap.Book).FirstOrDefault();
                    TempData["tensp"] = giveten.TenSach;
                    ViewBag.MaSP = new SelectList(list_sach.Where(s => s.TrangThai == true), "Id", "TenSach");
                    return RedirectToAction("Index", new { phieuNhapId = ctphieuNhap.PhieuNhap });
                }
                catch (Exception e)
                {
                    TempData["masperror"] = ctphieuNhap.Book;
                    TempData["trungmasp"] = "trungmasp";
                    ViewBag.MaSP = new SelectList(list_sach.Where(s => s.TrangThai == true), "Id", "TenSach");
                    return View(ctphieuNhap);
                }
            }
            ViewData["Book"] = new SelectList(list_sach.Where(s => s.TrangThai == true), "Id", "TenSach");
            return View(ctphieuNhap);
        }

        // GET: CtphieuNhap/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ctphieuNhap = await _context.CtphieuNhap.FindAsync(id);
            if (ctphieuNhap == null)
            {
                return NotFound();
            }

            var list_sach = new List<Sach>();

            HttpResponseMessage respond = await _apiService.GetAsync("api/sach");

            if (respond.IsSuccessStatusCode)
            {
                var sachJsonString = await respond.Content.ReadAsStringAsync();

                list_sach = JsonConvert.DeserializeObject<IEnumerable<Sach>>(sachJsonString).ToList();
            }

            ViewData["Book"] = new SelectList(list_sach, "Id", "TenSach", ctphieuNhap.Book);
            return View(ctphieuNhap);
        }

        // POST: CtphieuNhap/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PhieuNhap,Book,SoLuong,TinhTrangSach")] CtphieuNhap ctphieuNhap)
        {
            var list_sach = new List<Sach>();

            HttpResponseMessage respond = await _apiService.GetAsync("api/sach");

            if (respond.IsSuccessStatusCode)
            {
                var sachJsonString = await respond.Content.ReadAsStringAsync();

                list_sach = JsonConvert.DeserializeObject<IEnumerable<Sach>>(sachJsonString).ToList();
            }

            int slpn_cu = (_context.CtphieuNhap.Find(id)).SoLuong;
            ViewBag.SoLuongSPCu = slpn_cu;

            if (id != ctphieuNhap.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var entity = _context.CtphieuNhap.Where(c => c.Id == ctphieuNhap.Id).AsQueryable().FirstOrDefault();
                _context.Entry(entity).CurrentValues.SetValues(ctphieuNhap);
                _context.SaveChanges();

                @TempData["notice"] = "Successfully edit";
                @TempData["ctpn"] = ctphieuNhap.PhieuNhap;
                @TempData["masp"] = ctphieuNhap.Book;

                Sach sach = list_sach.SingleOrDefault(m => m.Id == ctphieuNhap.Book);
                sach.SoLuong = sach.SoLuong + (ctphieuNhap.SoLuong - slpn_cu);

                respond = await _apiService.PutAsJsonAsync($"api/sach/{sach.Id}", sach);

                respond.EnsureSuccessStatusCode();

                PhieuNhap pn = _context.PhieuNhap.SingleOrDefault(m => m.Id == ctphieuNhap.PhieuNhap);
                pn.SoLuong = pn.SoLuong + (ctphieuNhap.SoLuong - slpn_cu);
                _context.Entry(pn).State = EntityState.Modified;
                _context.SaveChanges();

                return RedirectToAction("Index", new { phieuNhapId = ctphieuNhap.PhieuNhap });
            }
            ViewData["Book"] = new SelectList(list_sach, "Id", "TenSach", ctphieuNhap.Book);
            return View(ctphieuNhap);
        }

        // GET: CtphieuNhap/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ctphieuNhap = await _context.CtphieuNhap
                .Include(c => c.BookNavigation)
                .Include(c => c.PhieuNhapNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ctphieuNhap == null)
            {
                return NotFound();
            }

            return View(ctphieuNhap);
        }

        // POST: CtphieuNhap/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ctphieuNhap = await _context.CtphieuNhap.FindAsync(id);

            int sl = ctphieuNhap.SoLuong;

            @TempData["notice"] = "Successfully delete";
            @TempData["ctpn"] = ctphieuNhap.PhieuNhap;
            @TempData["masp"] = ctphieuNhap.Book;

            Sach sach = null;

            HttpResponseMessage respond = await _apiService.GetAsync($"api/sach/{ctphieuNhap.Book}");

            if (respond.IsSuccessStatusCode)
            {
                sach = await respond.Content.ReadAsAsync<Sach>();
            }

            sach.SoLuong -= sl;

            if (sach.SoLuong >= 0)
            {
                respond = await _apiService.PutAsJsonAsync($"api/sach/{sach.Id}", sach);

                respond.EnsureSuccessStatusCode();

                PhieuNhap pn = _context.PhieuNhap.SingleOrDefault(m => m.Id == ctphieuNhap.PhieuNhap);
                pn.SoLuong -= ctphieuNhap.SoLuong;
                _context.Entry(pn).State = EntityState.Modified;
                _context.SaveChanges();

                _context.CtphieuNhap.Remove(ctphieuNhap);
                await _context.SaveChangesAsync();
            }
            else
            {
                ViewBag.ErrorSLTon = "errorslt";
            }
            return RedirectToAction("Index", new { phieuNhapId = ctphieuNhap.PhieuNhap });
        }

        private bool CtphieuNhapExists(int id)
        {
            return _context.CtphieuNhap.Any(e => e.Id == id);
        }
    }
}
