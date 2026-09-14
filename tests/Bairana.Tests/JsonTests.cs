using System.Text.Json;
using System.Text.Json.Nodes;
using Bairana.Web.Models;
using Bairana.Web.Services;

namespace Bairana.Tests;

public class JsonTests
{
    [Fact]
    public void JsonRealEValidoFicticioESuasImagensExistem()
    {
        var dados = EmpreendedorJson.Ler(DadosTeste.JsonReal, true, DadosTeste.Hoje);
        Assert.Equal(8, dados.Count);
        Assert.Equal(dados.Count, dados.Select(e => e.Id).Distinct().Count());
        Assert.Equal(dados.Count, dados.Select(e => e.Slug).Distinct().Count());
        Assert.All(dados, e =>
        {
            Assert.True(Categorias.Existe(e.Categoria));
            Assert.Contains("fictício", e.Descricao);
            Assert.Equal(string.Empty, e.Whatsapp);
            Assert.Null(e.Instagram);
            Assert.True(File.Exists(Path.Combine(DadosTeste.Wwwroot, e.Imagem.TrimStart('/'))));
        });
    }

    [Theory]
    [InlineData("id")]
    [InlineData("slug")]
    [InlineData("nome")]
    [InlineData("categoria")]
    [InlineData("descricao")]
    [InlineData("whatsapp")]
    [InlineData("imagem")]
    [InlineData("atualizadoEm")]
    public void CampoObrigatorioAusenteFalha(string campo)
    {
        var dado = DadosTeste.Objeto();
        dado.Remove(campo);
        Assert.Throws<JsonException>(() => EmpreendedorJson.Ler(new JsonArray(dado).ToJsonString(), true, DadosTeste.Hoje));
    }

    [Theory]
    [InlineData("2026-9-14")]
    [InlineData("14/09/2026")]
    [InlineData("2026-02-30")]
    [InlineData("2026-09-14T00:00:00")]
    public void DataDeveSerIsoValida(string valor)
    {
        var dado = DadosTeste.Objeto();
        dado["atualizadoEm"] = valor;
        Assert.Throws<JsonException>(() => EmpreendedorJson.Ler(new JsonArray(dado).ToJsonString(), true));
    }

    [Fact]
    public void CampoDesconhecidoDuplicadoOuTipoErradoFalha()
    {
        Assert.Throws<JsonException>(() => EmpreendedorJson.Ler(DadosTeste.Json().Replace("\"id\":", "\"extra\":true,\"id\":"), true));
        Assert.Throws<JsonException>(() => EmpreendedorJson.Ler(DadosTeste.Json().Replace("\"id\":", "\"id\":\"outro\",\"id\":"), true));
        Assert.Throws<JsonException>(() => EmpreendedorJson.Ler(DadosTeste.Json().Replace("\"whatsapp\":\"\"", "\"whatsapp\":123"), true));
        Assert.Throws<JsonException>(() => EmpreendedorJson.Ler("invalido", true));
        Assert.Throws<InvalidDataException>(() => EmpreendedorJson.Ler("null", true));
        Assert.Throws<InvalidDataException>(() => EmpreendedorJson.Ler("[null]", true));
    }

    [Fact]
    public void TagsOpcionaisViraramColecaoVaziaEDataNaoEModificada()
    {
        var dado = DadosTeste.Objeto();
        dado.Remove("tags");
        var registro = Assert.Single(EmpreendedorJson.Ler(new JsonArray(dado).ToJsonString(), true, DadosTeste.Hoje.AddDays(3)));
        Assert.Empty(registro.Tags);
        Assert.Equal(DadosTeste.Hoje, registro.AtualizadoEm);
    }
}
