namespace Bairana.Web.Models;

public static class Categorias
{
    public static IReadOnlyList<(string Id, string Nome)> Todas { get; } = Array.AsReadOnly(
        new (string Id, string Nome)[]
        {
            ("comida", "Comida"), ("doces", "Doces"), ("imoveis", "Imóveis"),
            ("beleza", "Beleza"), ("servicos", "Serviços"), ("aulas", "Aulas"),
            ("produtos", "Produtos"), ("outros", "Outros")
        });

    public static bool Existe(string? id) => Todas.Any(c => c.Id == id);
    public static string Nome(string id) => Todas.First(c => c.Id == id).Nome;
}
