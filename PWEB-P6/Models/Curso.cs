namespace PWEB_P6.Models
{
    public class Curso
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Disponivel { get; set; }
        public string Categoria { get; set; }
        public string Descricao { get; set; }
        public string DescricaoResumida { get; set; }
        public string Requisitos { get; set; }
        public int IdadeMinima { get; set; }
    }
}
