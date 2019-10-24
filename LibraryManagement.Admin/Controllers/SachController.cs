using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LibraryManagement.API.Models;
using LibraryManagement.Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace LibraryManagement.Admin.Controllers
{
    public class SachController : Controller
    {
        const string apiAddress = "https://localhost:44387";

        // GET: Sach
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TenSachSortParm = sortOrder == "tensach" ? "tensach_desc" : "tensach";
            ViewBag.SoLuongSortParm = sortOrder == "soluong" ? "soluong_desc" : "soluong";

            var list_sach = new List<Sach>();

            HttpResponseMessage respond = await ApiService.GetAPI(apiAddress).GetAsync("/api/sach");

            if (respond.IsSuccessStatusCode)
            {
                var sachJsonString = await respond.Content.ReadAsStringAsync();

                list_sach = JsonConvert.DeserializeObject<IEnumerable<Sach>>(sachJsonString).ToList();
            }

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
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(list_sach.ToPagedList(pageNumber, pageSize));
        }

        // GET: Sach/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Sach/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sach/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Sach/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Sach/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Sach/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Sach/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}