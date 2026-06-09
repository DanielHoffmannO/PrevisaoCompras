using Microsoft.EntityFrameworkCore;
using PrevisaoCompras.Domain.Entities;

namespace PrevisaoCompras.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<HistoricoCompra> Historicos => Set<HistoricoCompra>();
    public DbSet<Previsao> Previsoes => Set<Previsao>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Produto>(e => { e.HasKey(p => p.Id); e.Property(p => p.Nome).HasMaxLength(100); });
        mb.Entity<HistoricoCompra>(e =>
        {
            e.HasKey(h => h.Id);
            e.HasOne(h => h.Produto).WithMany().HasForeignKey(h => h.ProdutoId);
        });
        mb.Entity<Previsao>(e =>
        {
            e.HasKey(p => p.Id);
            e.HasOne(p => p.Produto).WithMany().HasForeignKey(p => p.ProdutoId);
        });
    }
}

public static class DbSeed
{
    public static void Run(AppDbContext db)
    {
        if (db.Produtos.Any()) return;

        var produtos = new[]
        {
            new Produto { Nome = "Arroz 5kg", Categoria = "Grãos", Unidade = "pct" },
            new Produto { Nome = "Feijão 1kg", Categoria = "Grãos", Unidade = "pct" },
            new Produto { Nome = "Leite Integral", Categoria = "Laticínios", Unidade = "L" },
            new Produto { Nome = "Banana", Categoria = "Frutas", Unidade = "kg" },
            new Produto { Nome = "Tomate", Categoria = "Hortifruti", Unidade = "kg" },
            new Produto { Nome = "Peito de Frango", Categoria = "Carnes", Unidade = "kg" },
            new Produto { Nome = "Café 500g", Categoria = "Bebidas", Unidade = "pct" },
            new Produto { Nome = "Açúcar 1kg", Categoria = "Mercearia", Unidade = "pct" },
            new Produto { Nome = "Óleo de Soja", Categoria = "Mercearia", Unidade = "L" },
            new Produto { Nome = "Macarrão 500g", Categoria = "Massas", Unidade = "pct" },
        };
        db.Produtos.AddRange(produtos);
        db.SaveChanges();

        // Gerar 12 meses de histórico para cada produto
        var rng = new Random(42);
        var baseQuantidades = new[] { 4, 6, 12, 5, 4, 3, 4, 3, 2, 5 };
        var basePrecos = new[] { 28.90m, 8.50m, 5.90m, 6.50m, 8.00m, 22.00m, 18.90m, 5.50m, 9.90m, 4.50m };

        for (int mes = 1; mes <= 12; mes++)
        {
            for (int i = 0; i < produtos.Length; i++)
            {
                var variacao = rng.Next(-2, 4);
                var qtd = Math.Max(1, baseQuantidades[i] + variacao);
                db.Historicos.Add(new HistoricoCompra
                {
                    ProdutoId = produtos[i].Id,
                    Quantidade = qtd,
                    PrecoUnitario = basePrecos[i] + (decimal)(rng.NextDouble() * 3 - 1),
                    DataCompra = new DateTime(2025, mes, rng.Next(1, 28))
                });
            }
        }
        db.SaveChanges();
    }
}
