using Microsoft.EntityFrameworkCore;
using PrevisaoCompras.Domain.Interfaces;
using PrevisaoCompras.Persistence;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("Default") ?? "Data Source=previsao.db"));
builder.Services.AddScoped<IPrevisaoRepository, PrevisaoRepository>();
builder.Services.AddScoped<IPrevisaoService, PrevisaoService>();
builder.Services.AddHostedService<PrevisaoWorker>();

var host = builder.Build();

// Seed
using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    DbSeed.Run(db);
}

host.Run();

public class PrevisaoWorker : BackgroundService
{
    private readonly IServiceProvider _sp;
    private readonly ILogger<PrevisaoWorker> _logger;

    public PrevisaoWorker(IServiceProvider sp, ILogger<PrevisaoWorker> logger)
    {
        _sp = sp;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                using var scope = _sp.CreateScope();
                var svc = scope.ServiceProvider.GetRequiredService<IPrevisaoService>();

                var proximoMes = DateTime.Now.AddMonths(1);
                _logger.LogInformation("Gerando previsões para {Mes}/{Ano}", proximoMes.Month, proximoMes.Year);
                await svc.GerarPrevisoes(proximoMes.Month, proximoMes.Year);
                _logger.LogInformation("Previsões geradas com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar previsões");
            }

            await Task.Delay(TimeSpan.FromHours(6), ct);
        }
    }
}
