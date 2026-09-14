using System.Text.RegularExpressions;

namespace Bairana.Web.Helpers;

public static partial class LinksExternos
{
    // Formato internacional: 8 a 15 dígitos ASCII, sem +, espaços ou pontuação.
    public static bool WhatsappValido(string? numero) =>
        numero is not null && NumeroRegex().IsMatch(numero);

    public static string? Whatsapp(string? numero) =>
        WhatsappValido(numero) ? $"https://wa.me/{numero}" : null;

    // Somente username, sem @, URL, pontos consecutivos ou ponto nas extremidades.
    public static string? Instagram(string? username) =>
        username is not null && UsuarioRegex().IsMatch(username) && !username.Contains("..")
            ? $"https://www.instagram.com/{username}/" : null;

    [GeneratedRegex(@"\A[1-9][0-9]{7,14}\z", RegexOptions.CultureInvariant)]
    private static partial Regex NumeroRegex();

    [GeneratedRegex(@"\A[a-zA-Z0-9_](?:[a-zA-Z0-9_.]{0,28}[a-zA-Z0-9_])?\z", RegexOptions.CultureInvariant)]
    private static partial Regex UsuarioRegex();
}
