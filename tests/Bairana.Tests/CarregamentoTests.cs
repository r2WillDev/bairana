using System.Net;
using System.Text.Json;
using Bairana.Web.Services;

namespace Bairana.Tests;

public class CarregamentoTests
{
    [Fact]
    public async Task CacheCompartilhaRequisicaoEntreChamadas()
    {
        var resposta = new TaskCompletionSource<HttpResponseMessage>();
        using var handler = new Handler(() => resposta.Task);
        using var http = new HttpClient(handler) { BaseAddress = new Uri("https://example.invalid/guia/") };
        var service = new EmpreendedorDataService(http, true);
        var primeira = service.CarregarAsync();
        var segunda = service.CarregarAsync();
        Assert.False(primeira.IsCompleted);
        resposta.SetResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(DadosTeste.Json()) });
        Assert.Same(await primeira, await segunda);
        Assert.Same(await primeira, await service.CarregarAsync());
        Assert.Equal(1, handler.Chamadas);
        Assert.Equal("https://example.invalid/guia/data/empreendedores.json", handler.UltimoUri);
    }

    [Fact]
    public async Task FalhaHttpPermiteTentarNovamenteEVazioEValido()
    {
        var falhar = true;
        using var handler = new Handler(() => Task.FromResult(falhar
            ? new HttpResponseMessage(HttpStatusCode.NotFound)
            : new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("[]") }));
        using var http = new HttpClient(handler) { BaseAddress = new Uri("https://example.invalid/") };
        var service = new EmpreendedorDataService(http, true);
        await Assert.ThrowsAsync<HttpRequestException>(() => service.CarregarAsync());
        falhar = false;
        Assert.Empty(await service.CarregarAsync());
        Assert.Equal(2, handler.Chamadas);
    }

    [Theory]
    [InlineData("json-invalido")]
    [InlineData("null")]
    public async Task JsonInvalidoNaoViraListaVazia(string json)
    {
        using var handler = new Handler(() => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) }));
        using var http = new HttpClient(handler) { BaseAddress = new Uri("https://example.invalid/") };
        var service = new EmpreendedorDataService(http, true);
        var erro = await Record.ExceptionAsync(() => service.CarregarAsync());
        Assert.True(erro is JsonException or InvalidDataException);
    }

    private sealed class Handler(Func<Task<HttpResponseMessage>> responder) : HttpMessageHandler
    {
        internal int Chamadas { get; private set; }
        internal string? UltimoUri { get; private set; }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Chamadas++;
            UltimoUri = request.RequestUri!.ToString();
            return responder();
        }
    }
}
