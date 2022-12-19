namespace PWEB_P6.Models
{
    public class Carrinho
    {
        public List<CarrinhoItem> items { get; set; } = new List<CarrinhoItem>();

        public void AddItem(Curso curso, int qtd)
        {
            var item = items.FirstOrDefault(i => i.CursoId == curso.Id);

            if (item != null)
            {
                item.Quantidade += qtd;
                return;
            }

            items.Add(new CarrinhoItem
            {
                CursoId = curso.Id,
                CursoNome = curso.Nome,
                PrecoUnit = curso.Preco.GetValueOrDefault(),
                Quantidade = qtd
            });
        }

        public void RemoveItem(Curso curso) => items.RemoveAll(i => i.CursoId == curso.Id);
        public int TotalItems() => items.Sum(i => i.Quantidade);
        public decimal Total() => items.Sum(i => i.PrecoUnit * i.Quantidade);
        public void Clear() => items.Clear();
    }
}
