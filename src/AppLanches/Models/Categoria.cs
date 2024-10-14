namespace AppLanches.Models;

public class Categoria
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? UrlImagem { get; set; }
    public string? CaminhoImagem => AppConfig.BaseUrl + UrlImagem;
}
