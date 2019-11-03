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
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace LibraryManagement.Admin.Controllers
{
    public class SachController : Controller
    {
        private readonly LibraryDBContext _context;

        public IConfiguration _configuration;
        private readonly IHostingEnvironment _appEnvironment;

        private HttpClient _apiService;
        private readonly string apiAddress;

        public SachController(LibraryDBContext context, IConfiguration configuration, IHostingEnvironment appEnvironment)
        {
            _context = context;
            _configuration = configuration;
            _appEnvironment = appEnvironment;

            apiAddress = _configuration.GetSection("ApiAddress").GetSection("Url").Value;
            _apiService = ApiService.GetAPI(apiAddress);
        }

        // GET: Sach
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TenSachSortParm = sortOrder == "tensach" ? "tensach_desc" : "tensach";
            ViewBag.SoLuongSortParm = sortOrder == "soluong" ? "soluong_desc" : "soluong";

            var list_sach = await _apiService.GetAsync("api/sach").Result.Content.ReadAsAsync<List<Sach>>();

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
                default:
                    list_sach = list_sach.OrderBy(s => s.LoaiSachNavigation.TenLoai).ToList();
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

            var sach = await _apiService.GetAsync($"api/sach/{id}").Result.Content.ReadAsAsync<Sach>();

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
                sach.Id = Random_Generator.GenerateString(13);
                while (await SachExistsAsync(sach.Id))
                {
                    sach.Id = Random_Generator.GenerateString(13);
                }
                sach.SoLuong = 0;
                sach.TrangThai = true;

                //Image being saved
                string webRootPath = _appEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                if (files.Count != 0 && files[0].Length > 0)
                {
                    //Image has been uploaded to path specify in the images folder under wwwroot
                    var uploads = Path.Combine(webRootPath, @"uploads\images");
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var filestream = new FileStream(Path.Combine(uploads, sach.Id + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }

                    //Copy uploaded image to Client wwwroot
                    var clientRootPath = webRootPath.Replace("LibraryManagement.Admin", "LibraryManagement.Web");

                    var clientUploads = Path.Combine(clientRootPath, @"uploads\images");

                    using (var filestream = new FileStream(Path.Combine(clientUploads, sach.Id + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }

                    //Rename the image uploaded using the folder path, it and the picture extension
                    sach.HinhAnh = @"\" + @"uploads\images" + @"\" + sach.Id + extension;
                }
                else if (files.Count == 0)
                {
                    //When the user doesn't upload image, use the default image in the /images folder
                    sach.HinhAnh = @"\" + @"uploads\images" + @"\" + "default_image.png";
                }

                HttpResponseMessage respond = await _apiService.PostAsJsonAsync("api/sach", sach);
                respond.EnsureSuccessStatusCode();

                TempData["notice"] = "Successfully create";
                TempData["sach"] = sach.TenSach;

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

            var sach = await _apiService.GetAsync($"api/sach/{id}").Result.Content.ReadAsAsync<Sach>();
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
                    //Image being saved
                    string webRootPath = _appEnvironment.WebRootPath;
                    var files = HttpContext.Request.Form.Files;

                    if (files.Count != 0 && files[0].Length > 0)
                    {
                        //Image has been uploaded to path specify in the images folder under wwwroot
                        var uploads = Path.Combine(webRootPath, @"uploads\images");
                        var extension = Path.GetExtension(files[0].FileName);

                        using (var filestream = new FileStream(Path.Combine(uploads, sach.Id + extension), FileMode.Create))
                        {
                            files[0].CopyTo(filestream);
                        }

                        //Copy uploaded image to Client wwwroot
                        var clientRootPath = webRootPath.Replace("LibraryManagement.Admin", "LibraryManagement.Web");

                        var clientUploads = Path.Combine(clientRootPath, @"uploads\images");

                        using (var filestream = new FileStream(Path.Combine(clientUploads, sach.Id + extension), FileMode.Create))
                        {
                            files[0].CopyTo(filestream);
                        }

                        //Rename the image uploaded using the folder path, it and the picture extension
                        sach.HinhAnh = @"\" + @"uploads\images" + @"\" + sach.Id + extension;
                    }

                    HttpResponseMessage respond = await _apiService.PutAsJsonAsync($"api/sach/{sach.Id}", sach);
                    respond.EnsureSuccessStatusCode();

                    TempData["notice"] = "Successfully edit";
                    TempData["sach"] = sach.TenSach;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await SachExistsAsync(sach.Id))
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

            var sach = await _apiService.GetAsync($"api/sach/{id}").Result.Content.ReadAsAsync<Sach>();
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
            var sach = await _apiService.GetAsync($"api/sach/{id}").Result.Content.ReadAsAsync<Sach>();

            sach.TrangThai = false;

            HttpResponseMessage respond = await _apiService.PutAsJsonAsync($"api/sach/{sach.Id}", sach);
            respond.EnsureSuccessStatusCode();

            TempData["notice"] = "Successfully delete";
            TempData["sach"] = sach.TenSach;

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> SachExistsAsync(string id)
        {
            var list_sach = await _apiService.GetAsync("api/sach").Result.Content.ReadAsAsync<List<Sach>>();

            return list_sach.Any(e => e.Id == id);
        }
    }
}
