[>] [English](README.en.md) | [Espanol](README.es.md)

# {%} Previsao de Compras

[![.NET CI](https://github.com/DanielHoffmannO/PrevisaoCompras/actions/workflows/dotnet.yml/badge.svg)](https://github.com/DanielHoffmannO/PrevisaoCompras/actions)
![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)
![SQLite](https://img.shields.io/badge/SQLite-003B57?logo=sqlite&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker&logoColor=white)
![License](https://img.shields.io/badge/license-MIT-green)

> Sistema de previsao de compras para mercado/feira usando media movel ponderada. Analisa historico e preve quanto comprar no proximo mes.

<img width="590" height="561" alt="image" src="https://github.com/user-attachments/assets/957727a6-4626-41d9-86ed-08d9d118bee2" />

## {?} Como funciona

1. **Worker Service** roda em background a cada 6h, analisa historico dos ultimos 6 meses
2. Aplica **media movel ponderada** -- meses mais recentes pesam mais (6, 5, 4, 3, 2, 1)
3. Gera previsoes de quantidade para o proximo mes
4. **Dashboard** exibe graficos de consumo e previsao

## {=} Tech Stack

| Componente | Tecnologia |
|-----------|-----------|
| Backend | .NET 9, Worker Service |
| API | ASP.NET Core Minimal APIs |
| Banco | SQLite (auto-contido) |
| Algoritmo | Media Movel Ponderada |
| Front | HTML + Chart.js (single page) |
| Infra | Docker Compose |

## [!] Como Rodar

```bash
docker-compose up --build
```

- **Dashboard:** http://localhost:5000
- **API:** http://localhost:5000/api/dashboard

### Sem Docker

```bash
cd src
dotnet run --project PrevisaoCompras.Api
```

O banco SQLite e criado automaticamente com dados de exemplo (10 produtos x 12 meses).

## [>] Endpoints

| Metodo | Rota | O que faz |
|--------|------|-----------|
| GET | `/api/produtos` | Lista todos os produtos |
| GET | `/api/historico` | Historico de compras (12 meses) |
| GET | `/api/historico/{id}` | Historico por produto |
| GET | `/api/previsoes?mes=X&ano=Y` | Previsoes filtradas |
| GET | `/api/dashboard` | Dados consolidados (front) |
| POST | `/api/previsoes/gerar` | Forca geracao de previsao |

## [*] Algoritmo

**Media Movel Ponderada** com janela de 6 meses:

```
Previsao = soma(quantidade_mes x peso) / soma(pesos)

Pesos: [6, 5, 4, 3, 2, 1] (mais recente = maior peso)
```

Exemplo: ultimos 6 meses comprou [10, 8, 12, 9, 11, 7]:
```
= (10x6 + 8x5 + 12x4 + 9x3 + 11x2 + 7x1) / (6+5+4+3+2+1)
= (60 + 40 + 48 + 27 + 22 + 7) / 21
= 204 / 21 ~ 10
```

## {/} Arquitetura

```
src/
+-- PrevisaoCompras.Domain       <- Entidades e interfaces
+-- PrevisaoCompras.Persistence  <- EF Core, Repository, Service
+-- PrevisaoCompras.Worker       <- Background Service (previsao a cada 6h)
+-- PrevisaoCompras.Api          <- Minimal API + Dashboard (wwwroot)
```

## [$] Licenca

Este projeto esta sob a licenca [MIT](LICENSE).
