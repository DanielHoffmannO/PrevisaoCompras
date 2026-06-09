using Microsoft.EntityFrameworkCore;
using PrevisaoCompras.Domain.Entities;
using PrevisaoCompras.Domain.Interfaces;

namespace PrevisaoCompras.Persistence;

public class PrevisaoRepository : IPrevisaoRepository
{
    private readonly AppDbContext _db;
    public PrevisaoRepository(AppDbContext db) => _db = db;

    public Task<List<Produto>> GetProdutosAsync()
        => _db.Produtos.ToListAsync();

    public Task<List<HistoricoCompra>> GetHistoricoByProdutoAsync(int produtoId)
        => _db.Historicos.Where(h => h.ProdutoId == produtoId).OrderBy(h => h.DataCompra).ToListAsync();

    public Task<List<Previsao>> GetPrevisoesAsync(int? mes = null, int? ano = null)
    {
        var q = _db.Previsoes.Include(p => p.Produto).AsQueryable();
        if (mes.HasValue) q = q.Where(p => p.Mes == mes.Value);
        if (ano.HasValue) q = q.Where(p => p.Ano == ano.Value);
        return q.OrderBy(p => p.Produto.Nome).ToListAsync();
    }

    public async Task SalvarPrevisaoAsync(Previsao previsao)
    {
        var existente = await _db.Previsoes.FirstOrDefaultAsync(p => p.ProdutoId == previsao.ProdutoId && p.Mes == previsao.Mes && p.Ano == previsao.Ano);
        if (existente != null)
        {
            existente.QuantidadePrevista = previsao.QuantidadePrevista;
            existente.GeradaEm = previsao.GeradaEm;
        }
        else
            await _db.Previsoes.AddAsync(previsao);
        await _db.SaveChangesAsync();
    }

    public Task<List<HistoricoCompra>> GetHistoricoRecenteAsync(int meses = 12)
        => _db.Historicos.Include(h => h.Produto)
            .Where(h => h.DataCompra >= DateTime.Now.AddMonths(-meses))
            .OrderBy(h => h.DataCompra).ToListAsync();
}
