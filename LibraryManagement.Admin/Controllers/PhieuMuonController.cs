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
using X.PagedList;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Admin.Controllers
{
    public class PhieuMuonController : Controller
    {
        private readonly LibraryDBContext _context;

        public IConfiguration _configuration;
        private readonly IHostingEnvironment _appEnvironment;

        private HttpClient _apiService;
        private readonly string apiAddress;

        public PhieuMuonController(LibraryDBContext context, IConfiguration configuration, IHostingEnvironment appEnvironment)
        {
            _context = context;
            _configuration = configuration;
            _appEnvironment = appEnvironment;

            apiAddress = _configuration.GetSection("ApiAddress").GetSection("Url").Value;
            _apiService = ApiService.GetAPI(apiAddress);
        }

        // GET: PhieuMuon
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.MaPNSortParm = string.IsNullOrEmpty(sortOrder) ? "mapm" : "";
            ViewBag.NgayMuonSortParm = sortOrder == "ngaymuon" ? "ngaymuon_desc" : "ngaymuon";
            var list_phieumuon = await _apiService.GetAsync("api/phieuMuon").Result.Content.ReadAsAsync<List<PhieuMuon>>();
            list_phieumuon = list_phieumuon.Where(p => p.TrangThai != 0).ToList();
            if (searchString != null)
                page = 1;
            else searchString = currentFilter;
            ViewBag.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                list_phieumuon = list_phieumuon.Where(s => s.MaDgNavigation.TenDg.ToUpper().Contains(searchString.ToUpper())).ToList();
                if (list_phieumuon.Count() > 0)
                {
                    TempData["notice"] = "Have result";
                    TempData["dem"] = list_phieumuon.Count();
                }
                else
                {
                    TempData["notice"] = "No result";
                }
            }
            switch (sortOrder)
            {
                case "mapm":
                    list_phieumuon = list_phieumuon.OrderBy(s => s.Id).ToList();
                    break;
                case "ngaynhap":
                    list_phieumuon = list_phieumuon.OrderBy(s => s.NgayMuon).ToList();
                    break;
                case "ngaynhap_desc":
                    list_phieumuon = list_phieumuon.OrderByDescending(s => s.NgayMuon).ToList();
                    break;
                default:
                    list_phieumuon = list_phieumuon.OrderByDescending(s => s.NgayMuon).ToList();
                    break;

            }

            var expired = list_phieumuon.Where(x => x.HanTra < DateTime.Now && x.DaTra == false).Count();

            if (expired > 0)
            {
                TempData["expireNotice"] = "Has Expired";
                TempData["expired"] = expired;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(list_phieumuon.ToPagedList(pageNumber, pageSize));
        }

        // GET: PhieuMuon/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phieuMuon = await _apiService.GetAsync($"api/phieuMuon/{id}").Result.Content.ReadAsAsync<PhieuMuon>();
            if (phieuMuon == null)
            {
                return NotFound();
            }

            return View(phieuMuon);
        }

        // GET: PhieuMuon/Create
        public IActionResult Create()
        {
            ViewData["MaDg"] = new SelectList(_context.DocGia, "Id", "TenDg");
            ViewData["MaNv"] = new SelectList(_context.NhanVien.Where(p => p.ViTri != "Admin"), "Id", "TenNv");
            return View();
        }

        // POST: PhieuMuon/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaDg,MaNv,HanTra")] PhieuMuon phieuMuon)
        {
            if (ModelState.IsValid)
            {
                phieuMuon.NgayMuon = DateTime.Now;
                if (phieuMuon.NgayMuon < phieuMuon.HanTra)
                { 
                    if (phieuMuon.TongSachMuon == null)
                    {
                        phieuMuon.TongSachMuon = 0;
                        phieuMuon.TrangThai = 1;
                    }
                    var respond = await _apiService.PostAsJsonAsync("api/phieuMuon", phieuMuon).Result.Content.ReadAsAsync<PhieuMuon>();
                    TempData["notice"] = "Successfully create PhieuMuon";
                    TempData["phieumuon"] = respond.MaDgNavigation.TenDg + "-" + respond.NgayMuon.ToShortDateString();
                    return RedirectToAction("Index", "CtphieuMuon", new { phieuMuonId = respond.Id });
                }
                TempData["notice"] = "Create Error";
            }
            ViewData["MaDg"] = new SelectList(_context.DocGia, "Id", "TenDg", phieuMuon.MaDg);
            ViewData["MaNv"] = new SelectList(_context.NhanVien.Where(p => p.ViTri != "Admin"), "Id", "TenNv", phieuMuon.MaNv);
            return View(phieuMuon);
        }

        // GET: PhieuMuon/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phieuMuon = await _apiService.GetAsync($"api/phieuMuon/{id}").Result.Content.ReadAsAsync<PhieuMuon>();
            if (phieuMuon == null)
            {
                return NotFound();
            }
            ViewData["MaDg"] = new SelectList(_context.DocGia, "Id", "TenDg", phieuMuon.MaDg);
            ViewData["MaNv"] = new SelectList(_context.NhanVien.Where(p=>p.ViTri!="Admin"), "Id", "TenNv", phieuMuon.MaNv);
            return View(phieuMuon);
        }

        // POST: PhieuMuon/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MaDg,MaNv,NgayMuon,TongSachMuon,HanTra,DaTra,TrangThai")] PhieuMuon phieuMuon)
        {
            if (id != phieuMuon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (phieuMuon.NgayMuon < phieuMuon.HanTra)
                {
                    var phieuMuon_cu = await _apiService.GetAsync($"api/phieuMuon/{id}").Result.Content.ReadAsAsync<PhieuMuon>();
                    if(phieuMuon_cu.TrangThai==1)
                    {
                        string userSession = HttpContext.Session.GetString(CommonConstants.User_Session);
                        var nhanvien = _context.NhanVien.Where(p => p.Username == userSession).FirstOrDefault();
                        phieuMuon.MaNv = nhanvien.Id;
                    }
                    if (phieuMuon_cu.DaTra==false && phieuMuon.DaTra==true)
                    {
                        if (DateTime.Now < phieuMuon.HanTra)
                            phieuMuon.TrangThai = 3;
                        else
                            phieuMuon.TrangThai = 4;

                        var list_ctphieumuon = await _apiService.GetAsync($"api/ctPhieuMuon/{phieuMuon.Id}").Result.Content.ReadAsAsync<List<CtphieuMuon>>();
                        foreach(CtphieuMuon item in list_ctphieumuon)
                        {
                            var sach = await _apiService.GetAsync($"api/sach/{item.Book}").Result.Content.ReadAsAsync<Sach>();
                            sach.SoLuong += item.SoLuong;
                            sach.LoaiSachNavigation = null;
                            HttpResponseMessage respond_sach = await _apiService.PutAsJsonAsync($"api/sach/{sach.Id}", sach);
                            respond_sach.EnsureSuccessStatusCode();

                        }
                    }
                    var respond = await _apiService.PutAsJsonAsync($"api/phieuMuon/{id}", phieuMuon).Result.Content.ReadAsAsync<PhieuMuon>();
                    if (respond != null)
                    {
                        TempData["notice"] = "Successfully edit";
                        TempData["phieumuon"] = respond.MaDgNavigation.TenDg + " - " + respond.NgayMuon.ToShortDateString();
                        if(respond.TrangThai==4)
                        {
                            TempData["notice"] = "Return late";
                            TempData["phieumuon"] = respond.MaDgNavigation.TenDg + " - " + respond.NgayMuon.ToShortDateString();
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
                TempData["notice"] = "Edit Error";
            }
            ViewData["MaDg"] = new SelectList(_context.DocGia, "Id", "TenDg", phieuMuon.MaDg);
            ViewData["MaNv"] = new SelectList(_context.NhanVien.Where(p => p.ViTri != "Admin"), "Id", "TenNv", phieuMuon.MaNv);
            return View(phieuMuon);
        }

        // GET: PhieuMuon/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phieuMuon = await _apiService.GetAsync($"api/phieuMuon/{id}").Result.Content.ReadAsAsync<PhieuMuon>();
            if (phieuMuon == null)
            {
                return NotFound();
            }

            return View(phieuMuon);
        }

        // POST: PhieuMuon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phieuMuon = await _apiService.GetAsync($"api/phieuMuon/{id}").Result.Content.ReadAsAsync<PhieuMuon>();

            phieuMuon.TrangThai = 0;
            phieuMuon.MaDgNavigation = null;
            phieuMuon.MaNvNavigation = null;

            var respond = await _apiService.PutAsJsonAsync($"api/phieuMuon/{id}", phieuMuon).Result.Content.ReadAsAsync<PhieuMuon>();
            TempData["notice"] = "Successfully delete";
            TempData["phieumuon"] = respond.MaDgNavigation.TenDg + "-" + phieuMuon.NgayMuon.ToShortDateString();
            
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PhieuMuonExistsAsync(int id)
        {
            var list_phieuMuon = await _apiService.GetAsync("api/phieuMuon").Result.Content.ReadAsAsync<List<PhieuMuon>>();

            return list_phieuMuon.Any(e => e.Id == id);
        }

        public IActionResult SendExpiryWarningEmail(string tenDocGia, string ngayMuon, string email)
        {
            string webRoot = _appEnvironment.WebRootPath.ToString();

            string file = webRoot + "\\templates\\borrow_expiry_warning.html";

            string content = System.IO.File.ReadAllText(file);

            content = content.Replace("{{NgayMuon}}", ngayMuon);
            content = content.Replace("{{TenDocGgia}}", tenDocGia);

            var toEmail = email;

            new EmailHelper().SendEmail(toEmail, "Đến hạn trả sách", content);

            TempData["notice"] = "Successfully send email";
            TempData["docgia"] = tenDocGia;

            return RedirectToAction(nameof(Index));
        }
    }
}
