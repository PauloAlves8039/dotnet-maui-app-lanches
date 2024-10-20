namespace AppLanches.Pages;

public partial class CarrinhoVazioPage : ContentPage
{
	public CarrinhoVazioPage()
	{
		InitializeComponent();
	}

    private async void BtnRetornar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}