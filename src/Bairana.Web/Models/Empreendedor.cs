using System.Text.Json.Serialization;

namespace Bairana.Web.Models;

public sealed class Empreendedor
{
    [JsonRequired]
    public string Id { get; init; } = string.Empty;
    [JsonRequired]
    public string Slug { get; init; } = string.Empty;
    [JsonRequired]
    public string Nome { get; init; } = string.Empty;
    [JsonRequired]
    public string Categoria { get; init; } = string.Empty;
    public List<string> Tags { get; init; } = [];
    [JsonRequired]
    public string Descricao { get; init; } = string.Empty;
    [JsonRequired]
    public string Whatsapp { get; init; } = string.Empty;
    [JsonRequired]
    public string Imagem { get; init; } = string.Empty;
    public string? Horario { get; init; }
    public string? Instagram { get; init; }
    public bool? Entrega { get; init; }
    public decimal? PrecoInicial { get; init; }
    [JsonRequired]
    public DateOnly AtualizadoEm { get; init; }
}
