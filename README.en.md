🌐 [Português](README.md) | [Español](README.es.md)

# Purchase Forecast

<img width="590" height="561" alt="image" src="https://github.com/user-attachments/assets/957727a6-4626-41d9-86ed-08d9d118bee2" />

Purchase forecasting system for businesses (grocery stores, markets, restaurants) using **weighted moving average**. Analyzes consumption history and generates forecasts to avoid excess or shortage of products.

## How it Works

1. **Worker Service** runs in background every 6h, analyzes purchase history from the last 6 months
2. Applies **weighted moving average** — more recent months have higher weight (6, 5, 4, 3, 2, 1)
3. Generates quantity forecasts for the next month
4. **Dashboard** displays consumption and forecast charts

## Tech Stack

| Component | Technology |
|-----------|-----------|
| Backend | .NET 9, Worker Service |
| API | ASP.NET Core Minimal APIs |
| Database | SQLite (self-contained) |
| Algorithm | Weighted Moving Average |
| Front | HTML + Chart.js (single page) |
| Infra | Docker Compose |

## Run with Docker

```bash
docker-compose up --build
```

- **Dashboard:** http://localhost:5000
- **API:** http://localhost:5000/api/dashboard

## Run locally (no Docker)

```bash
cd src
dotnet run --project PrevisaoCompras.Api
```

SQLite database is created automatically with sample data (10 products × 12 months).

## Endpoints

| Method | Route | Description |
|--------|------|-----------|
| GET | `/api/produtos` | List all products |
| GET | `/api/historico` | Purchase history (12 months) |
| GET | `/api/historico/{id}` | History by product |
| GET | `/api/previsoes?mes=X&ano=Y` | Filtered forecasts |
| GET | `/api/dashboard` | Consolidated data (front) |
| POST | `/api/previsoes/gerar` | Force forecast generation |

## Forecast Algorithm

**Weighted Moving Average** with 6-month window:

```
Forecast = Σ(quantity_month × weight) / Σ(weights)

Weights: [6, 5, 4, 3, 2, 1] (most recent = highest weight)
```

## Architecture

```
src/
├── PrevisaoCompras.Domain       ← Entities and interfaces
├── PrevisaoCompras.Persistence  ← EF Core, Repository, Service
├── PrevisaoCompras.Worker       ← Background Service (forecast every 6h)
└── PrevisaoCompras.Api          ← Minimal API + Dashboard (wwwroot)
```