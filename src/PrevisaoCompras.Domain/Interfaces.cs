using PrevisaoCompras.Domain.Entities;

namespace PrevisaoCompras.Domain.Interfaces;

public interface IPrevisaoService
{
    Task GerarPrevisoes(int mes, int ano);
}

public interface IPrevisaoRepository
{
    Task<List<Produto>> GetProdutosAsync();
    Task<List<HistoricoCompra>> GetHistoricoByProdutoAsync(int produtoId);
    Task<List<Previsao>> GetPrevisoesAsync(int? mes = null, int? ano = null);
    Task SalvarPrevisaoAsync(Previsao previsao);
    Task<List<HistoricoCompra>> GetHistoricoRecenteAsync(int meses = 12);
}
