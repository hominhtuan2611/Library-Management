﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.API.Models;

namespace LibraryManagement.API.Controllers
{
    [Route("api/sach")]
    [ApiController]
    public class SachesController : ControllerBase
    {
        private readonly LibraryDBContext _context;

        public SachesController(LibraryDBContext context)
        {
            _context = context;
        }

        // GET: api/sach
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sach>>> GetSach()
        {
            return await _context.Sach.Include(a => a.LoaiSachNavigation).Where(x => x.TrangThai == true).ToListAsync();
        }

        // GET: api/sach/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sach>> GetSach(string id)
        {
            var sach = await _context.Sach
                .Include(a => a.LoaiSachNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (sach == null)
            {
                return NotFound();
            }

            return sach;
        }

        // PUT: api/sach/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSach(string id, Sach sach)
        {
            if (id != sach.Id)
            {
                return BadRequest();
            }

            _context.Entry(sach).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SachExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/sach
        [HttpPost]
        public async Task<ActionResult<Sach>> PostSach(Sach sach)
        {
            sach.TrangThai = true;
            _context.Sach.Add(sach);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SachExists(sach.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSach", new { id = sach.Id }, sach);
        }

        // DELETE: api/sach
        [HttpDelete("{id}")]
        public async Task<ActionResult<Sach>> DeleteSach(string id)
        {
            var sach = await _context.Sach.FindAsync(id);
            if (sach == null)
            {
                return NotFound();
            }

            sach.TrangThai = false;
            await _context.SaveChangesAsync();

            return sach;
        }

        private bool SachExists(string id)
        {
            return _context.Sach.Any(e => e.Id == id);
        }
    }
}
