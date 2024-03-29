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
    [Route("api/phieuMuon")]
    [ApiController]
    public class PhieuMuonsController : ControllerBase
    {
        private readonly LibraryDBContext _context;

        public PhieuMuonsController(LibraryDBContext context)
        {
            _context = context;
        }

        // GET: api/PhieuMuon
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhieuMuon>>> GetPhieuMuon()
        {
            return await _context.PhieuMuon.Include(c => c.MaDgNavigation).Include(c => c.MaNvNavigation).ToListAsync();
        }

        // GET: api/PhieuMuon/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhieuMuon>> GetPhieuMuon(int id)
        {
            var phieuMuon = await _context.PhieuMuon
                .Include(c => c.MaDgNavigation)
                .Include(c => c.MaNvNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (phieuMuon == null)
            {
                return NotFound();
            }

            return phieuMuon;
        }

        // PUT: api/PhieuMuon/5
        [HttpPut("{id}")]
        public async Task<ActionResult<PhieuMuon>> PutPhieuMuon(int id, PhieuMuon phieuMuon)
        {
            if (id != phieuMuon.Id)
            {
                return BadRequest();
            }

            _context.Entry(phieuMuon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhieuMuonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var new_phieuMuon = await _context.PhieuMuon
                            .Include(c => c.MaDgNavigation)
                            .Include(c => c.MaNvNavigation)
                            .FirstOrDefaultAsync(m => m.Id == phieuMuon.Id);
            return new_phieuMuon;
        }

        // POST: api/PhieuMuon
        [HttpPost]
        public async Task<ActionResult<PhieuMuon>> PostPhieuMuon(PhieuMuon phieuMuon)
        {
            _context.PhieuMuon.Add(phieuMuon);
            await _context.SaveChangesAsync();

            var new_phieuMuon = await _context.PhieuMuon
                .Include(c => c.MaDgNavigation)
                .Include(c => c.MaNvNavigation)
                .FirstOrDefaultAsync(m => m.Id == phieuMuon.Id);
            return new_phieuMuon;
        }

        // DELETE: api/PhieuMuon/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PhieuMuon>> DeletePhieuMuon(int id)
        {
            var phieuMuon = await _context.PhieuMuon.FindAsync(id);
            if (phieuMuon == null)
            {
                return NotFound();
            }

            _context.PhieuMuon.Remove(phieuMuon);
            await _context.SaveChangesAsync();

            return phieuMuon;
        }

        private bool PhieuMuonExists(int id)
        {
            return _context.PhieuMuon.Any(e => e.Id == id);
        }
    }
}
