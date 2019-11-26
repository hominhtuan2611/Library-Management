using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LibraryManagement.Admin.Models.Report;
using LibraryManagement.API.Models;
using LibraryManagement.Application.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using X.PagedList;

namespace LibraryManagement.Admin.Controllers
{
    public class ThongKeController : Controller
    {
        public IConfiguration _configuration;

        private HttpClient _apiService;
        private readonly string apiAddress;

        public ThongKeController(IConfiguration configuration)
        {
            _configuration = configuration;

            apiAddress = _configuration.GetSection("ApiAddress").GetSection("Url").Value;
            _apiService = ApiService.GetAPI(apiAddress);
        }

        public async Task<IActionResult> TongPhieuMuon()
        {
            var list_phieumuon = await _apiService.GetAsync("api/phieumuon").Result.Content.ReadAsAsync<List<PhieuMuon>>();

            List<string> months = new List<string>();

            for (int i = 1; i <= 12; i++)
            {
                months.Add(i.ToString());
            }

            List<decimal> tongphieumuontheothang = new List<decimal>();

            foreach (var a in months)
            {
                var phieumuoncuathang = list_phieumuon.Where(x => x.NgayMuon.Month == Convert.ToInt32(a)).Count();

                tongphieumuontheothang.Add(phieumuoncuathang);
            }

            TongPhieuMuonModel tongphieumuon = new TongPhieuMuonModel();

            tongphieumuon.Months = months;
            tongphieumuon.tongPhieuMuonTheoThang = tongphieumuontheothang;

            return View(tongphieumuon);
        }

        public async Task<IActionResult> SachMuonNhieu()
        {
            var list_phieumuon = await _apiService.GetAsync("api/phieumuon").Result.Content.ReadAsAsync<List<PhieuMuon>>();

            var list_sach = await _apiService.GetAsync("api/sach").Result.Content.ReadAsAsync<List<Sach>>();

            var list_chitietphieumuon = await _apiService.GetAsync("api/ctPhieuMuon").Result.Content.ReadAsAsync<List<CtphieuMuon>>();

            var month = DateTime.Now.Month;

            var result = (from s in list_sach
                          join ctpm in list_chitietphieumuon on s.Id equals ctpm.Book
                          join pm in list_phieumuon on ctpm.PhieuMuon equals pm.Id

                          where ctpm.PhieuMuon == pm.Id && ctpm.Book == s.Id
                          && pm.NgayMuon.Month == month

                          group new { s, ctpm } by s.TenSach into sachmuonnhieu

                          select new SachMuonModel()
                          {
                              TenSach = sachmuonnhieu.Select(x => x.s.TenSach).FirstOrDefault(),

                              SoLanMuon = sachmuonnhieu.Where(x => x.ctpm.Book == x.s.Id).Count(),

                              Month = month

                          }).OrderByDescending(s => s.SoLanMuon).Take(3).ToList();


            return View(result);
        }

        public async Task<IActionResult> SachMuonIt()
        {
            var list_phieumuon = await _apiService.GetAsync("api/phieumuon").Result.Content.ReadAsAsync<List<PhieuMuon>>();

            var list_sach = await _apiService.GetAsync("api/sach").Result.Content.ReadAsAsync<List<Sach>>();

            var list_chitietphieumuon = await _apiService.GetAsync("api/ctPhieuMuon").Result.Content.ReadAsAsync<List<CtphieuMuon>>();

            var month = DateTime.Now.Month;

            var result = (from s in list_sach
                          join ctpm in list_chitietphieumuon on s.Id equals ctpm.Book
                          join pm in list_phieumuon on ctpm.PhieuMuon equals pm.Id

                          where ctpm.PhieuMuon == pm.Id && ctpm.Book == s.Id
                          && pm.NgayMuon.Month == month

                          group new { s, ctpm } by s.TenSach into sachmuonnhieu

                          select new SachMuonModel()
                          {
                              TenSach = sachmuonnhieu.Select(x => x.s.TenSach).FirstOrDefault(),

                              SoLanMuon = sachmuonnhieu.Where(x => x.ctpm.Book == x.s.Id).Count(),

                              Month = month

                          }).OrderBy(s => s.SoLanMuon).Take(3).ToList();


            return View(result);
        }

        public async Task<IActionResult> SachMuonTrongNgay(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TenSachSortParm = sortOrder == "tensach" ? "tensach_desc" : "tensach";
            ViewBag.LoaiSachSortParm = sortOrder == "loaisach" ? "loaisach_desc" : "loaisach";
            ViewBag.SoLuongSortParm = sortOrder == "soluong" ? "soluong_desc" : "soluong";

            var list_phieumuon = await _apiService.GetAsync("api/phieumuon").Result.Content.ReadAsAsync<List<PhieuMuon>>();

            var list_sach = await _apiService.GetAsync("api/sach").Result.Content.ReadAsAsync<List<Sach>>();

            var list_chitietphieumuon = await _apiService.GetAsync("api/ctPhieuMuon").Result.Content.ReadAsAsync<List<CtphieuMuon>>();

            var result = (from s in list_sach
                          join ctpm in list_chitietphieumuon on s.Id equals ctpm.Book
                          join pm in list_phieumuon on ctpm.PhieuMuon equals pm.Id

                          where ctpm.PhieuMuon == pm.Id && ctpm.Book == s.Id
                          && pm.NgayMuon.Day == DateTime.Now.Day

                          select s).ToList();

            if (searchString != null)
                page = 1;
            else searchString = currentFilter;
            ViewBag.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                result = result.Where(s => s.TenSach.ToUpper().Contains(searchString.ToUpper())).ToList();
                if (result.Count() > 0)
                {
                    TempData["notice"] = "Have result";
                    TempData["dem"] = result.Count();
                }
                else
                {
                    TempData["notice"] = "No result";
                }
            }
            switch (sortOrder)
            {
                case "tensach":
                    result = result.OrderBy(s => s.TenSach).ToList();
                    break;
                case "tensach_desc":
                    result = result.OrderByDescending(s => s.TenSach).ToList();
                    break;
                case "loaisach":
                    list_sach = list_sach.OrderBy(s => s.LoaiSachNavigation.TenLoai).ToList();
                    break;
                case "loaisach_desc":
                    list_sach = list_sach.OrderByDescending(s => s.LoaiSachNavigation.TenLoai).ToList();
                    break;
                case "soluong":
                    result = result.OrderBy(s => s.SoLuong).ToList();
                    break;
                case "soluong_desc":
                    result = result.OrderByDescending(s => s.SoLuong).ToList();
                    break;
                default:
                    result = result.OrderBy(s => s.Id).ToList();
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(result.ToPagedList(pageNumber, pageSize));
        }

        public async Task<IActionResult> SachKhongMuonTrongNam(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TenSachSortParm = sortOrder == "tensach" ? "tensach_desc" : "tensach";
            ViewBag.LoaiSachSortParm = sortOrder == "loaisach" ? "loaisach_desc" : "loaisach";
            ViewBag.SoLuongSortParm = sortOrder == "soluong" ? "soluong_desc" : "soluong";

            var list_sach = await _apiService.GetAsync("api/sach").Result.Content.ReadAsAsync<List<Sach>>();

            var list_chitietphieumuon = await _apiService.GetAsync("api/ctPhieuMuon").Result.Content.ReadAsAsync<List<CtphieuMuon>>();

            var sachNotBorrow = new List<Sach>();

            foreach (var sach in list_sach)
            {
                if (list_chitietphieumuon.FirstOrDefault(x => x.Book == sach.Id) == null)
                {
                    sachNotBorrow.Add(sach);
                }
            }

            if (searchString != null)
                page = 1;
            else searchString = currentFilter;
            ViewBag.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                sachNotBorrow = sachNotBorrow.Where(s => s.TenSach.ToUpper().Contains(searchString.ToUpper())).ToList();
                if (sachNotBorrow.Count() > 0)
                {
                    TempData["notice"] = "Have result";
                    TempData["dem"] = sachNotBorrow.Count();
                }
                else
                {
                    TempData["notice"] = "No result";
                }
            }
            switch (sortOrder)
            {
                case "tensach":
                    sachNotBorrow = sachNotBorrow.OrderBy(s => s.TenSach).ToList();
                    break;
                case "tensach_desc":
                    sachNotBorrow = sachNotBorrow.OrderByDescending(s => s.TenSach).ToList();
                    break;
                case "loaisach":
                    list_sach = list_sach.OrderBy(s => s.LoaiSachNavigation.TenLoai).ToList();
                    break;
                case "loaisach_desc":
                    list_sach = list_sach.OrderByDescending(s => s.LoaiSachNavigation.TenLoai).ToList();
                    break;
                case "soluong":
                    sachNotBorrow = sachNotBorrow.OrderBy(s => s.SoLuong).ToList();
                    break;
                case "soluong_desc":
                    sachNotBorrow = sachNotBorrow.OrderByDescending(s => s.SoLuong).ToList();
                    break;
                default:
                    sachNotBorrow = sachNotBorrow.OrderBy(s => s.Id).ToList();
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(sachNotBorrow.ToPagedList(pageNumber, pageSize));
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var sach = await _apiService.GetAsync($"api/sach/{id}").Result.Content.ReadAsAsync<Sach>();

            if (sach == null)
            {
                return NotFound();
            }

            return View(sach);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sach = await _apiService.GetAsync($"api/sach/{id}").Result.Content.ReadAsAsync<Sach>();
            if (sach == null)
            {
                return NotFound();
            }

            return View(sach);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var sach = await _apiService.GetAsync($"api/sach/{id}").Result.Content.ReadAsAsync<Sach>();

            sach.TrangThai = false;

            HttpResponseMessage respond = await _apiService.PutAsJsonAsync($"api/sach/{sach.Id}", sach);
            respond.EnsureSuccessStatusCode();

            TempData["notice"] = "Successfully delete";
            TempData["sach"] = sach.TenSach;

            return RedirectToAction(nameof(SachKhongMuonTrongNam));
        }
    }
}