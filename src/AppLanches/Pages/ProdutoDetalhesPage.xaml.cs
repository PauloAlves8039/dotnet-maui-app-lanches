using AppLanches.Models;
using AppLanches.Services;
using AppLanches.Validations;

namespace AppLanches.Pages;

public partial class ProdutoDetalhesPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    private int _produtoId;
    private bool _loginPageDisplayed = false;
    private FavoritosService _favoritosService = new FavoritosService();
    private string? _imagemUrl;

    public ProdutoDetalhesPage(int produtoId,
                                string produtoNome,
                                ApiService apiService,
                                IValidator validator)
    {
        InitializeComponent();
        _apiService = apiService;
        _validator = validator;
        _produtoId = produtoId;
        Title = produtoNome ?? "Detalhe do Produto";
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetProdutoDetalhes(_produtoId);
        AtualizaFavoritoButton();
    }

    private async Task<Produto?> GetProdutoDetalhes(int produtoId)
    {
        var (produtoDetalhe, errorMessage) = await _apiService.GetProdutoDetalhe(produtoId);

        if (errorMessage == "Unauthorized" && !_loginPageDisplayed)
        {
            await DisplayLoginPage();
            return null;
        }

        if (produtoDetalhe is null)
        {
            await DisplayAlert("Erro", errorMessage ?? "Não foi possível obter o produto.", "OK");
            return null;
        }

        if (produtoDetalhe != null)
        {
            ImagemProduto.Source = produtoDetalhe.CaminhoImagem;
            LblProdutoNome.Text = produtoDetalhe.Nome;
            LblProdutoPreco.Text = produtoDetalhe.Preco.ToString();
            LblProdutoDescricao.Text = produtoDetalhe.Detalhe;
            LblPrecoTotal.Text = produtoDetalhe.Preco.ToString();
            _imagemUrl = produtoDetalhe.CaminhoImagem;
        }
        else
        {
            await DisplayAlert("Erro", errorMessage ?? "Não foi possível obter os detalhes do produto.", "OK");
            return null;
        }
        return produtoDetalhe;
    }

    private async void ImagemBtnFavorito_Clicked(object sender, EventArgs e) 
    {
        try
        {
            var existeFavorito = await _favoritosService.ReadAsync(_produtoId);
            if (existeFavorito is not null)
            {
                await _favoritosService.DeleteAsync(existeFavorito);
            }
            else
            {
                var produtoFavorito = new ProdutoFavorito()
                {
                    ProdutoId = _produtoId,
                    IsFavorito = true,
                    Detalhe = LblProdutoDescricao.Text,
                    Nome = LblProdutoNome.Text,
                    Preco = Convert.ToDecimal(LblProdutoPreco.Text),
                    ImagemUrl = _imagemUrl
                };

                await _favoritosService.CreateAsync(produtoFavorito);
            }
            AtualizaFavoritoButton();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Ocorreu um erro: {ex.Message}", "OK");
        }
    }
    
    private void BtnAdiciona_Clicked(object sender, EventArgs e) 
    {
        if (int.TryParse(LblQuantidade.Text, out int quantidade) &&
            decimal.TryParse(LblProdutoPreco.Text, out decimal precoUnitario))
        {
            quantidade++;
            LblQuantidade.Text = quantidade.ToString();

            var precoTotal = quantidade * precoUnitario;
            LblPrecoTotal.Text = precoTotal.ToString();
        }
        else
        {
            DisplayAlert("Erro", "Valores inv lidos", "OK");
        }
    }

    private void BtnRemove_Clicked(object sender, EventArgs e) 
    {
        if (int.TryParse(LblQuantidade.Text, out int quantidade) &&
            decimal.TryParse(LblProdutoPreco.Text, out decimal precoUnitario))
        {
            quantidade = Math.Max(1, quantidade - 1);
            LblQuantidade.Text = quantidade.ToString();

            var precoTotal = quantidade * precoUnitario;
            LblPrecoTotal.Text = precoTotal.ToString();
        }
        else
        {
            DisplayAlert("Erro", "Valores inv lidos", "OK");
        }
    }
    
    private async void BtnIncluirNoCarrinho_Clicked(object sender, EventArgs e) 
    {
        try
        {
            var carrinhoCompra = new CarrinhoCompra()
            {
                Quantidade = Convert.ToInt32(LblQuantidade.Text),
                PrecoUnitario = Convert.ToDecimal(LblProdutoPreco.Text),
                ValorTotal = Convert.ToDecimal(LblPrecoTotal.Text),
                ProdutoId = _produtoId,
                ClienteId = Preferences.Get("usuarioid", 0)
            };
            var response = await _apiService.AdicionaItemNoCarrinho(carrinhoCompra);
            if (response.Data)
            {
                await DisplayAlert("Sucesso", "Item adicionado ao carrinho !", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Erro", $"Falha ao adicionar item: {response.ErrorMessage}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Ocorreu um erro: {ex.Message}", "OK");
        }
    }

    private async Task DisplayLoginPage()
    {
        _loginPageDisplayed = true;

        await Navigation.PushAsync(new LoginPage(_apiService, _validator));
    }

    private async void AtualizaFavoritoButton()
    {
        var existeFavorito = await
               _favoritosService.ReadAsync(_produtoId);

        if (existeFavorito is not null)
            ImagemBtnFavorito.Source = "heartfill";
        else
            ImagemBtnFavorito.Source = "heart";
    }
}