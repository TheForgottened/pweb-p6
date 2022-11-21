﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PWEB_P6.Data;
using PWEB_P6.Models;

namespace PWEB_P6.Controllers
{
    public class TipoDeAulasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TipoDeAulasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TipoDeAulas
        public async Task<IActionResult> Index()
        {
              return View(await _context.TipoDeAulas.ToListAsync());
        }

        // GET: TipoDeAulas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TipoDeAulas == null)
            {
                return NotFound();
            }

            var tipoDeAula = await _context.TipoDeAulas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoDeAula == null)
            {
                return NotFound();
            }

            return View(tipoDeAula);
        }

        // GET: TipoDeAulas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoDeAulas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,ValorHora")] TipoDeAula tipoDeAula)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoDeAula);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoDeAula);
        }

        // GET: TipoDeAulas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TipoDeAulas == null)
            {
                return NotFound();
            }

            var tipoDeAula = await _context.TipoDeAulas.FindAsync(id);
            if (tipoDeAula == null)
            {
                return NotFound();
            }
            return View(tipoDeAula);
        }

        // POST: TipoDeAulas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,ValorHora")] TipoDeAula tipoDeAula)
        {
            if (id != tipoDeAula.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoDeAula);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoDeAulaExists(tipoDeAula.Id))
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
            return View(tipoDeAula);
        }

        // GET: TipoDeAulas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TipoDeAulas == null)
            {
                return NotFound();
            }

            var tipoDeAula = await _context.TipoDeAulas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoDeAula == null)
            {
                return NotFound();
            }

            return View(tipoDeAula);
        }

        // POST: TipoDeAulas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TipoDeAulas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TipoDeAulas'  is null.");
            }
            var tipoDeAula = await _context.TipoDeAulas.FindAsync(id);
            if (tipoDeAula != null)
            {
                _context.TipoDeAulas.Remove(tipoDeAula);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoDeAulaExists(int id)
        {
          return _context.TipoDeAulas.Any(e => e.Id == id);
        }
    }
}
