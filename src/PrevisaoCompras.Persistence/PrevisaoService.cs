using PrevisaoCompras.Domain.Entities;
using PrevisaoCompras.Domain.Interfaces;

namespace PrevisaoCompras.Persistence;

public class PrevisaoService : IPrevisaoService
{
    private readonly IPrevisaoRepository _repo;
    public PrevisaoService(IPrevisaoRepository repo) => _repo = repo;

    public async Task GerarPrevisoes(int mes, int ano)
    {
        var produtos = await _repo.GetProdutosAsync();

        foreach (var produto in produtos)
        {
            var historico = await _repo.GetHistoricoByProdutoAsync(produto.Id);
            var previstoQtd = CalcularMediaMovelPonderada(historico);

            await _repo.SalvarPrevisaoAsync(new Previsao
            {
                ProdutoId = produto.Id,
                QuantidadePrevista = previstoQtd,
                Mes = mes,
                Ano = ano,
                GeradaEm = DateTime.UtcNow
            });
        }
    }

    /// <summary>
    /// Média móvel ponderada: meses mais recentes têm peso maior.
    /// Últimos 3 meses: peso 3, 2, 1. Se tiver menos dados, usa o que tem.
    /// </summary>
    private static int CalcularMediaMovelPonderada(List<HistoricoCompra> historico)
    {
        if (historico.Count == 0) return 0;

        // Agrupa por mês e soma quantidades
        var porMes = historico
            .GroupBy(h => new { h.DataCompra.Year, h.DataCompra.Month })
            .OrderByDescending(g => g.Key.Year).ThenByDescending(g => g.Key.Month)
            .Select(g => g.Sum(x => x.Quantidade))
            .Take(6)
            .ToList();

        if (porMes.Count == 0) return 0;

        // Pesos decrescentes: mais recente = maior peso
        var pesos = new[] { 6, 5, 4, 3, 2, 1 };
        double somaPonderada = 0;
        int somaPesos = 0;

        for (int i = 0; i < porMes.Count; i++)
        {
            somaPonderada += porMes[i] * pesos[i];
            somaPesos += pesos[i];
        }

        return (int)Math.Round(somaPonderada / somaPesos);
    }
}
