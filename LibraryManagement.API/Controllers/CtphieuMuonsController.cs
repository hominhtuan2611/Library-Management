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
    [Route("api/ctPhieuMuon")]
    [ApiController]
    public class CtphieuMuonsController : ControllerBase
    {
        private readonly LibraryDBContext _context;

        public CtphieuMuonsController(LibraryDBContext context)
        {
            _context = context;
        }

        // GET: api/CtphieuMuons
        [HttpGet("{phieuMuonId}")]
        public async Task<ActionResult<IEnumerable<CtphieuMuon>>> GetCtphieuMuon(int phieuMuonId)
        {
            return await _context.CtphieuMuon.Include(c => c.BookNavigation).Include(c => c.PhieuMuonNavigation).Where(x => x.PhieuMuon == phieuMuonId).ToListAsync();
        }

        // GET: api/CtphieuMuons/5
        [HttpGet("Detail/{id}")]
        public async Task<ActionResult<CtphieuMuon>> GetCtphieuMuonDetail(int id)
        {
            var ctphieuMuon = await _context.CtphieuMuon
                .Include(c => c.BookNavigation)
                .Include(c => c.PhieuMuonNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ctphieuMuon == null)
            {
                return NotFound();
            }

            return ctphieuMuon;
        }

        // PUT: api/CtphieuMuons/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCtphieuMuon(int id, CtphieuMuon ctphieuMuon)
        {
            if (id != ctphieuMuon.Id)
            {
                return BadRequest();
            }

            _context.Entry(ctphieuMuon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CtphieuMuonExists(id))
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

        // POST: api/CtphieuMuons
        [HttpPost]
        public async Task<ActionResult<CtphieuMuon>> PostCtphieuMuon(CtphieuMuon ctphieuMuon)
        {
            _context.CtphieuMuon.Add(ctphieuMuon);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCtphieuMuon", new { id = ctphieuMuon.Id }, ctphieuMuon);
        }

        // DELETE: api/CtphieuMuons/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CtphieuMuon>> DeleteCtphieuMuon(int id)
        {
            var ctphieuMuon = await _context.CtphieuMuon.FindAsync(id);
            if (ctphieuMuon == null)
            {
                return NotFound();
            }

            _context.CtphieuMuon.Remove(ctphieuMuon);
            await _context.SaveChangesAsync();

            return ctphieuMuon;
        }

        private bool CtphieuMuonExists(int id)
        {
            return _context.CtphieuMuon.Any(e => e.Id == id);
        }
    }
}
