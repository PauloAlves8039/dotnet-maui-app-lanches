namespace AppLanches.Models
{
    public class CarrinhoCompra
    {
        public decimal PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorTotal { get; set; }
        public int ProdutoId { get; set; }
        public int ClienteId { get; set; }
    }
}
