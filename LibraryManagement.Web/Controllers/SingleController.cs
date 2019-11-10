using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Web.Controllers
{
    public class SingleController : Controller
    {
        public IActionResult Single()
        {
            return View();
        }
    }
}