using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PWEB_P6.Models
{
    public class Curso
    {
        public int Id { get; set; }

        [Display(Name = "Nome", Description = "Nome do Curso", Prompt = "Introduza o Nome do Curso")]
        public string Nome { get; set; }

        [Display(Name = "Disponível?", Description = "Disponibilidade do Curso")]
        public bool Disponivel { get; set; }
        // public string Categoria { get; set; }

        [Display(Name = "Descrição", Description = "Detalhes sobre o Curso", Prompt = "Introduza Detalhes Sobre o Curso")]
        [StringLength(9999999, ErrorMessage = "Descrição demasiado grande!")]
        public string Descricao { get; set; }

        [Display(Name = "Resumo da Descrição", Description = "Apenas o mais Importante da Descrição", Prompt = "Introduza um Resumo da Descrição")]
        public string DescricaoResumida { get; set; }

        [Display(Prompt = "Introduza o que é Necessário para poder Ingressar no Curso")]
        public string Requisitos { get; set; }

        [Display(Name = "Idade Mínima", Description = "Idade Mínima Obrigatória para Frequentar o Curso")]
        [Range(14, 150, ErrorMessage = "Idade fora dos limites compreendidos!")]
        public int IdadeMinima { get; set; }

        [Display(Name = "Preço", Description = "Custo do Curso")]
        [Range(0.50, 10000, ErrorMessage = "Idade fora dos limites compreendidos!")]
        public decimal? Preco { get; set; }

        public int? CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
    }
}
