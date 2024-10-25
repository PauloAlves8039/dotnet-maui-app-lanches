namespace AppLanches.Services;

public class ServiceFactory
{
    public static FavoritosService CreateFavoritosService()
    {
        return new FavoritosService();
    }
}
