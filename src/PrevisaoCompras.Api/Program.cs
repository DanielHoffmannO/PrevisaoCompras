using Microsoft.EntityFrameworkCore;
using PrevisaoCompras.Domain.Interfaces;
using PrevisaoCompras.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("Default") ?? "Data Source=previsao.db"));
builder.Services.AddScoped<IPrevisaoRepository, PrevisaoRepository>();
builder.Services.AddScoped<IPrevisaoService, PrevisaoService>();
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    DbSeed.Run(db);
}

app.UseCors();

app.MapGet("/api/produtos", async (IPrevisaoRepository repo) =>
    Results.Ok(await repo.GetProdutosAsync()));

app.MapGet("/api/historico/{produtoId:int}", async (int produtoId, IPrevisaoRepository repo) =>
    Results.Ok(await repo.GetHistoricoByProdutoAsync(produtoId)));

app.MapGet("/api/historico", async (IPrevisaoRepository repo) =>
    Results.Ok(await repo.GetHistoricoRecenteAsync()));

app.MapGet("/api/previsoes", async (int? mes, int? ano, IPrevisaoRepository repo) =>
    Results.Ok(await repo.GetPrevisoesAsync(mes, ano)));

app.MapPost("/api/previsoes/gerar", async (IPrevisaoService svc) =>
{
    var prox = DateTime.Now.AddMonths(1);
    await svc.GerarPrevisoes(prox.Month, prox.Year);
    return Results.Ok(new { message = $"Previsões geradas para {prox.Month}/{prox.Year}" });
});

app.MapGet("/api/dashboard", async (IPrevisaoRepository repo) =>
{
    var produtos = await repo.GetProdutosAsync();
    var historico = await repo.GetHistoricoRecenteAsync();
    var prox = DateTime.Now.AddMonths(1);
    var previsoes = await repo.GetPrevisoesAsync(prox.Month, prox.Year);

    return Results.Ok(new
    {
        produtos,
        historico = historico.Select(h => new { h.ProdutoId, produtoNome = h.Produto.Nome, h.Quantidade, h.PrecoUnitario, h.DataCompra }),
        previsoes = previsoes.Select(p => new { p.ProdutoId, produtoNome = p.Produto.Nome, p.QuantidadePrevista, p.Mes, p.Ano })
    });
});

app.UseStaticFiles();
app.MapFallbackToFile("index.html");

app.Run();
