using AppLanches.Models;
using SQLite;

namespace AppLanches.Services;

public class FavoritosService
{
    private readonly SQLiteAsyncConnection _database;

    public FavoritosService()
    {
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "favoritos.db");
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<ProdutoFavorito>().Wait();
    }

    public async Task<ProdutoFavorito> ReadAsync(int id)
    {
        try
        {
            return await _database.Table<ProdutoFavorito>().Where(p => p.ProdutoId == id).FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<ProdutoFavorito>> ReadAllAsync()
    {
        try
        {
            return await _database.Table<ProdutoFavorito>().ToListAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task CreateAsync(ProdutoFavorito produtoFavorito)
    {
        try
        {
            await _database.InsertAsync(produtoFavorito);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteAsync(ProdutoFavorito produtoFavorito)
    {
        try
        {
            await _database.DeleteAsync(produtoFavorito);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
