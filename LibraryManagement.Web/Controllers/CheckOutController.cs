using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using LibraryManagement.API.Models;
using LibraryManagement.Application.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LibraryManagement.Web.Controllers
{
    public class CheckOutController : Controller
    {
        public IConfiguration _configuration;

        private HttpClient _apiService;
        private readonly string apiAddress;

        public CheckOutController( IConfiguration configuration)
        {
            _configuration = configuration;

            apiAddress = _configuration.GetSection("ApiAddress").GetSection("Url").Value;
            _apiService = ApiService.GetAPI(apiAddress);

        }
        public async Task<IActionResult> CheckOut()
        {
            return View();
        }
        public async Task<IActionResult> muonsach()
        {
            var docgia = HttpContext.Session.GetObject<int>(CommonConstants.Docgia_Session);
            var ss_lsSach = HttpContext.Session.GetObject<List<Models.SessionSach>>("dssach");
            var tongsach = 0;
            ss_lsSach.ForEach(lssach => tongsach += lssach.soluong);
            if (docgia != 0 && ss_lsSach != null)
            {
                CtphieuMuon ctpm = new CtphieuMuon();
                PhieuMuon pm = new PhieuMuon();
                pm.NgayMuon = DateTime.Now;
                pm.MaDg = docgia;
                pm.TrangThai = 1;
                pm.TongSachMuon = tongsach;
                pm.HanTra = DateTime.Now.AddDays(30);
                if (pm.NgayMuon < pm.HanTra)
                {
                    if (pm.TongSachMuon == null)
                    {
                        pm.TongSachMuon = 0;
                    }
                    var respond = await _apiService.PostAsJsonAsync("api/phieuMuon", pm).Result.Content.ReadAsAsync<PhieuMuon>();
                }
                for (int i = 0; i < ss_lsSach.Count; i++)
                {
                    ctpm.PhieuMuon = pm.Id;
                    ctpm.SoLuong = ss_lsSach[i].soluong;
                    ctpm.Book = ss_lsSach[i].sach.Id;
                    ctpm.TinhTrangSach = "1";
                    var phieuMuon = await _apiService.GetAsync($"api/phieuMuon/{ctpm.PhieuMuon}").Result.Content.ReadAsAsync<PhieuMuon>();
                    var sach = await _apiService.GetAsync($"api/sach/{ctpm.Book}").Result.Content.ReadAsAsync<Sach>();
                    if (sach.SoLuong >= ctpm.SoLuong)
                    {
                        phieuMuon.TongSachMuon += ctpm.SoLuong;
                        phieuMuon.MaDgNavigation = null;
                        phieuMuon.MaNvNavigation = null;
                        sach.SoLuong -= ctpm.SoLuong;

                        CtphieuMuon new_ctPhieuMuon = await _apiService.PostAsJsonAsync("api/ctPhieuMuon", ctpm).Result.Content.ReadAsAsync<CtphieuMuon>();
                        HttpResponseMessage respond_sach = await _apiService.PutAsJsonAsync($"api/sach/{sach.Id}", sach);
                        respond_sach.EnsureSuccessStatusCode();
                        HttpResponseMessage respond_phieuMuon = await _apiService.PutAsJsonAsync($"api/phieuMuon/{phieuMuon.Id}", phieuMuon);
                        respond_phieuMuon.EnsureSuccessStatusCode();

                        @TempData["notice"] = "Successfully create";
                    }
                }
            }

            Response.Redirect("CheckOut/Checkout");
            return View();
        }

    }
}