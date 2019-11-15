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
    [Route("api/PhieuNhap")]
    [ApiController]
    public class PhieuNhapsController : ControllerBase
    {
        private readonly LibraryDBContext _context;

        public PhieuNhapsController(LibraryDBContext context)
        {
            _context = context;
        }

        // GET: api/PhieuNhaps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhieuNhap>>> GetPhieuNhap()
        {
            return await _context.PhieuNhap.ToListAsync();
        }

        // GET: api/PhieuNhaps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhieuNhap>> GetPhieuNhap(int id)
        {
            var phieuNhap = await _context.PhieuNhap.FindAsync(id);

            if (phieuNhap == null)
            {
                return NotFound();
            }

            return phieuNhap;
        }

        // PUT: api/PhieuNhaps/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhieuNhap(int id, PhieuNhap phieuNhap)
        {
            if (id != phieuNhap.Id)
            {
                return BadRequest();
            }

            _context.Entry(phieuNhap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhieuNhapExists(id))
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

        // POST: api/PhieuNhaps
        [HttpPost]
        public async Task<ActionResult<PhieuNhap>> PostPhieuNhap(PhieuNhap phieuNhap)
        {
            _context.PhieuNhap.Add(phieuNhap);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPhieuNhap", new { id = phieuNhap.Id }, phieuNhap);
        }

        // DELETE: api/PhieuNhaps/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PhieuNhap>> DeletePhieuNhap(int id)
        {
            var phieuNhap = await _context.PhieuNhap.FindAsync(id);
            if (phieuNhap == null)
            {
                return NotFound();
            }

            _context.PhieuNhap.Remove(phieuNhap);
            await _context.SaveChangesAsync();

            return phieuNhap;
        }

        private bool PhieuNhapExists(int id)
        {
            return _context.PhieuNhap.Any(e => e.Id == id);
        }
    }
}
