namespace PWEB_P6.Models
{
    public class CarrinhoItem
    {
        public int CursoId { get; set; }
        public string CursoNome { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnit { get; set; }

        public decimal SubTotal() => Quantidade * PrecoUnit;
    }
}
