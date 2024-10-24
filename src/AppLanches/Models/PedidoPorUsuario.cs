namespace AppLanches.Models;

public class PedidoPorUsuario
{
    public int Id { get; set; }
    public decimal PedidoTotal { get; set; }
    public DateTime DataPedido { get; set; }
}
