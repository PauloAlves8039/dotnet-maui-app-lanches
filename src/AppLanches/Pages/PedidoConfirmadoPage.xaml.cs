namespace AppLanches.Pages;

public partial class PedidoConfirmadoPage : ContentPage
{
	public PedidoConfirmadoPage()
	{
		InitializeComponent();
	}

    private async void BtnRetornar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}