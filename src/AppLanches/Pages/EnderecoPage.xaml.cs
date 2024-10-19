namespace AppLanches.Pages;

public partial class EnderecoPage : ContentPage
{
	public EnderecoPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CarregarDadosSalvos();
    }

    private void CarregarDadosSalvos()
    {
        if (Preferences.ContainsKey("nome")) 
        {
            EntNome.Text = Preferences.Get("nome", string.Empty);
        }

        if (Preferences.ContainsKey("endereco")) 
        {
            EntEndereco.Text = Preferences.Get("endereco", string.Empty);
        }


        if (Preferences.ContainsKey("telefone")) 
        {
            EntTelefone.Text = Preferences.Get("telefone", string.Empty);
        }
    }

    private void BtnSalvar_Clicked(object sender, EventArgs e)
    {
        Preferences.Set("nome", EntNome.Text);
        Preferences.Set("endereco", EntEndereco.Text);
        Preferences.Set("telefone", EntTelefone.Text);
        Navigation.PopAsync();
    }
}