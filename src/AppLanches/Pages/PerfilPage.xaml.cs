using AppLanches.Services;
using AppLanches.Validations;

namespace AppLanches.Pages;

public partial class PerfilPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    private bool _loginPageDisplayed = false;

    public PerfilPage(ApiService apiService, IValidator validator)
    {
        InitializeComponent();
        LblNomeUsuario.Text = Preferences.Get("usuarionome", string.Empty);
        _apiService = apiService;
        _validator = validator;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        ImgBtnPerfil.Source = await GetImagemPerfil();
    }

    private async Task<string?> GetImagemPerfil()
    {
        string imagemPadrao = AppConfig.PerfilImagemPadrao;

        var (response, errorMessage) = await _apiService.GetImagemPerfilUsuario();

        if (errorMessage is not null)
        {
            switch (errorMessage)
            {
                case "Unauthorized":
                    if (!_loginPageDisplayed)
                    {
                        await DisplayLoginPage();
                        return null;
                    }
                    break;
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

    private async void ImgBtnPerfil_Clicked(object sender, EventArgs e)
    {
        try
        {
            var imagemArray = await SelecionarImagemAsync();

            if (imagemArray is null)
            {
                await DisplayAlert("Erro", "N o foi poss vel carregar a imagem", "Ok");
                return;
            }

            ImgBtnPerfil.Source = ImageSource.FromStream(() => new MemoryStream(imagemArray));

            var response = await _apiService.UploadImagemUsuario(imagemArray);

            if (response.Data)
            {
                await DisplayAlert("", "Imagem enviada com sucesso", "Ok");
            }
            else
            {
                await DisplayAlert("Erro", response.ErrorMessage ?? "Ocorreu um erro desconhecido", "Cancela");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Ocorreu um erro inesperado: {ex.Message}", "Ok");
        }
    }

    private async Task<byte[]?> SelecionarImagemAsync()
    {
        try
        {
            var arquivo = await MediaPicker.PickPhotoAsync();

            if (arquivo is null) return null;

            using (var stream = await arquivo.OpenReadAsync())
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
        catch (FeatureNotSupportedException)
        {
            await DisplayAlert("Erro", "A funcionalidade n o   suportada no dispositivo", "Ok");
        }
        catch (PermissionException)
        {
            await DisplayAlert("Erro", "Permiss es n o concedidas para acessar a c mera ou galeria", "Ok");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro ao selecionar a imagem: {ex.Message}", "Ok");
        }

        return null;
    }

    private void TapPedidos_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PushAsync(new PedidosPage(_apiService, _validator));
    }

    private void MinhaConta_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PushAsync(new MinhaContaPage(_apiService));
    }

    private void Perguntas_Tapped(object sender, TappedEventArgs e)
    {

    }

    private void BtnLogout_Clicked(object sender, EventArgs e)
    {

    }

    private async Task DisplayLoginPage()
    {
        _loginPageDisplayed = true;
        await Navigation.PushAsync(new LoginPage(_apiService, _validator));
    }
}