using System.Net;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Bairana.Web.Components;
using Bairana.Web.Models;
using Bairana.Web.Pages;
using Bairana.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bairana.Tests;

public class CategoriasTelaTests
{
    private static async Task<string> RenderAsync<T>(string json, string caminho = "/categorias") where T : IComponent
    {
        using var handler = new JsonHandler(json);
        using var http = new HttpClient(handler) { BaseAddress = new Uri("https://example.invalid/") };
        using var services = new ServiceCollection().AddLogging()
            .AddSingleton(new EmpreendedorDataService(http, true))
            .AddSingleton<NavigationManager>(new NavegacaoTeste(caminho)).BuildServiceProvider();
        await using var renderer = new HtmlRenderer(services, services.GetRequiredService<ILoggerFactory>());
        return await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var rendered = await renderer.RenderComponentAsync<T>();
            return WebUtility.HtmlDecode(rendered.ToHtmlString());
        });
    }

    [Fact]
    public async Task RenderizaTodasAsCategoriasOficiaisNaOrdemVisual()
    {
        var html = await RenderAsync<ExplorarCategorias>(DadosTeste.JsonReal);
        var nomes = Regex.Matches(html, "<h3>([^<]+)</h3>").Select(m => m.Groups[1].Value).ToArray();
        Assert.Equal(["Comida", "Doces", "Serviços", "Beleza", "Aulas", "Produtos", "Imóveis", "Outros"], nomes);
        Assert.Equal(Categorias.Todas.Select(c => c.Nome).Order(), nomes.Order());
        Assert.Contains($"{Categorias.Todas.Count} categorias ativas", html);
    }

    [Theory]
    [InlineData(0, "0 negócios")]
    [InlineData(1, "1 negócio")]
    [InlineData(2, "2 negócios")]
    public async Task ContagensSaoDerivadasDosRegistrosComSingularEPlural(int quantidade, string esperado)
    {
        var registros = new JsonArray();
        for (var i = 0; i < quantidade; i++)
        {
            var registro = DadosTeste.Objeto();
            registro["id"] = $"emp-{i}";
            registro["slug"] = $"negocio-{i}";
            registros.Add(registro);
        }
        var doce = DadosTeste.Objeto();
        doce["categoria"] = "doces";
        registros.Add(doce);
        var html = await RenderAsync<ExplorarCategorias>(registros.ToJsonString());
        var comida = Regex.Match(html, "<a[^>]*category-tile surface-card category-tone-comida[^>]*>(.*?)</a>", RegexOptions.Singleline).Value;
        var doces = Regex.Match(html, "<a[^>]*category-tile surface-card category-tone-doces[^>]*>(.*?)</a>", RegexOptions.Singleline).Value;
        Assert.Contains($"<span>{esperado}</span>", comida);
        Assert.Contains("<span>1 negócio</span>", doces);
    }

    [Fact]
    public async Task ChipsECardsCompartilhamDestinoEVitrineNaoTemFiltro()
    {
        var html = await RenderAsync<ExplorarCategorias>(DadosTeste.JsonReal);
        Assert.Equal(2, Regex.Matches(html, "href=\"\\?categoria=doces\"").Count);
        foreach (var categoria in Categorias.Todas)
            Assert.Contains($"href=\"?categoria={categoria.Id}\"", html);
        Assert.Matches("<a href(?:=\"\")?>Ver toda a vitrine", html);
    }

    [Theory]
    [InlineData("/categorias", "categorias")]
    [InlineData("/", "")]
    [InlineData("/?categoria=doces", "")]
    public async Task BarraInferiorAtivaSomenteARotaAtual(string caminho, string href)
    {
        var html = await RenderAsync<NavegacaoInferior>("[]", caminho);
        var ativo = Regex.Matches(html, "<a[^>]*aria-current=\"page\"[^>]*>");
        Assert.Single(ativo);
        Assert.Contains($"href=\"{href}\"", ativo[0].Value);
        Assert.Equal(3, Regex.Matches(html, "<button[^>]*disabled").Count);
    }

    [Fact]
    public async Task GuiaVazioMantemCategoriasComContagemZero()
    {
        var html = await RenderAsync<ExplorarCategorias>("[]");
        Assert.Equal(8, Regex.Matches(html, "<span>0 negócios</span>").Count);
        Assert.DoesNotContain("Não foi possível carregar", html);
    }

    private sealed class JsonHandler(string json) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) });
    }

    private sealed class NavegacaoTeste : NavigationManager
    {
        public NavegacaoTeste(string caminho) => Initialize("https://example.invalid/", "https://example.invalid" + caminho);
    }
}
