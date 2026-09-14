using System.Text.RegularExpressions;
using Bairana.Web.Helpers;

namespace Bairana.Web.Models;

public static partial class EmpreendedorValidator
{
    public static IReadOnlyList<string> Validar(
        IReadOnlyList<Empreendedor?>? registros, bool demonstracao = false, DateOnly? hoje = null)
    {
        List<string> erros = [];
        if (registros is null)
        {
            erros.Add("Coleção: obrigatória.");
            return erros;
        }

        var ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var slugs = new HashSet<string>(StringComparer.Ordinal);
        var dataLimite = hoje ?? DateOnly.FromDateTime(DateTime.Today);
        for (var i = 0; i < registros.Count; i++)
        {
            var registro = registros[i];
            void Erro(string campo) => erros.Add($"Registro {i + 1}: {campo} inválido ou ausente.");
            if (registro is null)
            {
                Erro("registro");
                continue;
            }

            if (string.IsNullOrWhiteSpace(registro.Id) || registro.Id != registro.Id.Trim()) Erro("id");
            else if (!ids.Add(registro.Id)) Erro("id duplicado");
            if (!SlugValido(registro.Slug)) Erro("slug");
            else if (!slugs.Add(registro.Slug)) Erro("slug duplicado");
            if (string.IsNullOrWhiteSpace(registro.Nome)) Erro("nome");
            if (!Categorias.Existe(registro.Categoria)) Erro("categoria");
            if (string.IsNullOrWhiteSpace(registro.Descricao)) Erro("descricao");
            if (registro.Tags is null || registro.Tags.Any(string.IsNullOrWhiteSpace)) Erro("tags");
            if (!ImagemValida(registro.Imagem)) Erro("imagem");
            if (registro.PrecoInicial < 0) Erro("precoInicial");
            if (registro.AtualizadoEm == default || registro.AtualizadoEm > dataLimite) Erro("atualizadoEm");

            if (demonstracao)
            {
                // A demo nunca publica contatos, mesmo que sejam sintaticamente válidos.
                if (registro.Whatsapp != string.Empty) Erro("whatsapp na demonstração");
                if (!string.IsNullOrEmpty(registro.Instagram)) Erro("instagram na demonstração");
            }
            else
            {
                if (!LinksExternos.WhatsappValido(registro.Whatsapp)) Erro("whatsapp");
                if (!string.IsNullOrEmpty(registro.Instagram) && LinksExternos.Instagram(registro.Instagram) is null)
                    Erro("instagram");
            }
        }
        return erros;
    }

    public static bool SlugValido(string? slug) => slug is not null && SlugRegex().IsMatch(slug);
    public static bool ImagemValida(string? caminho) => caminho is not null && ImagemRegex().IsMatch(caminho);

    [GeneratedRegex(@"\A[a-z0-9]+(?:-[a-z0-9]+)*\z", RegexOptions.CultureInvariant)]
    private static partial Regex SlugRegex();

    [GeneratedRegex(@"\A/?images/(?:[a-z0-9_-]+/)*[a-z0-9_-]+\.(?:svg|webp|png|jpg|jpeg|avif)\z", RegexOptions.CultureInvariant)]
    private static partial Regex ImagemRegex();
}
