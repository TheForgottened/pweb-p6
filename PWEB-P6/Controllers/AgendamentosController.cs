using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PWEB_P6.Data;
using PWEB_P6.Models;
using PWEB_P6.ViewModels;

namespace PWEB_P6.Controllers
{
    public class AgendamentosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AgendamentosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Agendamentos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Agendamentos.Include(a => a.tipoDeAula);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Agendamentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Agendamentos == null)
            {
                return NotFound();
            }

            var agendamento = await _context.Agendamentos
                .Include(a => a.tipoDeAula)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agendamento == null)
            {
                return NotFound();
            }

            return View(agendamento);
        }

        // GET: Agendamentos/Create
        public IActionResult Create()
        {
            ViewData["TipoDeAulaId"] = new SelectList(_context.TipoDeAulas, "Id", "Id");
            return View();
        }

        // POST: Agendamentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cliente,DataInicio,DataFim,DuracaoEmHoras,DuracaoEmMinutos,Preco,DataHoraDoPedido,TipoDeAulaId")] Agendamento agendamento)
        {
            ModelState.Remove(nameof(agendamento.tipoDeAula));

            if (ModelState.IsValid)
            {
                _context.Add(agendamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoDeAulaId"] = new SelectList(_context.TipoDeAulas, "Id", "Id", agendamento.TipoDeAulaId);
            return View(agendamento);
        }

        // GET: Agendamentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Agendamentos == null)
            {
                return NotFound();
            }

            var agendamento = await _context.Agendamentos.FindAsync(id);
            if (agendamento == null)
            {
                return NotFound();
            }
            ViewData["TipoDeAulaId"] = new SelectList(_context.TipoDeAulas, "Id", "Id", agendamento.TipoDeAulaId);
            return View(agendamento);
        }

        // POST: Agendamentos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cliente,DataInicio,DataFim,DuracaoEmHoras,DuracaoEmMinutos,Preco,DataHoraDoPedido,TipoDeAulaId")] Agendamento agendamento)
        {
            if (id != agendamento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(agendamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgendamentoExists(agendamento.Id))
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
            ViewData["TipoDeAulaId"] = new SelectList(_context.TipoDeAulas, "Id", "Id", agendamento.TipoDeAulaId);
            return View(agendamento);
        }

        // GET: Agendamentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Agendamentos == null)
            {
                return NotFound();
            }

            var agendamento = await _context.Agendamentos
                .Include(a => a.tipoDeAula)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agendamento == null)
            {
                return NotFound();
            }

            return View(agendamento);
        }

        // POST: Agendamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Agendamentos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Agendamentos'  is null.");
            }
            var agendamento = await _context.Agendamentos.FindAsync(id);
            if (agendamento != null)
            {
                _context.Agendamentos.Remove(agendamento);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Agendamentos/Pedido
        public IActionResult Pedido()
        {
            ViewData["TipoDeAulaId"] = new SelectList(_context.TipoDeAulas.ToList(), "Id", "Nome");

            return View();
        }

        // POST: Agendamentos/Calcular
        [HttpPost]
        public async Task<IActionResult> Calcular([Bind("Cliente,DataInicio,DataFim,TipoDeAulaId")] AgendamentoViewModel pedido)
        {
            if (pedido.DataInicio > pedido.DataFim)
            {
                ModelState.AddModelError("DataInicio", "A data de inicio deve ser sempre anterior à data de fim");
            }

            var tipoDeAula = _context.TipoDeAulas.Find(pedido.TipoDeAulaId);
            if (tipoDeAula == null)
            {
                ModelState.AddModelError("TipoDeAulaId", "Tipo de aula não existe");
            }

            if (!ModelState.IsValid)
            {
                ViewData["TipoDeAulaId"] = new SelectList(_context.TipoDeAulas.ToList(), "Id", "Nome");

                return View("Pedido");
            }
            
            var agendamento = new Agendamento();

            agendamento.Cliente = pedido.Cliente;
            agendamento.DataInicio = pedido.DataInicio;
            agendamento.DataFim = pedido.DataFim;
            agendamento.TipoDeAulaId = pedido.TipoDeAulaId;
            agendamento.tipoDeAula = tipoDeAula;

            agendamento.DuracaoEmHoras = (int) Math.Floor((agendamento.DataFim - agendamento.DataInicio).TotalHours);
            agendamento.DuracaoEmMinutos = (int)Math.Floor((agendamento.DataFim - agendamento.DataInicio).TotalMinutes);
            agendamento.Preco = tipoDeAula.ValorHora * agendamento.DuracaoEmHoras;
            agendamento.DataHoraDoPedido = DateTime.Now;

            return View("PedidoConfirmacao", agendamento);
        }

        private bool AgendamentoExists(int id)
        {
          return _context.Agendamentos.Any(e => e.Id == id);
        }
    }
}
