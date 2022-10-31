using PWEB_P6.Models;

namespace PWEB_P6.ViewModels
{
    public class PesquisaCursoViewModel
    {
        public List<Curso> ListaDeCursos { get; set; }
        public int NumResultados { get; set; }
        public string TextoAPesquisar { get; set; }
    }
}
