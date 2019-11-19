using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Admin.Models;
using Microsoft.AspNetCore.Http;
using LibraryManagement.Application.Common;
using LibraryManagement.API.Models;

namespace LibraryManagement.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly LibraryDBContext _context;

        public HomeController(LibraryDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            string userSession = HttpContext.Session.GetString(CommonConstants.User_Session);

            if (userSession == "" || userSession == null)
            {
                return RedirectToAction(nameof(Login));
            }

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.NhanVien.Where(x => x.Username == model.Username).FirstOrDefault();

                if (user != null)
                {
                    if (Password_Encryptor.HashSHA1(model.Password) == user.Password)
                    {
                        HttpContext.Session.SetString(CommonConstants.User_Session, user.Username);

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

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove(CommonConstants.User_Session);

            return RedirectToAction(nameof(Login));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
