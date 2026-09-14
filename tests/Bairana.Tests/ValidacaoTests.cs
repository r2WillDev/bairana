using Bairana.Web.Models;
using Bairana.Web.Services;

namespace Bairana.Tests;

public class ValidacaoTests
{
    [Fact]
    public void DemoValidaNaoPossuiContato() =>
        Assert.Empty(EmpreendedorValidator.Validar([DadosTeste.Registro()], true, DadosTeste.Hoje));

    [Theory]
    [InlineData("id", "\" \"")]
    [InlineData("id", "null")]
    [InlineData("slug", "\"\"")]
    [InlineData("nome", "\" \"")]
    [InlineData("descricao", "\"\"")]
    [InlineData("categoria", "\"nova\"")]
    [InlineData("tags", "null")]
    [InlineData("tags", "[null]")]
    [InlineData("tags", "[\" \"]")]
    [InlineData("imagem", "\"\"")]
    [InlineData("precoInicial", "-0.01")]
    [InlineData("atualizadoEm", "\"0001-01-01\"")]
    [InlineData("atualizadoEm", "\"2026-09-15\"")]
    [InlineData("whatsapp", "null")]
    public void RejeitaCampoInvalido(string campo, string valor) =>
        Assert.NotEmpty(EmpreendedorValidator.Validar([DadosTeste.Registro(campo, valor)], true, DadosTeste.Hoje));

    [Fact]
    public void ColecaoOuRegistroNuloSaoInvalidos()
    {
        Assert.NotEmpty(EmpreendedorValidator.Validar(null, true));
        Assert.NotEmpty(EmpreendedorValidator.Validar([null], true));
        Assert.Empty(EmpreendedorValidator.Validar([], true));
    }

    [Fact]
    public void IdESlugDevemSerUnicos()
    {
        var erros = EmpreendedorValidator.Validar([DadosTeste.Registro(), DadosTeste.Registro()], true, DadosTeste.Hoje);
        Assert.Contains(erros, e => e.Contains("id duplicado"));
        Assert.Contains(erros, e => e.Contains("slug duplicado"));
    }

    [Theory]
    [InlineData("Crepe da Carol")]
    [InlineData("crépe")]
    [InlineData("/negocio/crepe")]
    [InlineData("https://example.invalid")]
    [InlineData("crepe_da_carol")]
    [InlineData("crepe--carol")]
    [InlineData("-crepe")]
    [InlineData("crepe\n")]
    public void SlugRejeitaFormatoInseguro(string slug) => Assert.False(EmpreendedorValidator.SlugValido(slug));

    [Theory]
    [InlineData("crepe-da-carol")]
    [InlineData("loja-2")]
    public void SlugAceitaFormatoOficial(string slug) => Assert.True(EmpreendedorValidator.SlugValido(slug));

    [Theory]
    [InlineData("https://example.invalid/a.png")]
    [InlineData("//example.invalid/a.png")]
    [InlineData("images/../a.svg")]
    [InlineData("images/%2e%2e/a.svg")]
    [InlineData("images/a.svg?url=x")]
    [InlineData("data:image/svg+xml,x")]
    [InlineData("javascript:alert(1)")]
    [InlineData("file:///a.png")]
    public void ImagemRejeitaCaminhoInseguro(string caminho) => Assert.False(EmpreendedorValidator.ImagemValida(caminho));

    [Fact]
    public void PilotoExigeNumeroEDemoRejeitaContato()
    {
        Assert.NotEmpty(EmpreendedorValidator.Validar([DadosTeste.Registro()], false, DadosTeste.Hoje));
        var registro = DadosTeste.Registro("whatsapp", $"\"{DadosTeste.NumeroSintatico}\"");
        Assert.Empty(EmpreendedorValidator.Validar([registro], false, DadosTeste.Hoje));
        Assert.NotEmpty(EmpreendedorValidator.Validar([registro], true, DadosTeste.Hoje));
        Assert.NotEmpty(EmpreendedorValidator.Validar([DadosTeste.Registro("instagram", "\"demo_teste\"")], true, DadosTeste.Hoje));
    }

    [Fact]
    public void CategoriasCorrespondemExatamenteAoContrato()
    {
        Assert.Equal(["comida", "doces", "imoveis", "beleza", "servicos", "aulas", "produtos", "outros"], Categorias.Todas.Select(c => c.Id));
        Assert.Equal("Imóveis", Categorias.Nome("imoveis"));
        Assert.False(Categorias.Existe("Comida"));
    }
}
