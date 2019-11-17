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

            var month = DateTime.Now.AddMonths(-1).Month;

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

            var month = DateTime.Now.AddMonths(-1).Month;

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
    }
}