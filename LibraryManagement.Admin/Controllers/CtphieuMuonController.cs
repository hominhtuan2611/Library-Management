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

namespace LibraryManagement.Admin.Controllers
{
    public class CtphieuMuonController : Controller
    {
        private readonly LibraryDBContext _context;

        public IConfiguration _configuration;

        private HttpClient _apiService;
        private readonly string apiAddress;

        public CtphieuMuonController(LibraryDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

            apiAddress = _configuration.GetSection("ApiAddress").GetSection("Url").Value;
            _apiService = ApiService.GetAPI(apiAddress);
        }

        // GET: CtphieuMuon
        public async Task<IActionResult> Index()
        {
            var libraryDBContext = _context.CtphieuMuon.Include(c => c.BookNavigation).Include(c => c.PhieuMuonNavigation);
            return View(await libraryDBContext.ToListAsync());
        }

        // GET: CtphieuMuon/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ctphieuMuon = await _context.CtphieuMuon
                .Include(c => c.BookNavigation)
                .Include(c => c.PhieuMuonNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ctphieuMuon == null)
            {
                return NotFound();
            }

            return View(ctphieuMuon);
        }

        // GET: CtphieuMuon/Create
        public IActionResult Create()
        {
            ViewData["Book"] = new SelectList(_context.Sach, "Id", "Id");
            ViewData["PhieuMuon"] = new SelectList(_context.PhieuMuon, "Id", "Id");
            return View();
        }

        // POST: CtphieuMuon/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PhieuMuon,Book,SoLuong,NgayMuon,NgayTra,TinhTrangSach")] CtphieuMuon ctphieuMuon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ctphieuMuon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Book"] = new SelectList(_context.Sach, "Id", "Id", ctphieuMuon.Book);
            ViewData["PhieuMuon"] = new SelectList(_context.PhieuMuon, "Id", "Id", ctphieuMuon.PhieuMuon);
            return View(ctphieuMuon);
        }

        // GET: CtphieuMuon/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ctphieuMuon = await _context.CtphieuMuon.FindAsync(id);
            if (ctphieuMuon == null)
            {
                return NotFound();
            }
            ViewData["Book"] = new SelectList(_context.Sach, "Id", "Id", ctphieuMuon.Book);
            ViewData["PhieuMuon"] = new SelectList(_context.PhieuMuon, "Id", "Id", ctphieuMuon.PhieuMuon);
            return View(ctphieuMuon);
        }

        // POST: CtphieuMuon/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PhieuMuon,Book,SoLuong,NgayMuon,NgayTra,TinhTrangSach")] CtphieuMuon ctphieuMuon)
        {
            if (id != ctphieuMuon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ctphieuMuon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CtphieuMuonExists(ctphieuMuon.Id))
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
            ViewData["Book"] = new SelectList(_context.Sach, "Id", "Id", ctphieuMuon.Book);
            ViewData["PhieuMuon"] = new SelectList(_context.PhieuMuon, "Id", "Id", ctphieuMuon.PhieuMuon);
            return View(ctphieuMuon);
        }

        // GET: CtphieuMuon/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ctphieuMuon = await _context.CtphieuMuon
                .Include(c => c.BookNavigation)
                .Include(c => c.PhieuMuonNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ctphieuMuon == null)
            {
                return NotFound();
            }

            return View(ctphieuMuon);
        }

        // POST: CtphieuMuon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ctphieuMuon = await _context.CtphieuMuon.FindAsync(id);
            _context.CtphieuMuon.Remove(ctphieuMuon);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CtphieuMuonExists(int id)
        {
            return _context.CtphieuMuon.Any(e => e.Id == id);
        }
    }
}
