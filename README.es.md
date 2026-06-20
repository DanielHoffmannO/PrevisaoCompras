🌐 [Português](README.md) | [English](README.en.md)

# Previsión de Compras

<img width="590" height="561" alt="image" src="https://github.com/user-attachments/assets/957727a6-4626-41d9-86ed-08d9d118bee2" />

Sistema de previsión de compras para establecimientos (supermercado, feria, restaurante) usando **media móvil ponderada**. Analiza el historial de consumo y genera previsiones para evitar exceso o falta de productos.

## Cómo Funciona

1. **Worker Service** se ejecuta en background cada 6h, analiza el historial de compras de los últimos 6 meses
2. Aplica **media móvil ponderada** — meses más recientes tienen mayor peso (6, 5, 4, 3, 2, 1)
3. Genera previsiones de cantidad para el próximo mes
4. **Dashboard** muestra gráficos de consumo y previsión

## Tech Stack

| Componente | Tecnología |
|-----------|-----------|
| Backend | .NET 9, Worker Service |
| API | ASP.NET Core Minimal APIs |
| Base de datos | SQLite (autocontenido) |
| Algoritmo | Media Móvil Ponderada |
| Front | HTML + Chart.js (single page) |
| Infra | Docker Compose |

## Ejecutar con Docker

```bash
docker-compose up --build
```

- **Dashboard:** http://localhost:5000
- **API:** http://localhost:5000/api/dashboard

## Ejecutar local (sin Docker)

```bash
cd src
dotnet run --project PrevisaoCompras.Api
```

La base de datos SQLite se crea automáticamente con datos de ejemplo (10 productos × 12 meses).

## Endpoints

| Método | Ruta | Descripción |
|--------|------|-----------|
| GET | `/api/produtos` | Lista todos los productos |
| GET | `/api/historico` | Historial de compras (12 meses) |
| GET | `/api/historico/{id}` | Historial por producto |
| GET | `/api/previsoes?mes=X&ano=Y` | Previsiones filtradas |
| GET | `/api/dashboard` | Datos consolidados (front) |
| POST | `/api/previsoes/gerar` | Forzar generación de previsión |

## Algoritmo de Previsión

**Media Móvil Ponderada** con ventana de 6 meses:

```
Previsión = Σ(cantidad_mes × peso) / Σ(pesos)

Pesos: [6, 5, 4, 3, 2, 1] (más reciente = mayor peso)
```

## Arquitectura

```
src/
├── PrevisaoCompras.Domain       ← Entidades e interfaces
├── PrevisaoCompras.Persistence  ← EF Core, Repository, Service
├── PrevisaoCompras.Worker       ← Background Service (previsión cada 6h)
└── PrevisaoCompras.Api          ← Minimal API + Dashboard (wwwroot)
```

## Autor

Daniel Hoffmann
