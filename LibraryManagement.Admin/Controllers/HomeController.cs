using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Admin.Models;
using Microsoft.AspNetCore.Http;
using LibraryManagement.Application.Common;

namespace LibraryManagement.Admin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //string user = HttpContext.Session.GetString(CommonConstants.User_Session);

            //if (user == "" || user == null)
            //{
            //    return RedirectToAction("Authorize", "Home");
            //}
            //else
            //{
            //    return View();
            //}
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
