using System.Globalization;
using System.Text;
using Bairana.Web.Models;

namespace Bairana.Web.Helpers;

public static class PesquisaEmpreendedores
{
    private static readonly StringComparer Ordem = StringComparer.Create(CultureInfo.GetCultureInfo("pt-BR"), true);

    public static IReadOnlyList<Empreendedor> Filtrar(
        IEnumerable<Empreendedor> registros, string? busca = null, string? categoria = null)
    {
        var termo = Normalizar(busca);
        return registros
            .Where(e => string.IsNullOrEmpty(categoria) || e.Categoria == categoria)
            .Where(e => termo.Length == 0 ||
                Normalizar(e.Nome).Contains(termo, StringComparison.Ordinal) ||
                Normalizar(Categorias.Nome(e.Categoria)).Contains(termo, StringComparison.Ordinal) ||
                Normalizar(e.Descricao).Contains(termo, StringComparison.Ordinal) ||
                e.Tags.Any(tag => Normalizar(tag).Contains(termo, StringComparison.Ordinal)))
            .OrderBy(e => e.Nome, Ordem)
            .ThenBy(e => e.Slug, StringComparer.Ordinal)
            .ToArray();
    }

    public static string Normalizar(string? texto)
    {
        if (string.IsNullOrWhiteSpace(texto)) return string.Empty;
        var resultado = new StringBuilder();
        foreach (var caractere in texto.Trim().Normalize(NormalizationForm.FormD))
        {
            if (CharUnicodeInfo.GetUnicodeCategory(caractere) != UnicodeCategory.NonSpacingMark)
                resultado.Append(char.ToLowerInvariant(caractere));
        }
        return resultado.ToString().Normalize(NormalizationForm.FormC);
    }
}
