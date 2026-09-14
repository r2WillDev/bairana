using Bairana.Web.Helpers;
using Bairana.Web.Services;

namespace Bairana.Tests;

public class PesquisaTests
{
    [Theory]
    [InlineData("crepe", "crepe-da-carol")]
    [InlineData("CREPE", "crepe-da-carol")]
    [InlineData("  Crépe  ", "crepe-da-carol")]
    [InlineData("manutencao", "carlos-manutencao")]
    [InlineData("IMOVEIS", "joao-corretor")]
    [InlineData("eletricista", "carlos-manutencao")]
    [InlineData("fim de tarde", "crepe-da-carol")]
    public void BuscaNosQuatroCamposIgnoraCaixaEAcentos(string busca, string slug)
    {
        var dados = EmpreendedorJson.Ler(DadosTeste.JsonReal, true, DadosTeste.Hoje);
        Assert.Equal(slug, Assert.Single(PesquisaEmpreendedores.Filtrar(dados, busca)).Slug);
    }

    [Fact]
    public void CategoriaEBuscaSaoIntersecao()
    {
        var dados = EmpreendedorJson.Ler(DadosTeste.JsonReal, true, DadosTeste.Hoje);
        Assert.Single(PesquisaEmpreendedores.Filtrar(dados, categoria: "comida"));
        Assert.Single(PesquisaEmpreendedores.Filtrar(dados, "crepe", "comida"));
        Assert.Empty(PesquisaEmpreendedores.Filtrar(dados, "crepe", "beleza"));
        Assert.Single(PesquisaEmpreendedores.Filtrar(dados, "crepe", null));
        Assert.Empty(PesquisaEmpreendedores.Filtrar(dados, "xyz-inexistente"));
        Assert.Empty(PesquisaEmpreendedores.Filtrar(dados, "emp-001"));
        Assert.Empty(PesquisaEmpreendedores.Filtrar(dados, "crepe-da-carol"));
    }

    [Fact]
    public void OrdemAlfabeticaPortuguesaEEstavel()
    {
        var dados = EmpreendedorJson.Ler(DadosTeste.JsonReal, true, DadosTeste.Hoje);
        Assert.Equal(["Ana Manicure", "Ateliê Ipê", "Aulas da Lia", "Carlos Manutenção", "Costuras da Nina", "Crepe da Carol", "João Corretor", "Morangos da Ju"],
            PesquisaEmpreendedores.Filtrar(dados, " ").Select(e => e.Nome));
        Assert.Equal(string.Empty, PesquisaEmpreendedores.Normalizar(null));
    }
}
