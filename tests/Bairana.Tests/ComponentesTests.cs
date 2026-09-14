using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using Bairana.Web.Components;
using Bairana.Web.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bairana.Tests;

public class ComponentesTests
{
    // O renderer do framework verifica o HTML sem adicionar um pacote de componentes.
    private static async Task<string> RenderAsync<T>(Dictionary<string, object?> parametros) where T : IComponent
    {
        using var services = new ServiceCollection().AddLogging().BuildServiceProvider();
        await using var renderer = new HtmlRenderer(services, services.GetRequiredService<ILoggerFactory>());
        return await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var rendered = await renderer.RenderComponentAsync<T>(ParameterView.FromDictionary(parametros));
            return rendered.ToHtmlString();
        });
    }

    [Fact]
    public async Task ContatosAusentesNaoRenderizamLinks()
    {
        var html = await RenderAsync<ContatosNegocio>(new() { ["Negocio"] = DadosTeste.Registro() });
        Assert.DoesNotContain("<a", html);
    }

    [Fact]
    public async Task ContatosValidosTemHostFixoENovaAbaProtegida()
    {
        var dado = DadosTeste.Objeto();
        dado["whatsapp"] = DadosTeste.NumeroSintatico;
        dado["instagram"] = "usuario_teste";
        var registro = dado.Deserialize<Empreendedor>(new JsonSerializerOptions(JsonSerializerDefaults.Web))!;
        var html = await RenderAsync<ContatosNegocio>(new() { ["Negocio"] = registro });
        Assert.Contains($"href=\"https://wa.me/{DadosTeste.NumeroSintatico}\"", html);
        Assert.Contains("href=\"https://www.instagram.com/usuario_teste/\"", html);
        Assert.Equal(2, html.Split("rel=\"noopener noreferrer\"").Length - 1);
        Assert.Equal(2, html.Split("target=\"_blank\"").Length - 1);
    }

    [Fact]
    public async Task OpcionaisAusentesNaoGeramBlocosVazios()
    {
        var html = await RenderAsync<InformacoesNegocio>(new() { ["Negocio"] = DadosTeste.Registro() });
        Assert.DoesNotContain("<ul", html);
        Assert.DoesNotContain("<li", html);
    }

    [Fact]
    public async Task PrecoZeroEValidoEEntregaFalsaNaoEInferida()
    {
        var dado = DadosTeste.Objeto();
        dado["precoInicial"] = 0;
        dado["entrega"] = false;
        var registro = dado.Deserialize<Empreendedor>(new JsonSerializerOptions(JsonSerializerDefaults.Web))!;
        var html = WebUtility.HtmlDecode(await RenderAsync<InformacoesNegocio>(new() { ["Negocio"] = registro }));
        Assert.Contains("0,00", html);
        Assert.DoesNotContain("Entrega no condomínio", html);
        Assert.DoesNotContain("Horário declarado", html);
    }

    [Fact]
    public async Task TextoDeNegocioEHtmlEscapado()
    {
        const string texto = "<img src=x onerror=alert(1)>";
        var html = await RenderAsync<InformacoesNegocio>(new() { ["Negocio"] = DadosTeste.Registro("horario", JsonSerializer.Serialize(texto)) });
        Assert.DoesNotContain(texto, html);
        Assert.Contains(HtmlEncoder.Default.Encode(texto), html);
    }

    [Fact]
    public async Task ImagemInseguraRenderizaFallbackAcessivel()
    {
        var html = await RenderAsync<ImagemNegocio>(new() { ["Caminho"] = "javascript:alert(1)", ["Nome"] = "Teste" });
        Assert.Contains("role=\"img\"", html);
        Assert.DoesNotContain("<img", html);
        Assert.DoesNotContain("javascript:", html);
    }

    [Fact]
    public async Task WhatsappDaDemoEVisualMasNaoAcionavel()
    {
        var html = await RenderAsync<ContatosNegocio>(new()
        {
            ["Negocio"] = DadosTeste.Registro(),
            ["MostrarDemonstracao"] = true,
            ["ExibirInstagram"] = false
        });
        Assert.Contains("<button", html);
        Assert.Contains("disabled", html);
        Assert.DoesNotContain("href=", html);
        Assert.DoesNotContain("wa.me", html);
    }

    [Fact]
    public async Task PrecoBrasileiroTemSimboloSeparadoresEAgrupamento()
    {
        var html = WebUtility.HtmlDecode(await RenderAsync<InformacoesNegocio>(new()
        {
            ["Negocio"] = DadosTeste.Registro("precoInicial", "1234.56"),
            ["Detalhado"] = true
        }));
        Assert.Contains("R$ 1.234,56", html);
    }

    [Fact]
    public async Task CardUsaCategoriaDoContratoSemAfirmarDisponibilidadeOuVerificacao()
    {
        var html = WebUtility.HtmlDecode(await RenderAsync<EmpreendedorCard>(new() { ["Negocio"] = DadosTeste.Registro() }));
        Assert.Contains("Comida", html);
        Assert.Contains("negocio/negocio-teste", html);
        Assert.Contains("Negócio fictício", html);
        Assert.DoesNotContain("Disponível agora", html);
        Assert.DoesNotContain("verificado", html);
    }
}
