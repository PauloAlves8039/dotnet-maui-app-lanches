using AppLanches.Services;

namespace AppLanches.Pages;

public partial class MinhaContaPage : ContentPage
{
    private readonly ApiService _apiService;
    private const string NomeUsuarioKey = "usuarionome";
    private const string EmailUsuarioKey = "usuarioemail";
    private const string TelefoneUsuarioKey = "usuariotelefone";

    public MinhaContaPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        CarregarInformacoesUsuario();
        ImgBtnPerfil.Source = await GetImagemPerfilAsync();
    }

    private void CarregarInformacoesUsuario()
    {
        LblNomeUsuario.Text = Preferences.Get(NomeUsuarioKey, string.Empty);
        EntNome.Text = LblNomeUsuario.Text;
        EntEmail.Text = Preferences.Get(EmailUsuarioKey, string.Empty);
        EntFone.Text = Preferences.Get(TelefoneUsuarioKey, string.Empty);
    }

    private async Task<string?> GetImagemPerfilAsync()
    {
        string imagemPadrao = AppConfig.PerfilImagemPadrao;

        var (response, errorMessage) = await _apiService.GetImagemPerfilUsuario();

        if (errorMessage is not null)
        {
            switch (errorMessage)
            {
                case "Unauthorized":
                    await DisplayAlert("Erro", "N o autorizado", "OK");
                    return imagemPadrao;
                default:
                    await DisplayAlert("Erro", errorMessage ?? "N o foi poss vel obter a imagem.", "OK");
                    return imagemPadrao;
            }
        }

        if (response?.UrlImagem is not null)
        {
            return response.CaminhoImagem;
        }
        return imagemPadrao;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        Preferences.Set(NomeUsuarioKey, EntNome.Text);
        Preferences.Set(EmailUsuarioKey, EntEmail.Text);
        Preferences.Set(TelefoneUsuarioKey, EntFone.Text);
        await DisplayAlert("Informa  es Salvas", "Suas informa  es foram salvas com sucesso!", "OK");
    }
}