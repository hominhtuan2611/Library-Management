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
    [Route("api/CtphieuNhap")]
    [ApiController]
    public class CtphieuNhapsController : ControllerBase
    {
        private readonly LibraryDBContext _context;

        public CtphieuNhapsController(LibraryDBContext context)
        {
            _context = context;
        }

        // GET: api/CtphieuNhaps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CtphieuNhap>>> GetCtphieuNhap()
        {
            return await _context.CtphieuNhap.ToListAsync();
        }

        // GET: api/CtphieuNhaps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CtphieuNhap>> GetCtphieuNhap(int id)
        {
            var ctphieuNhap = await _context.CtphieuNhap.FindAsync(id);

            if (ctphieuNhap == null)
            {
                return NotFound();
            }

            return ctphieuNhap;
        }

        // PUT: api/CtphieuNhaps/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCtphieuNhap(int id, CtphieuNhap ctphieuNhap)
        {
            if (id != ctphieuNhap.Id)
            {
                return BadRequest();
            }

            _context.Entry(ctphieuNhap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CtphieuNhapExists(id))
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

        // POST: api/CtphieuNhaps
        [HttpPost]
        public async Task<ActionResult<CtphieuNhap>> PostCtphieuNhap(CtphieuNhap ctphieuNhap)
        {
            _context.CtphieuNhap.Add(ctphieuNhap);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCtphieuNhap", new { id = ctphieuNhap.Id }, ctphieuNhap);
        }

        // DELETE: api/CtphieuNhaps/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CtphieuNhap>> DeleteCtphieuNhap(int id)
        {
            var ctphieuNhap = await _context.CtphieuNhap.FindAsync(id);
            if (ctphieuNhap == null)
            {
                return NotFound();
            }

            _context.CtphieuNhap.Remove(ctphieuNhap);
            await _context.SaveChangesAsync();

            return ctphieuNhap;
        }

        private bool CtphieuNhapExists(int id)
        {
            return _context.CtphieuNhap.Any(e => e.Id == id);
        }
    }
}
