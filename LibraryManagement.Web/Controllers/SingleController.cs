using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using LibraryManagement.API.Models;
using LibraryManagement.Application.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace LibraryManagement.Web.Controllers
{
    public class SingleController : Controller
    {
        public IConfiguration _configuration;

        private HttpClient _apiService;
        private readonly string apiAddress;

        public SingleController(IConfiguration configuration)
        {
            _configuration = configuration;

            apiAddress = _configuration.GetSection("ApiAddress").GetSection("Url").Value;
            _apiService = ApiService.GetAPI(apiAddress);
        }

        public async Task<IActionResult> Single(string id)
        {

            var loaisach = await _apiService.GetAsync("api/LoaiSach").Result.Content.ReadAsAsync<List<LoaiSach>>();
            var list_sach = new List<Sach>();
            HttpContext.Session.SetObject("dssach", list_sach); 
            var sach = await _apiService.GetAsync($"api/sach/{id}").Result.Content.ReadAsAsync<Sach>();
            var tuple = new Tuple<List<LibraryManagement.API.Models.LoaiSach>, List<LibraryManagement.API.Models.Sach>, LibraryManagement.API.Models.Sach>(loaisach, list_sach, sach);
            return View(tuple);
        }
    }
}