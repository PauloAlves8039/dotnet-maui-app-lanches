namespace AppLanches.Models;

public class Pedido
{
    public string? Endereco { get; set; }
    public decimal ValorTotal { get; set; }
    public int UsuarioId { get; set; }
}
