using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Web.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Product1()
        {
            return View();
        }

        public IActionResult Product2()
        {
            return View();
        }
    }
}