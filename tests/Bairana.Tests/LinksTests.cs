using Bairana.Web.Helpers;

namespace Bairana.Tests;

public class LinksTests
{
    [Fact]
    public void WhatsappEConstruidoComHostFixo() =>
        Assert.Equal($"https://wa.me/{DadosTeste.NumeroSintatico}", LinksExternos.Whatsapp(DadosTeste.NumeroSintatico));

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("+12345678")]
    [InlineData("01234567")]
    [InlineData("1234567")]
    [InlineData("1234567890123456")]
    [InlineData("12345678\n")]
    [InlineData("１２３４５６７８")]
    [InlineData("https://wa.me/12345678")]
    public void WhatsappInvalidoNaoGeraUrl(string? valor) => Assert.Null(LinksExternos.Whatsapp(valor));

    [Fact]
    public void InstagramEConstruidoComHostFixo() =>
        Assert.Equal("https://www.instagram.com/usuario_teste.2/", LinksExternos.Instagram("usuario_teste.2"));

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("javascript:alert(1)")]
    [InlineData("data:text/html,x")]
    [InlineData("file:///teste")]
    [InlineData("https://www.instagram.com/teste/")]
    [InlineData("@teste")]
    [InlineData("a..b")]
    [InlineData(".teste")]
    [InlineData("teste.")]
    [InlineData("teste\n")]
    public void InstagramInvalidoNaoGeraUrl(string? valor) => Assert.Null(LinksExternos.Instagram(valor));
}
