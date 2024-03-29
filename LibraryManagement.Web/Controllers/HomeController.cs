﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Web.Models;
using LibraryManagement.API.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using LibraryManagement.Application.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using X.PagedList;

namespace LibraryManagement.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly LibraryDBContext _context;

        public IConfiguration _configuration;

        private HttpClient _apiService;
        private readonly string apiAddress;

        public HomeController(LibraryDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

            apiAddress = _configuration.GetSection("ApiAddress").GetSection("Url").Value;
            _apiService = ApiService.GetAPI(apiAddress);
        }

        public async Task<IActionResult> Index()
        {
            var notice = "";
            if (TempData["notice"] != null) {
                notice = TempData["notice"].ToString();
            }
            var loaisach = await _apiService.GetAsync("api/LoaiSach").Result.Content.ReadAsAsync<List<LoaiSach>>();
            var list_sach = await _apiService.GetAsync("api/sach").Result.Content.ReadAsAsync<List<Sach>>();
            var sach = new Sach();
            var tuple = new Tuple<List<LibraryManagement.API.Models.LoaiSach>, List<LibraryManagement.API.Models.Sach>, LibraryManagement.API.Models.Sach>(loaisach, list_sach, sach);
            return View(tuple);
        }

        public async Task<IActionResult> Login(string name, string password)
        {
            if (ModelState.IsValid)
            {
                var user = _context.DocGia.Where(x => x.Username == name).FirstOrDefault();
                if (user != null)
                {
                    if (Password_Encryptor.HashSHA1(password) == user.Password)
                    {
                        HttpContext.Session.SetObject<DocGia>(CommonConstants.User_Session, user);

                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thông tin đăng nhập không đúng!");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại!");
                }
            }
            var loaisach = await _apiService.GetAsync("api/LoaiSach").Result.Content.ReadAsAsync<List<LoaiSach>>();
            var list_sach = await _apiService.GetAsync("api/sach").Result.Content.ReadAsAsync<List<Sach>>();
            var sach = new Sach();
            var tuple = new Tuple<List<LibraryManagement.API.Models.LoaiSach>, List<LibraryManagement.API.Models.Sach>, LibraryManagement.API.Models.Sach>(loaisach, list_sach, sach);
            return View(tuple);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(CommonConstants.User_Session);

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
