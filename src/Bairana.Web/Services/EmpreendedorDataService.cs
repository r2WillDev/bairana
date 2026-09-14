using Bairana.Web.Models;

namespace Bairana.Web.Services;

public sealed class EmpreendedorDataService(HttpClient http, bool demonstracao)
{
    private Task<IReadOnlyList<Empreendedor>>? carregamento;
    public bool Demonstracao { get; } = demonstracao;

    public async Task<IReadOnlyList<Empreendedor>> CarregarAsync()
    {
        var tarefa = carregamento ??= LerAsync();
        try
        {
            return await tarefa;
        }
        catch
        {
            // Uma falha não fica no cache: a próxima tentativa consulta o arquivo novamente.
            if (ReferenceEquals(carregamento, tarefa)) carregamento = null;
            throw;
        }
    }

    private async Task<IReadOnlyList<Empreendedor>> LerAsync()
    {
        var json = await http.GetStringAsync("data/empreendedores.json");
        return EmpreendedorJson.Ler(json, Demonstracao);
    }
}
