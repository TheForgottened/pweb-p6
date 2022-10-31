using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PWEB_P6.Data;
using PWEB_P6.Models;

namespace PWEB_P6.Controllers
{
    public class CursosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CursosController(ApplicationDbContext context)
        {
            this._context = context;
        }

        // GET: Cursos
        public async Task<IActionResult> Index(bool? disponivel)
        {
            if (disponivel == null)
            {
                ViewData["Title"] = "Lista de Todos os Cursos";
                return View(await _context.Cursos.ToListAsync());
            }

            if (disponivel == true)
            {
                ViewData["Title"] = "Lista de Cursos Ativos";
            }
            else
            {
                ViewData["Title"] = "Lista de Cursos Inativos";
            }

            return View(await _context.Cursos.Where(c => c.Disponivel == disponivel).ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string TextoAPesquisar)
        {
            ViewData["Title"] = "Lista de Cursos com '" + TextoAPesquisar + "'";

            return View(await _context.Cursos.Where(c => c.Nome.Contains(TextoAPesquisar) || c.Descricao.Contains(TextoAPesquisar)).ToListAsync());
        }

        public async Task<IActionResult> Search(string TextoAPesquisar)
        {
            ViewData["Title"] = "Lista de Cursos com '" + TextoAPesquisar + "'";

            return View(
                    from curso in _context.Cursos
                    where curso.Nome.Contains(TextoAPesquisar) || curso.Descricao.Contains(TextoAPesquisar)
                    select curso
                );
        }

        // GET: Cursos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cursos == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // GET: Cursos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cursos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Disponivel,Categoria,Descricao,DescricaoResumida,Requisitos,IdadeMinima,Preco")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(curso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(curso);
        }

        // GET: Cursos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cursos == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }
            return View(curso);
        }

        // POST: Cursos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Disponivel,Categoria,Descricao,DescricaoResumida,Requisitos,IdadeMinima,Preco")] Curso curso)
        {
            if (id != curso.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(curso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CursoExists(curso.Id))
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
            return View(curso);
        }

        // GET: Cursos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cursos == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // POST: Cursos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cursos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cursos'  is null.");
            }
            var curso = await _context.Cursos.FindAsync(id);
            if (curso != null)
            {
                _context.Cursos.Remove(curso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(e => e.Id == id);
        }
    }
}
