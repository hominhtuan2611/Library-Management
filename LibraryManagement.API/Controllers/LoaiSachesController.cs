using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.API.Models;

namespace LibraryManagement.API.Controllers
{
    [Route("api/loaisach")]
    [ApiController]
    public class LoaiSachesController : ControllerBase
    {
        private readonly LibraryDBContext _context;

        public LoaiSachesController(LibraryDBContext context)
        {
            _context = context;
        }

        // GET: api/LoaiSaches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoaiSach>>> GetLoaiSach()
        {
            return await _context.LoaiSach.ToListAsync();
        }

        // GET: api/LoaiSaches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoaiSach>> GetLoaiSach(int id)
        {
            var loaiSach = await _context.LoaiSach.FindAsync(id);

            if (loaiSach == null)
            {
                return NotFound();
            }

            return loaiSach;
        }

        // GET: api/LoaiSaches/5
        [HttpGet("sach/{id}")]
        public async Task<ActionResult<IEnumerable<Sach>>> GetSach(int id)
        {
            var sach = await _context.Sach.Include(a => a.LoaiSachNavigation).Where(m => m.LoaiSach == id).ToListAsync();

            if (sach == null)
            { 
                return NotFound();
            }

            return sach;
        }

        // PUT: api/LoaiSaches/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoaiSach(int id, LoaiSach loaiSach)
        {
            if (id != loaiSach.Id)
            {
                return BadRequest();
            }

            _context.Entry(loaiSach).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoaiSachExists(id))
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

        // POST: api/LoaiSaches
        [HttpPost]
        public async Task<ActionResult<LoaiSach>> PostLoaiSach(LoaiSach loaiSach)
        {
            _context.LoaiSach.Add(loaiSach);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLoaiSach", new { id = loaiSach.Id }, loaiSach);
        }

        // DELETE: api/LoaiSaches/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LoaiSach>> DeleteLoaiSach(int id)
        {
            var loaiSach = await _context.LoaiSach.FindAsync(id);
            if (loaiSach == null)
            {
                return NotFound();
            }

            _context.LoaiSach.Remove(loaiSach);
            await _context.SaveChangesAsync();

            return loaiSach;
        }

        private bool LoaiSachExists(int id)
        {
            return _context.LoaiSach.Any(e => e.Id == id);
        }
    }
}
