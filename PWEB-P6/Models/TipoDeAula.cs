namespace PWEB_P6.Models
{
    public class TipoDeAula
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal ValorHora { get; set; }

        public ICollection<Agendamento> Agendamentos { get; set; }
    }
}
