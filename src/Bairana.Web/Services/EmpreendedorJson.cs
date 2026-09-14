using System.Text.Json;
using System.Text.Json.Serialization;
using Bairana.Web.Models;

namespace Bairana.Web.Services;

public static class EmpreendedorJson
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        RespectNullableAnnotations = true,
        AllowDuplicateProperties = false
    };

    public static IReadOnlyList<Empreendedor> Ler(string json, bool demonstracao = false, DateOnly? hoje = null)
    {
        var registros = JsonSerializer.Deserialize<List<Empreendedor?>>(json, Options);
        var erros = EmpreendedorValidator.Validar(registros, demonstracao, hoje);
        if (erros.Count > 0)
            throw new InvalidDataException(string.Join(" ", erros));

        return registros!.Select(e => e!).ToArray();
    }
}
