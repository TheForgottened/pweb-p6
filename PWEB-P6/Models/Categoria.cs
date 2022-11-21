using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PWEB_P6.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Display(Name = "Nome", Description = "Nome do Categoria", Prompt = "Introduza o Nome do Categoria")]
        public string Nome { get; set; }

        [Display(Name = "Descrição", Description = "Detalhes sobre o Categoria", Prompt = "Introduza Detalhes Sobre o Categoria")]
        [StringLength(9999999, ErrorMessage = "Descrição demasiado grande!")]
        public string Descricao { get; set; }

        [Display(Name = "Disponível?", Description = "Disponibilidade do Curso")]
        public bool Disponivel { get; set; }

        public ICollection<Curso> Cursos { get; set; }
    }
}
