namespace AppLanches.Models;

public class ImagemPerfil
{
    public string? UrlImagem { get; set; }
    public string? CaminhoImagem => AppConfig.BaseUrl + UrlImagem;
}
