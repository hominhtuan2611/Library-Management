using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Web.Models;
using LibraryManagement.API.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using LibraryManagement.Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading;

namespace LibraryManagement.Web.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly LibraryDBContext _context;
        public IConfiguration _configuration;

        private HttpClient _apiService;
        private readonly string apiAddress;

        public CheckOutController(LibraryDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

            apiAddress = _configuration.GetSection("ApiAddress").GetSection("Url").Value;
            _apiService = ApiService.GetAPI(apiAddress);

        }
        public async Task<IActionResult> CheckOut()
        {
            var loaisach = await _apiService.GetAsync("api/LoaiSach").Result.Content.ReadAsAsync<List<LoaiSach>>();
            var list_sach = new List<Sach>();
            var sach = new Sach();
            var tuple = new Tuple<List<LibraryManagement.API.Models.LoaiSach>, List<LibraryManagement.API.Models.Sach>, LibraryManagement.API.Models.Sach>(loaisach, list_sach, sach);
            var LS_Sach = HttpContext.Session.GetObject<List<SessionSach>>("dssach");
            if (LS_Sach == null)
            {
                var ss_lsSach = new List<SessionSach>();
                HttpContext.Session.SetObject("dssach", ss_lsSach);
            }
            return View(tuple);
        }
        public async Task<IActionResult> muonsach()
        {
            var docgia = HttpContext.Session.GetObject<DocGia>(CommonConstants.User_Session);
            var ss_lsSach = HttpContext.Session.GetObject<List<Models.SessionSach>>("dssach");
            var tongsach = 0;
            ss_lsSach.ForEach(lssach => tongsach += lssach.soluong);
            if (docgia != null && ss_lsSach != null)
            {

                PhieuMuon pm = new PhieuMuon();
                pm.NgayMuon = DateTime.Now;
                pm.MaDg = docgia.Id;
                pm.TrangThai = 1;
                pm.TongSachMuon = tongsach;
                pm.MaNv = 1;
                pm.HanTra = DateTime.Now.AddDays(30);
                if (pm.NgayMuon < pm.HanTra)
                {
                    var respond = await _apiService.PostAsJsonAsync("api/phieuMuon", pm).Result.Content.ReadAsAsync<PhieuMuon>();

                    for (int i = 0; i < ss_lsSach.Count; i++)
                    {
                        CtphieuMuon ctpm = new CtphieuMuon();
                        ctpm.PhieuMuon = respond.Id;
                        ctpm.SoLuong = ss_lsSach[i].soluong;
                        ctpm.Book = ss_lsSach[i].sach.Id;
                        ctpm.TinhTrangSach = "1";

                        var sach = await _apiService.GetAsync($"api/sach/{ctpm.Book}").Result.Content.ReadAsAsync<Sach>();
                        if (sach.SoLuong >= ctpm.SoLuong)
                        {
                            sach.SoLuong -= ctpm.SoLuong;

                            CtphieuMuon new_ctPhieuMuon = await _apiService.PostAsJsonAsync("api/ctPhieuMuon", ctpm).Result.Content.ReadAsAsync<CtphieuMuon>();
                            HttpResponseMessage respond_sach = await _apiService.PutAsJsonAsync($"api/sach/{sach.Id}", sach);
                            respond_sach.EnsureSuccessStatusCode();
                            TempData["notice"] = "success";

                        }
                        else
                        {
                            TempData["notice"] = "fail";
                        }
                    }
                    ss_lsSach = new List<SessionSach>();
                    HttpContext.Session.SetObject("dssach", ss_lsSach);
                    return Redirect("~/Home/Index");

                 }
            }
            else
            {
                TempData["notice"] = "fail";
                return Redirect("~/checkout/checkout");
            }

            return Redirect("~/Home/Index");
        }
    }
}
