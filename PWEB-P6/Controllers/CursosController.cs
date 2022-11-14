using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PWEB_P6.Data;
using PWEB_P6.Models;
using PWEB_P6.ViewModels;

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
            ViewData["ListaDeCategorias"] = new SelectList(_context.Categorias.ToList(), "Id", "Nome");

            if (disponivel == null)
            {
                ViewData["Title"] = "Lista de Todos os Cursos";
                return View(await _context.Cursos.Include("Categoria").ToListAsync());
            }

            if (disponivel == true)
            {
                ViewData["Title"] = "Lista de Cursos Ativos";
            }
            else
            {
                ViewData["Title"] = "Lista de Cursos Inativos";
            }

            return View(await _context.Cursos.Include("Categoria").Where(c => c.Disponivel == disponivel).ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string TextoAPesquisar, int CategoriaId)
        {
            ViewData["Title"] = "Lista de Cursos com '" + TextoAPesquisar + "'";
            ViewData["ListaDeCategorias"] = new SelectList(_context.Categorias.ToList(), "Id", "Nome");

            return View(await _context.Cursos.Include("Categoria").Where(c =>
                (c.Nome.Contains(TextoAPesquisar) || c.Descricao.Contains(TextoAPesquisar)) && c.CategoriaId == CategoriaId).ToListAsync());
        }

        public async Task<IActionResult> Search(string? textoAPesquisar)
        {
            ViewData["Title"] = "Lista de Cursos com '" + textoAPesquisar + "'";

            var pesquisaVM = new PesquisaCursoViewModel();
            pesquisaVM.TextoAPesquisar = textoAPesquisar;

            if (string.IsNullOrWhiteSpace(textoAPesquisar))
            {
                pesquisaVM.ListaDeCursos = await _context.Cursos.Include("Categoria").ToListAsync();
            }
            else
            {
                pesquisaVM.ListaDeCursos = await _context.Cursos.Include("Categoria")
                    .Where(c => c.Nome.Contains(pesquisaVM.TextoAPesquisar) || c.Descricao.Contains(pesquisaVM.TextoAPesquisar)).ToListAsync();
            }

            pesquisaVM.NumResultados = pesquisaVM.ListaDeCursos.Count;

            return View(pesquisaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search([Bind("textoAPesquisar")]
            PesquisaCursoViewModel pesquisaCurso)
        {
            ViewData["Title"] = "Lista de Cursos com '" + pesquisaCurso.TextoAPesquisar + "'";

            if (string.IsNullOrWhiteSpace(pesquisaCurso.TextoAPesquisar))
            {
                pesquisaCurso.ListaDeCursos = await _context.Cursos.Include("Categoria").ToListAsync();
            }
            else
            {
                pesquisaCurso.ListaDeCursos = await _context.Cursos.Include("Categoria")
                    .Where(c => c.Nome.Contains(pesquisaCurso.TextoAPesquisar) || c.Descricao.Contains(pesquisaCurso.TextoAPesquisar)).ToListAsync();
            }

            pesquisaCurso.NumResultados = pesquisaCurso.ListaDeCursos.Count;

            return View(pesquisaCurso);
        }

        // GET: Cursos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cursos == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos.Include("Categoria")
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
            ViewData["ListaDeCategorias"] = new SelectList(_context.Categorias.ToList(), "Id", "Nome");

            return View();
        }

        // POST: Cursos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Disponivel,Descricao,DescricaoResumida,Requisitos,IdadeMinima,Preco,CategoriaId")] Curso curso)
        {
            ModelState.Remove(nameof(curso.Categoria));

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

            ViewData["ListaDeCategorias"] = new SelectList(_context.Categorias.ToList(), "Id", "Nome", curso.CategoriaId);

            return View(curso);
        }

        // POST: Cursos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Disponivel,Descricao,DescricaoResumida,Requisitos,IdadeMinima,Preco,CategoriaId")] Curso curso)
        {
            if (id != curso.Id)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(curso.Categoria));

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

            var curso = await _context.Cursos.Include("Categoria")
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
