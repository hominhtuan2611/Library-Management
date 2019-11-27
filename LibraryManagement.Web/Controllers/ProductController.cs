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
using X.PagedList;
using LibraryManagement.Web.Models;

namespace LibraryManagement.Web.Controllers
{
    public class ProductController : Controller
    {
        public IConfiguration _configuration;

        private HttpClient _apiService;
        private readonly string apiAddress;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;

            apiAddress = _configuration.GetSection("ApiAddress").GetSection("Url").Value;
            _apiService = ApiService.GetAPI(apiAddress);
        }

        public async Task<IActionResult> Product1()
        {
            var list_sach = await _apiService.GetAsync("api/sach").Result.Content.ReadAsAsync<List<Sach>>();
            return View(list_sach.ToPagedList());
        }

        public async Task<IActionResult> Product2(string id)
        {
            var loaisach = await _apiService.GetAsync("api/Loaisach").Result.Content.ReadAsAsync<List<LoaiSach>>();
            var list_sach = await _apiService.GetAsync($"api/loaisach/sach/{id}").Result.Content.ReadAsAsync<List<Sach>>(); 
            var sach = new Sach();
            var tuple = new Tuple<List<LibraryManagement.API.Models.LoaiSach>, List<LibraryManagement.API.Models.Sach>, LibraryManagement.API.Models.Sach>(loaisach, list_sach, sach);

            return View(tuple);
        }

        public async void addcart(string idloai, string idsach, int sl)
        {
            var sach = await _apiService.GetAsync($"api/sach/{idsach}").Result.Content.ReadAsAsync<Sach>();
            var LS_Sach = HttpContext.Session.GetObject<List<SessionSach>>("dssach") ;
            var s_sach = new SessionSach();
            s_sach.sach = sach;
            s_sach.soluong = sl;
            if (LS_Sach != null)
            {
                for(int i = 0; i<LS_Sach.Count; i++)
                {
                    if (LS_Sach[i].sach.Id.Equals(sach.Id))
                    {
                        LS_Sach[i].soluong += sl;
                        sl = 0;
                    }
                }
                if(sl != 0)
                {
                    LS_Sach.Add(s_sach);
                }
            }
            else
            {
                LS_Sach = new List<SessionSach>();
                LS_Sach.Add(s_sach);
            }
            HttpContext.Session.SetObject("dssach", LS_Sach);
            Response.Redirect($"Product2/{idloai}");
        }

    }
}