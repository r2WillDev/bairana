using System.Text.Json;
using System.Text.Json.Nodes;
using Bairana.Web.Models;

namespace Bairana.Tests;

internal static class DadosTeste
{
    internal static readonly DateOnly Hoje = new(2026, 9, 14);
    // Somente entrada sintática de teste. Nunca publicada na fixture nem acessada por HTTP.
    internal static string NumeroSintatico => string.Concat(Enumerable.Range(1, 8));
    internal static JsonObject Objeto() => JsonNode.Parse("""
        {"id":"emp-teste","slug":"negocio-teste","nome":"Negócio Teste","categoria":"comida",
         "tags":["lanche"],"descricao":"Exemplo fictício","whatsapp":"",
         "imagem":"images/comida.svg","atualizadoEm":"2026-09-14"}
        """)!.AsObject();

    internal static Empreendedor Registro(string? campo = null, string? valorJson = null)
    {
        var dado = Objeto();
        if (campo is not null) dado[campo] = JsonNode.Parse(valorJson!);
        return dado.Deserialize<Empreendedor>(new JsonSerializerOptions(JsonSerializerDefaults.Web))!;
    }

    internal static string Json() => new JsonArray(Objeto()).ToJsonString();
    internal static string Wwwroot => Path.Combine(AppContext.BaseDirectory, "wwwroot");
    internal static string JsonReal => File.ReadAllText(Path.Combine(Wwwroot, "data", "empreendedores.json"));
}
