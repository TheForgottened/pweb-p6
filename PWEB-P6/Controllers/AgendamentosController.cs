using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public AgendamentosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Agendamentos
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Agendamentos
                .Include(a => a.tipoDeAula)
                .Include(a => a.ApplicationUser);

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
                .Include(a => a.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agendamento == null)
            {
                return NotFound();
            }

            return View(agendamento);
        }

        // GET: Agendamentos/Create
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,DataInicio,DataFim,DuracaoEmHoras,DuracaoEmMinutos,Preco,DataHoraDoPedido,TipoDeAulaId")] Agendamento agendamento)
        {
            ModelState.Remove(nameof(agendamento.tipoDeAula));
            ModelState.Remove(nameof(agendamento.ApplicationUser));
            ModelState.Remove(nameof(agendamento.ApplicationUserId));

            agendamento.ApplicationUserId = _userManager.GetUserId(User);

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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DataInicio,DataFim,DuracaoEmHoras,DuracaoEmMinutos,Preco,DataHoraDoPedido,TipoDeAulaId")] Agendamento agendamento)
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> Calcular([Bind("DataInicio,DataFim,TipoDeAulaId")] AgendamentoViewModel pedido)
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

            agendamento.DataInicio = pedido.DataInicio;
            agendamento.DataFim = pedido.DataFim;
            agendamento.TipoDeAulaId = pedido.TipoDeAulaId;
            agendamento.tipoDeAula = tipoDeAula;
            agendamento.ApplicationUserId = _userManager.GetUserId(User);

            agendamento.DuracaoEmHoras = (int) Math.Floor((agendamento.DataFim - agendamento.DataInicio).TotalHours);
            agendamento.DuracaoEmMinutos = (int)Math.Floor((agendamento.DataFim - agendamento.DataInicio).TotalMinutes);
            agendamento.Preco = tipoDeAula.ValorHora * agendamento.DuracaoEmHoras;
            agendamento.DataHoraDoPedido = DateTime.Now;

            return View("PedidoConfirmacao", agendamento);
        }

        // GET: Agendamentos/OsMeusAgendamentos
        public IActionResult OsMeusAgendamentos()
        {
            var listaAgendamentos = _context.Agendamentos
                .Include(a => a.tipoDeAula)
                .Include(a => a.ApplicationUser)
                .Where(a => a.ApplicationUserId == _userManager.GetUserId(User));

            return View(listaAgendamentos);
        }

        private bool AgendamentoExists(int id)
        {
          return _context.Agendamentos.Any(e => e.Id == id);
        }
    }
}
