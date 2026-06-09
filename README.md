# Previsão de Compras

Sistema de previsão de compras para estabelecimentos (mercado, feira, restaurante) usando **média móvel ponderada**. Analisa o histórico de consumo e gera previsões para evitar excesso ou falta de produtos.

## Como funciona

1. **Worker Service** roda em background a cada 6h, analisa o histórico de compras dos últimos 6 meses
2. Aplica **média móvel ponderada** — meses mais recentes têm peso maior (6, 5, 4, 3, 2, 1)
3. Gera previsões de quantidade para o próximo mês
4. **Dashboard** exibe gráficos de consumo e previsão

## Tech Stack

| Componente | Tecnologia |
|-----------|-----------|
| Backend | .NET 9, Worker Service |
| API | ASP.NET Core Minimal APIs |
| Banco | SQLite (auto-contido) |
| Algoritmo | Média Móvel Ponderada |
| Front | HTML + Chart.js (single page) |
| Infra | Docker Compose |

## Executar com Docker

```bash
docker-compose up --build
```

- **Dashboard:** http://localhost:5000
- **API:** http://localhost:5000/api/dashboard

## Executar local (sem Docker)

```bash
cd src
dotnet run --project PrevisaoCompras.Api
```

O banco SQLite é criado automaticamente com dados de exemplo (10 produtos × 12 meses).

## Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/produtos` | Lista todos os produtos |
| GET | `/api/historico` | Histórico de compras (12 meses) |
| GET | `/api/historico/{id}` | Histórico por produto |
| GET | `/api/previsoes?mes=X&ano=Y` | Previsões filtradas |
| GET | `/api/dashboard` | Dados consolidados (front) |
| POST | `/api/previsoes/gerar` | Força geração de previsão |

## Algoritmo de Previsão

**Média Móvel Ponderada** com janela de 6 meses:

```
Previsão = Σ(quantidade_mês × peso) / Σ(pesos)

Pesos: [6, 5, 4, 3, 2, 1] (mais recente = maior peso)
```

Exemplo: se nos últimos 6 meses comprou [10, 8, 12, 9, 11, 7]:
```
= (10×6 + 8×5 + 12×4 + 9×3 + 11×2 + 7×1) / (6+5+4+3+2+1)
= (60 + 40 + 48 + 27 + 22 + 7) / 21
= 204 / 21 ≈ 10
```

## Estrutura

```
src/
├── PrevisaoCompras.Domain       ← Entidades e interfaces
├── PrevisaoCompras.Persistence  ← EF Core, Repository, Service
├── PrevisaoCompras.Worker       ← Background Service (previsão a cada 6h)
└── PrevisaoCompras.Api          ← Minimal API + Dashboard (wwwroot)
```

## Dados de Exemplo (seed)

10 produtos genéricos de mercado com 12 meses de histórico de compras variado:
Arroz, Feijão, Leite, Banana, Tomate, Frango, Café, Açúcar, Óleo, Macarrão

## Autor

Daniel Hoffmann
